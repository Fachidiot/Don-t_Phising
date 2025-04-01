using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    // 전역에서 접근 가능한 싱글톤 인스턴스
    public static DialogueManager Instance { get; private set; }

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;           // 대화 UI 전체 패널
    [SerializeField] private GameObject continueIcon;            // "계속" 아이콘
    [SerializeField] private TextMeshProUGUI dialogueText;       // 대사 텍스트
    [SerializeField] private GameObject[] choices;               // 선택지 버튼들

    [Header("Animation")]
    [SerializeField] private Animator dialogueAnimator;          // 패널 애니메이션 컨트롤러

    private TextMeshProUGUI[] choicesText;                       // 버튼 텍스트 저장용
    private Story currentStory;                                  // Ink 런타임 객체
    public bool dialogueIsPlaying { get; private set; } = false; // 현재 대화 중 상태

    private Coroutine displayLineCoroutine;                      // 텍스트 출력 코루틴
    private bool canContinueToNextLine = false;                  // 텍스트 다 나오고 넘어갈 수 있는지

    private List<string> dialogueLog = new List<string>();       // 대화 로그 저장용

    public int stack;                                            // 대화 스택

    private void Awake()
    {
        // 싱글톤 중복 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
    }

    private void Start()
    {
        dialoguePanel.SetActive(false);

        // 선택지 텍스트 초기화
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

        // 애니메이션 재생
        dialogueAnimator.Play("DialogueBox_Open");

        ContinueStory();
    }

    // 다음 줄 출력 또는 종료
    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string nextLine = currentStory.Continue();

            CheckForEndingTag(currentStory.currentTags); // 엔딩 태그 감지
            if (displayLineCoroutine != null) StopCoroutine(displayLineCoroutine);
            displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    // 한 줄 출력 + 타자 효과
    private IEnumerator DisplayLine(string line)
    {
        dialogueLog.Add(line); // 로그 저장
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        continueIcon.SetActive(false);
        HideChoices();
        canContinueToNextLine = false;

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.maxVisibleCharacters++;
            yield return new WaitForSeconds(0.02f);
        }

        continueIcon.SetActive(true);
        DisplayChoices();
        canContinueToNextLine = true;
    }

    // 선택지 숨기기
    private void HideChoices()
    {
        foreach (var choice in choices)
            choice.SetActive(false);
    }

    // 선택지 표시
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

    // 선택지 클릭 시 호출
    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }

    // 대화 종료 처리
    private IEnumerator ExitDialogueMode()
    {
        dialogueAnimator.Play("DialogueBox_Close"); // 닫기 애니메이션
        yield return new WaitForSeconds(0.3f); // 애니메이션 기다림

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    // 엔딩 태그 감지
    private void CheckForEndingTag(List<string> tags)
    {
        foreach (string tag in tags)
        {
            if (tag.StartsWith("ENDING_"))
            {
                string endingId = tag.Substring("ENDING_".Length);
                HandleEnding(endingId);
            }
        }
    }

    // 엔딩 처리
    private void HandleEnding(string endingId)
    {
        Debug.Log("Ending triggered: " + endingId);
        // 예시: 씬 전환, UI 표시 등
    }

    // Ink 상태 저장
    public string SaveStoryState()
    {
        return currentStory.state.ToJson();
    }

    // Ink 상태 불러오기
    public void LoadStoryState(string savedJson, TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        currentStory.state.LoadJson(savedJson);
    }

    // 대화 로그 출력
    public void PrintDialogueLog()
    {
        foreach (var line in dialogueLog)
        {
            Debug.Log(line);
        }
    }
}
