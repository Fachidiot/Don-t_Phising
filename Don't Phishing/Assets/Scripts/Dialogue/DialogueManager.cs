using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    // 싱글톤 패턴
    public static DialogueManager Instance { get; private set; }

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;         // 전체 대화 패널 
    [SerializeField] private GameObject[] choices;             // 선택지 버튼들
    [SerializeField] private Transform messageContainer;       // 메시지 프리팹이 쌓일 위치 (Vertical Layout Group 등 포함)

    //[Header("Prefabs")]
    //[SerializeField] private GameObject npcMessagePrefab;      // NPC 메시지 프리팹
    //[SerializeField] private GameObject playerMessagePrefab;   // 플레이어 메시지 프리팹

    [Header("Animation")]
    [SerializeField] private Animator dialogueAnimator;        // 대화창 애니메이션

    private TextMeshProUGUI[] choicesText;                     // 선택지 텍스트 캐싱
    private Story currentStory;                                // 현재 Ink 스토리
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

        // 선택지 버튼 텍스트 캐싱
        choicesText = new TextMeshProUGUI[choices.Length];
        for (int i = 0; i < choices.Length; i++)
        {
            choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    /// <summary>
    /// Ink JSON 받아서 대화 시작
    /// </summary>
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        //dialogueAnimator.Play("DialogueBox_Open");

        ContinueStory();
    }

    /// <summary>
    /// 다음 줄로 진행 (or 종료)
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
    /// 한 줄씩 출력 (타자 효과 제외, 바로 생성)
    /// </summary>
    private IEnumerator DisplayLine(string line)
    {
        HideChoices();

        // NPC 메시지 프리팹 생성
        SpawnNPCMessage(line);

        yield return new WaitForSeconds(0.4f);

        DisplayChoices(); // 다음 선택지 출력
        canContinueToNextLine = true;
    }

    /// <summary>
    /// 플레이어 선택 반영
    /// </summary>
    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            string selectedText = currentStory.currentChoices[choiceIndex].text;

            // 플레이어 말풍선 생성
            SpawnPlayerMessage(selectedText);

            // 선택 반영
            currentStory.ChooseChoiceIndex(choiceIndex);
            StartCoroutine(WaitAndContinue());
        }
    }

    /// <summary>
    /// 1프레임 후 Continue 호출
    /// </summary>
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

    /// <summary>
    /// 첫 번째 선택지 기본 포커싱
    /// </summary>
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
    /// NPC 메시지 생성
    /// </summary>
    private void SpawnNPCMessage(string message)
    {
        //GameObject go = Instantiate(npcMessagePrefab, messageContainer);
        //go.GetComponentInChildren<TextMeshProUGUI>().text = message;

        SMSManager.Instance.SaveMessage(message, false);
    }

    /// <summary>
    /// 플레이어 메시지 생성
    /// </summary>
    private void SpawnPlayerMessage(string message)
    {
        //GameObject go = Instantiate(playerMessagePrefab, messageContainer);
        //go.GetComponentInChildren<TextMeshProUGUI>().text = message;

        SMSManager.Instance.SaveMessage(message, true);
    }
}
