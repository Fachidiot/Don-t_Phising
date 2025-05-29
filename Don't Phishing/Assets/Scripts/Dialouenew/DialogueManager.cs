using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*  ����
 *  SMS�Ŵ����� Message�� �����ؼ� Message(name, message, MsgType.None, time.currenttime.toString());
 *  SMS�Ŵ����� InstantiateMessage(Message, meessage.type);
 */

/// <summary>
/// DialogueEvent�� ������� ��ȭ �帧�� �����ϴ� �Ŵ���
/// UI ���, ��ư ������ SMSManager���� ���
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Core Components")]
    [SerializeField] private TagHandler tagHandler; // ����� ������ ���� (����)

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
            tagHandler = new TagHandler(); // Animator�� CoroutineRunner�� �ʿ��ϸ� ����
        if (smsManager == null)
            smsManager = GetComponent<SMSManager>();
    }

    //DialogueEvent ����
    public void StartDialogue(DialogueEvent dialogueEvent)
    {
        if (dialogueEvent == null)
        {
            Debug.LogError("[DialogueManager] DialogueEvent�� null�Դϴ�. ������ �� �����ϴ�.");
            return;
        }

        currentEvent = dialogueEvent;
        InitializeDialogueMap();
        ProceedNext(1000); // ��ȭ ID ?���� ����
    }


    //���� ��� ����
    public void ProceedNext(int id)
    {
        
        if (!dialogueMap.TryGetValue(id, out var line))
        {
            Debug.LogWarning($"[DialogueManager] ID {id}�� �ش��ϴ� ��簡 �����ϴ�.");

            Debug.Log(line);
            return;
        }

        currentId = id;

        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);

        typewriterCoroutine = StartCoroutine(TypeTextCoroutine(line));
    }
      

    // Typewriter �ؽ�Ʈ ��� �� �±� ó�� �� ���� ��� ����
    private IEnumerator TypeTextCoroutine(Dialogue line)
    {

        bool isMine = IsPlayer(line.speaker);

        string currentText = "";
        SMSManager.Instance.SaveMessage("", isMine); // �� �޽��� ���� ����
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
    /// ������ ���ڿ� �Ľ� (��: "�˷��ش�:2,�˷����� �ʴ´�:3")
    /// </summary>
    // DialogueManager.cs ����
    private void ParseChoices(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            Debug.LogWarning("[DialogueManager] ������ ���ڿ��� ��� ����");
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
                Debug.LogWarning($"[DialogueManager] ������ �Ľ� ����: \"{part}\"");
            }
        }

        if (choices.Count == 0)
        {
            Debug.LogWarning("[DialogueManager] ��ȿ�� �������� ����");
            return;
        }

        if (choices.Count > 2)
            Debug.LogWarning("[DialogueManager] �������� 2���� �ʰ���. �� 2���� ���˴ϴ�.");

        SMSManager.Instance.DisplayChoiceButtons(choices.Take(2).ToList());
    }

    /// <summary>
    /// ��ȭ ����
    /// </summary>
    private void EndDialogue()
    {
        SMSManager.Instance.ClearFixedButtons();
        SMSManager.Instance.ResetApp();
        Debug.Log("[DialogueManager] ��ȭ �����");
    }

    /// <summary>
    /// �÷��̾����� Ȯ��
    /// </summary>
    private bool IsPlayer(string speaker)
    {
        return speaker.ToLower().Contains("player") || speaker == "��";
    }


    // �±� ó�� ������ ��� ������ ��� true
    public void SetWait(bool wait) => isWaiting = wait;

    /// <summary>
    /// DialogueEvent �� Dictionary<int, Dialogue> ����
    /// </summary>
    private void InitializeDialogueMap()
    {
        dialogueMap = new Dictionary<int, Dialogue>();

        if (currentEvent == null || currentEvent.lines == null || currentEvent.lines.Count == 0)
        {
            Debug.LogError("[DialogueManager] DialogueEvent�� ��� ������ �����ϴ�.");
            return;
        }

        foreach (var line in currentEvent.lines)
        {
            if (!dialogueMap.ContainsKey(line.id))
                dialogueMap[line.id] = line;
            else
                Debug.LogWarning($"[DialogueManager] �ߺ��� ��� ID �߰�: {line.id}");
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
