using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject[] choices;
    [Header("Animation")]
    [SerializeField] private Animator dialogueAnimator;

    private TextMeshProUGUI[] choicesText;
    private Story currentStory;
    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;
    public bool dialogueIsPlaying { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        dialoguePanel.SetActive(false);
        choicesText = new TextMeshProUGUI[choices.Length];
        for (int i = 0; i < choices.Length; i++)
        {
            choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    public void ContinueStory()
    {
        if (displayLineCoroutine != null)
            StopCoroutine(displayLineCoroutine);

        displayLineCoroutine = StartCoroutine(DisplayAllNPCDialogue());
    }

    private IEnumerator DisplayAllNPCDialogue()
    {
        HideChoices();

        while (currentStory.canContinue)
        {
            string line = currentStory.Continue().Trim();

            DialogueStateManager.Instance.HandleTags(currentStory.currentTags);

            // ALARM 메시지
            if (currentStory.currentTags.Contains("alarm"))
            {
                SMSManager.Instance.SaveMessage(line, false);
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            // PLAYER 메시지
            if (currentStory.currentTags.Contains("player"))
            {
                SMSManager.Instance.SaveMessage(line, true);
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            // NPC 메시지
            if (currentStory.currentTags.Contains("npc"))
            {
                SMSManager.Instance.SaveMessage("", false);
                yield return StartCoroutine(TypeText(line));
                yield return new WaitForSeconds(0.4f);
            } //ddd
        }

        DisplayChoices();
        canContinueToNextLine = true;
    }

    private IEnumerator TypeText(string fullText)
    {
        string temp = "";
        foreach (char c in fullText)
        {
            temp += c;
            SMSManager.Instance.UpdateLastNPCMessage(temp);
            yield return new WaitForSeconds(0.03f);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            StartCoroutine(WaitAndContinue());
        }
    }

    private IEnumerator WaitAndContinue()
    {
        yield return null;
        ContinueStory();
    }

    private void HideChoices()
    {
        foreach (var choice in choices)
            choice.SetActive(false);
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        for (int i = 0; i < currentChoices.Count; i++)
        {
            choices[i].SetActive(true);
            choicesText[i].text = currentChoices[i].text;
        }

        for (int i = currentChoices.Count; i < choices.Length; i++)
        {
            choices[i].SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0]);
    }

    private IEnumerator ExitDialogueMode()
    {
        dialogueAnimator.Play("DialogueBox_Close");
        yield return new WaitForSeconds(0.3f);

        dialogueIsPlaying = false;
        HideChoices();
        dialoguePanel.SetActive(false);
    }

    public void ContinueAfterWait()
    {
        DialogueStateManager.Instance.ClearState();
        ContinueStory();
    }
}
