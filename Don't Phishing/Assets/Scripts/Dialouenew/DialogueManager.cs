using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// DialogueEvent를 기반으로 한 줄씩 대사를 출력하고 선택지를 제어하는 매니저
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI speakerText;         // 화자 이름 텍스트
    [SerializeField] private TextMeshProUGUI dialogueText;        // 대사 텍스트
    [SerializeField] private GameObject choiceButtonPrefab;       // 선택지 버튼 프리팹
    [SerializeField] private Transform choiceParent;              // 선택지 버튼 생성 위치
    private TagHandler tagHandler;

    private DialogueEvent currentEvent;                           // 현재 진행 중인 대화 이벤트
    private Dictionary<int, Dialogue> dialogueMap;                // ID → Dialogue 매핑
    private int currentId;                                        // 현재 대사 ID

    private bool isWaiting = false;


    private void Awake()
    {
        Instance = this;

        tagHandler = new TagHandler(animator : null);
    }

    /// <summary>
    /// 대화 이벤트 시작
    /// </summary>
    public void StartDialogue(DialogueEvent dialogueEvent)
    {
        currentEvent = dialogueEvent;
        dialogueMap = new Dictionary<int, Dialogue>();

        foreach (var line in currentEvent.lines)
            dialogueMap[line.id] = line;

        ProceedNext(0); // 보통 0번 ID부터 시작
    }

    /// <summary>
    /// 다음 대사로 진행
    /// </summary>
    public void ProceedNext(int id)
    {
        if (!dialogueMap.TryGetValue(id, out var line)) return;

        currentId = id;
        speakerText.text = line.speaker;
        dialogueText.text = line.text;

        // 메시지 시스템에 출력 (플레이어 여부에 따라 구분)
        SMSManager.Instance.SaveMessage(line.text, line.speaker.ToLower() == "player");

        //태그 먼저 처리 → 끝난 후 다음 흐름으로 넘김
        tagHandler.ProcessTags(line.tag, () =>
        {
            // 선택지 출력 또는 다음 줄로 이동
            if (!string.IsNullOrEmpty(line.choices))
            {
                ShowChoices(line.choices);
            }
            else if (line.nextId != 0)
            {
                ProceedNext(line.nextId);
            }
            else
            {
                EndDialogue();
            }
        });
    }

    /// <summary>
    /// 선택지를 생성하고 버튼에 이벤트 바인딩
    /// </summary>
    private void ShowChoices(string raw)
    {
        foreach (Transform child in choiceParent)
            Destroy(child.gameObject);

        var parts = raw.Split(',');
        foreach (var part in parts)
        {
            var split = part.Split(':');
            if (split.Length != 2) continue;

            var text = split[0];
            var nextId = int.Parse(split[1]);

            var btn = Instantiate(choiceButtonPrefab, choiceParent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = text;
            btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ProceedNext(nextId));
        }
    }

    /// <summary>
    /// 대화 종료 시 처리
    /// </summary>
    private void EndDialogue()
    {
        Debug.Log("대화 종료됨");
        // 이후 UI 비활성화 등 처리 가능
    }

    public void SetWait(bool wait)
    {
        isWaiting = wait;
        // UI 상에서 "다음" 버튼을 활성화하거나 잠금 해제 처리 가능
    }
}
