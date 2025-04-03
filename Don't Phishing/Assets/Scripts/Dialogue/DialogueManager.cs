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
    [SerializeField] private Transform messageContainer;

    [Header("Prefabs")]
    [SerializeField] private GameObject npcMessagePrefab;
    [SerializeField] private GameObject playerMessagePrefab;

    [Header("Scroll")]
    [SerializeField] private ScrollRect scrollRect;

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
        DontDestroyOnLoad(gameObject);
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
        dialogueAnimator.Play("DialogueBox_Open");
        ContinueStory();
    }

    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string nextLine = currentStory.Continue();

            if (displayLineCoroutine != null)
                StopCoroutine(displayLineCoroutine);

            displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        HideChoices();

        SpawnNPCMessage(line);
        ScrollToBottom();

        yield return new WaitForSeconds(0.4f); // µÙ∑π¿Ã »ƒ º±≈√¡ˆ ∂ﬂ∞‘
        DisplayChoices();
        canContinueToNextLine = true;
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            string selectedText = currentStory.currentChoices[choiceIndex].text;
            SpawnPlayerMessage(selectedText);
            ScrollToBottom();

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

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    private void SpawnNPCMessage(string message)
    {
        GameObject go = Instantiate(npcMessagePrefab, messageContainer);
        go.GetComponentInChildren<TextMeshProUGUI>().text = message;
    }

    private void SpawnPlayerMessage(string message)
    {
        GameObject go = Instantiate(playerMessagePrefab, messageContainer);
        go.GetComponentInChildren<TextMeshProUGUI>().text = message;
    }
}
