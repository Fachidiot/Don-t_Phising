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

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private GameObject[] choices;

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

    // 대화 시작
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    // 줄 계속 출력
    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string nextLine = currentStory.Continue();

            if (displayLineCoroutine != null)
                StopCoroutine(displayLineCoroutine);

            displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
        }
        else if (currentStory.currentChoices.Count > 0)
        {
            // 줄은 없지만 선택지가 있는 경우 → 선택지 강제 표시
            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;
        continueIcon.SetActive(false);

        canContinueToNextLine = false;

        // 텍스트 출력 전 선택지 숨기기
        HideChoices();

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.maxVisibleCharacters++;
            yield return new WaitForSeconds(0.02f);
        }

        canContinueToNextLine = true;
        continueIcon.SetActive(true);

        if (currentStory.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("선택지가 UI에서 처리 가능한 개수보다 많습니다.");
        }

        for (int i = 0; i < currentChoices.Count; i++)
        {
            choices[i].gameObject.SetActive(true);
            choicesText[i].text = currentChoices[i].text;
        }

        for (int i = currentChoices.Count; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private void HideChoices()
    {
        foreach (GameObject choice in choices)
        {
            choice.SetActive(false);
        }
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0]);
    }

    public void MakeChoice(int choiceIndex)
    {
        if (!canContinueToNextLine) return;

        currentStory.ChooseChoiceIndex(choiceIndex);
        canContinueToNextLine = false;
        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        HideChoices();
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        dialogueIsPlaying = false;
    }
}
