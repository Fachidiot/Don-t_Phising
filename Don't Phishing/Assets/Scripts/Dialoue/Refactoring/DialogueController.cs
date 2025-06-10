using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 대화 흐름을 제어하는 컨트롤러 클래스
/// DialogueEvent 데이터를 기반으로 다음 대사 출력, 선택지 처리, 태그 처리 등을 담당
/// UI 출력 및 태그 실행은 외부 매니저로 분리됨
/// </summary>
public class DialogueController : MonoBehaviour
{
    [SerializeField] private DialogueUIManager uiManager;
    [SerializeField] private DialogueTagProcessor tagProcessor;

    private DialogueEvent currentEvent;
    private Dictionary<int, Dialogue> dialogueMap;
    private int currentId;

    [SerializeField] private float delayAfterLine = 2f; // 메시지 출력 후 고정 딜레이

    private void Awake()
    {
        if (uiManager == null)
            uiManager = FindObjectOfType<DialogueUIManager>();
        if (tagProcessor == null)
            tagProcessor = new DialogueTagProcessor();
    }

    /// <summary>
    /// 대화 시작 함수 - DialogueEvent를 받아 대화를 초기화하고 첫 대사 출력
    /// </summary>
    public void StartDialogue(DialogueEvent dialogueEvent)
    {
        currentEvent = dialogueEvent;
        BuildDialogueMap(dialogueEvent);
        ProceedNext(dialogueEvent.lines[0].id); // 시작 ID는 0번째 라인 기준
    }

    /// <summary>
    /// 다음 대사로 진행하는 함수 - ID 기반으로 대사 검색 및 출력 시작
    /// </summary>
    public void ProceedNext(int id)
    {
        if (!dialogueMap.TryGetValue(id, out Dialogue line))
        {
            Debug.LogWarning($"Dialogue ID {id} not found.");
            return;
        }

        currentId = id;

        bool isPlayer = line.speaker.ToLower().Contains("player") || line.speaker == "나";
        uiManager.ShowMessage("", isPlayer); // 빈 메시지 먼저 생성 (타자 효과 전용)

        StartCoroutine(TypeLineRoutine(line, isPlayer));
    }

    /// <summary>
    /// 대사 출력 코루틴 - 한 글자씩 출력 후 태그 처리 및 다음 분기 결정
    /// </summary>
    private IEnumerator TypeLineRoutine(Dialogue line, bool isPlayer)
    {
        string typed = "";
        foreach (char c in line.text)
        {
            typed += c;
            uiManager.UpdateLastMessage(typed);
            yield return new WaitForSeconds(0.03f);
        }

        // 태그 처리 후 2초 대기
        bool tagComplete = false;
        tagProcessor.Process(line.tag, () => tagComplete = true);
        yield return new WaitUntil(() => tagComplete);

        yield return new WaitForSeconds(delayAfterLine);

        // 다음 분기 처리
        if (!string.IsNullOrWhiteSpace(line.choices))
            uiManager.ShowChoices(ParseChoices(line.choices));
        else if (line.nextId != 0)
            ProceedNext(line.nextId);
        else
            EndDialogue();
    }

    /// <summary>
    /// 대화 종료 처리 함수 - 선택지 UI 제거 등
    /// </summary>
    private void EndDialogue()
    {
        uiManager.ClearChoices();
        Debug.Log("[DialogueController] Dialogue ended.");
    }

    /// <summary>
    /// DialogueEvent의 모든 대사 ID를 딕셔너리로 매핑
    /// </summary>
    private void BuildDialogueMap(DialogueEvent dialogueEvent)
    {
        dialogueMap = new Dictionary<int, Dialogue>();
        foreach (var line in dialogueEvent.lines)
        {
            if (!dialogueMap.ContainsKey(line.id))
                dialogueMap[line.id] = line;
            else
                Debug.LogWarning($"Duplicate dialogue ID: {line.id}");
        }
    }

    /// <summary>
    /// 선택지 문자열 파싱 함수 (ex: "예:2,아니오:3")
    /// </summary>
    private List<(string, int)> ParseChoices(string raw)
    {
        var choices = new List<(string, int)>();
        var parts = raw.Split(',');

        foreach (var part in parts)
        {
            var split = part.Split(':');
            if (split.Length == 2 && int.TryParse(split[1], out int nextId))
                choices.Add((split[0].Trim(), nextId));
        }

        return choices;
    }

    /// <summary>
    /// 현재 대사를 반환하는 함수 (외부 참조용)
    /// </summary>
    public Dialogue GetCurrentLine()
    {
        return dialogueMap.ContainsKey(currentId) ? dialogueMap[currentId] : null;
    }
}