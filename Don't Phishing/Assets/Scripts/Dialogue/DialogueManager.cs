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
    [SerializeField] private GameObject dialoguePanel;            // 대화 전체 패널
    [SerializeField] private GameObject[] choices;                // 선택지 버튼들
    [Header("Animation")]
    [SerializeField] private Animator dialogueAnimator;           // 대화창 열고 닫기 애니메이션
    //[SerializeField] private GameObject dialogueDelayAni;         // ... 애니메이션 오브젝트

    private TextMeshProUGUI[] choicesText;                        // 선택지 버튼 텍스트
    private Story currentStory;                                   // Ink 스토리
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

        // 선택지 텍스트 초기화
        choicesText = new TextMeshProUGUI[choices.Length];
        for (int i = 0; i < choices.Length; i++)
        {
            choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    /// <summary>
    /// Ink JSON 파일 받아서 대화 모드 진입
    /// </summary>
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    /// <summary>
    /// 다음 대사로 넘어가기
    /// </summary>
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

    /// <summary>
    /// 대사 한 줄 출력 (NPC/플레이어/알림 구분, 타자 효과 포함)
    /// </summary>
    private IEnumerator DisplayLine(string line)
    {
        HideChoices();

        // Ink 태그 분석
        bool isNpcLine = false;
        bool isPlayerLine = false;
        bool isAlarm = false;

        foreach (string tag in currentStory.currentTags)
        {
            if (tag == "npc") isNpcLine = true;
            if (tag == "player") isPlayerLine = true;
            if (tag.Contains("alarm")) isAlarm = true;
        }

        // 알림 효과 처리
        if (isAlarm)
        {
            SMSManager.Instance.SaveMessage(line, false); // 알림 처리 (나중에 효과 추가 가능)
            yield return new WaitForSeconds(0.5f);
        }

        // 플레이어 메시지: 바로 출력
        if (isPlayerLine)
        {
            SMSManager.Instance.SaveMessage(line, true);
            yield return new WaitForSeconds(0.5f);
        }

        // NPC 메시지: 애니메이션 + 천천히 출력
        if (isNpcLine)
        {
            string[] lines = line.Split('\n');

            foreach (string singleLine in lines)
            {
                //dialogueDelayAni.SetActive(true);
                yield return new WaitForSeconds(0.8f);
                // dialogueDelayAni.SetActive(false);

               // SMSManager.Instance.SaveMessage("", true);
                yield return StartCoroutine(TypeText(singleLine));
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(0.5f);
        }

        DisplayChoices();
        canContinueToNextLine = true;
    }

    /// <summary>
    /// 타이핑 효과: 한 글자씩 출력
    /// </summary>
    private IEnumerator TypeText(string fullText)
    {
        string temp = "";

        // 일단 NPC 메시지 생성
        SMSManager.Instance.SaveMessage("", false);

        foreach (char c in fullText)
        {
            temp += c;
            SMSManager.Instance.UpdateLastNPCMessage(temp);
            yield return new WaitForSeconds(0.03f); // 타자 효과 속도
        }
    }

    /// <summary>
    /// 플레이어 선택 반영
    /// </summary>
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

    /// <summary>
    /// 선택지 숨기기
    /// </summary>
    private void HideChoices()
    {
        foreach (var choice in choices)
            choice.SetActive(false);
    }

    /// <summary>
    /// 선택지 표시
    /// </summary>
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

    /// <summary>
    /// 대화 종료
    /// </summary>
    private IEnumerator ExitDialogueMode()
    {
        dialogueAnimator.Play("DialogueBox_Close");
        yield return new WaitForSeconds(0.3f);

        dialogueIsPlaying = false;
        HideChoices();
        dialoguePanel.SetActive(false);
    }

    /// <summary>
    /// WAIT 태그 해제
    /// </summary>
    public void ContinueAfterWait()
    {
        DialogueStateManager.Instance.ClearState();
        ContinueStory();
    }
}
