using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*  숙제
 *  SMS매니저의 Message를 생성해서 Message(name, message, MsgType.None, time.currenttime.toString());
 *  SMS매니저의 InstantiateMessage(Message, meessage.type);
 */

/// <summary>
/// DialogueEvent를 기반으로 대화 흐름을 제어하는 매니저
/// UI 출력, 버튼 생성은 SMSManager에서 담당
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Core Components")]
    [SerializeField] private TagHandler tagHandler; // 명시적 의존성 주입 (권장)

    private DialogueEvent currentEvent;
    private Dictionary<int, Dialogue> dialogueMap;
    private int currentId;
    private bool isWaiting;
    private Coroutine typewriterCoroutine;
    private SMSManager smsManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (tagHandler == null)
            tagHandler = new TagHandler(); // Animator나 CoroutineRunner가 필요하면 주입
        if (smsManager == null)
            smsManager = GetComponent<SMSManager>();
    }

    //DialogueEvent 시작
    public void StartDialogue(DialogueEvent dialogueEvent)
    {
        if (dialogueEvent == null)
        {
            Debug.LogError("[DialogueManager] DialogueEvent가 null입니다. 시작할 수 없습니다.");
            return;
        }

        currentEvent = dialogueEvent;
        InitializeDialogueMap();
        ProceedNext(1000); // 대화 ID ?부터 시작
    }


    //다음 대사 시작
    public void ProceedNext(int id)
    {
        
        if (!dialogueMap.TryGetValue(id, out var line))
        {
            Debug.LogWarning($"[DialogueManager] ID {id}에 해당하는 대사가 없습니다.");

            Debug.Log(line);
            return;
        }

        currentId = id;

        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);

        typewriterCoroutine = StartCoroutine(TypeTextCoroutine(line));
    }
      

    // Typewriter 텍스트 출력 후 태그 처리 및 다음 대사 진행
    private IEnumerator TypeTextCoroutine(Dialogue line)
    {

        bool isMine = IsPlayer(line.speaker);

        string currentText = "";
        SMSManager.Instance.SaveMessage("", isMine); // 빈 메시지 먼저 생성
        yield return new WaitForSeconds(1.5f);

        foreach (char c in line.text)
        {
            currentText += c;
            SMSManager.Instance.UpdateLastNPCMessage(currentText);
            Debug.Log(currentText);
            yield return new WaitForSeconds(0.03f);
        }

        tagHandler.ProcessTags(line.tag, () =>
        {
            if (!string.IsNullOrEmpty(line.choices))
                ParseChoices(line.choices);
            else if (line.nextId != 0)
                ProceedNext(line.nextId);
            else
                EndDialogue();
        });
    }

    /// <summary>
    /// 선택지 문자열 파싱 (예: "알려준다:2,알려주지 않는다:3")
    /// </summary>
    // DialogueManager.cs 내부
    private void ParseChoices(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            Debug.LogWarning("[DialogueManager] 선택지 문자열이 비어 있음");
            SMSManager.Instance.ClearFixedButtons();
            return;
        }

        var choices = new List<(string, int)>();
        var parts = raw.Split(',');

        foreach (var part in parts)
        {
            var split = part.Split(':');
            if (split.Length == 2 && int.TryParse(split[1].Trim(), out int nextId))
            {
                string choiceText = split[0].Trim();
                choices.Add((choiceText, nextId));
            }
            else
            {
                Debug.LogWarning($"[DialogueManager] 선택지 파싱 실패: \"{part}\"");
            }
        }

        if (choices.Count == 0)
        {
            Debug.LogWarning("[DialogueManager] 유효한 선택지가 없음");
            return;
        }

        if (choices.Count > 2)
            Debug.LogWarning("[DialogueManager] 선택지가 2개를 초과함. 앞 2개만 사용됩니다.");

        SMSManager.Instance.DisplayChoiceButtons(choices.Take(2).ToList());
    }

    /// <summary>
    /// 대화 종료
    /// </summary>
    private void EndDialogue()
    {
        SMSManager.Instance.ClearFixedButtons();
        SMSManager.Instance.ResetApp();
        Debug.Log("[DialogueManager] 대화 종료됨");
    }

    /// <summary>
    /// 플레이어인지 확인
    /// </summary>
    private bool IsPlayer(string speaker)
    {
        return speaker.ToLower().Contains("player") || speaker == "나";
    }


    // 태그 처리 등으로 대기 상태일 경우 true
    public void SetWait(bool wait) => isWaiting = wait;

    /// <summary>
    /// DialogueEvent → Dictionary<int, Dialogue> 구성
    /// </summary>
    private void InitializeDialogueMap()
    {
        dialogueMap = new Dictionary<int, Dialogue>();

        if (currentEvent == null || currentEvent.lines == null || currentEvent.lines.Count == 0)
        {
            Debug.LogError("[DialogueManager] DialogueEvent에 대사 라인이 없습니다.");
            return;
        }

        foreach (var line in currentEvent.lines)
        {
            if (!dialogueMap.ContainsKey(line.id))
                dialogueMap[line.id] = line;
            else
                Debug.LogWarning($"[DialogueManager] 중복된 대사 ID 발견: {line.id}");
        }
    }

    public void ShowSingleLine(Dialogue line)
    {
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);

            typewriterCoroutine = StartCoroutine(TypeTextCoroutine(line));
        }
    }
}
