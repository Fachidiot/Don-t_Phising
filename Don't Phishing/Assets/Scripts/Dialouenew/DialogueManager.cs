using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    /// <summary>
    /// DialogueEvent ����
    /// </summary>
    public void StartDialogue(DialogueEvent dialogueEvent)
    {
        if (dialogueEvent == null)
        {
            Debug.LogError("[DialogueManager] DialogueEvent�� null�Դϴ�. ������ �� �����ϴ�.");
            return;
        }

        currentEvent = dialogueEvent;
        InitializeDialogueMap();
        ProceedNext(0); // ��ȭ ID 0������ ����
    }

    /// <summary>
    /// ���� ��� ����
    /// </summary>
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

    /// <summary>
    /// Typewriter �ؽ�Ʈ ��� �� �±� ó�� �� ���� ��� ����
    /// </summary>
    private IEnumerator TypeTextCoroutine(Dialogue line)
    {
        bool isMine = IsPlayer(line.speaker);

        string currentText = "";
        SMSManager.Instance.SaveMessage("", isMine); // �� �޽��� ���� ����
        yield return null;

        foreach (char c in line.text)
        {
            currentText += c;
            SMSManager.Instance.UpdateLastNPCMessage(currentText);
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
    private void ParseChoices(string raw)
    {
        SMSManager.Instance.ClearChoiceButtons();

        if (string.IsNullOrEmpty(raw))
        {
            Debug.LogWarning("[DialogueManager] ������ ���ڿ��� ��� �ֽ��ϴ�.");
            return;
        }

        var parts = raw.Split(',');
        foreach (var part in parts)
        {
            var split = part.Split(':');
            if (split.Length != 2)
            {
                Debug.LogWarning($"[DialogueManager] �߸��� ������ ����: {part}");
                continue;
            }

            string choiceText = split[0].Trim();
            if (int.TryParse(split[1], out int nextId))
            {
                SMSManager.Instance.CreateChoiceButton(choiceText, nextId);
            }
            else
            {
                Debug.LogWarning($"[DialogueManager] ������ nextId �Ľ� ����: {split[1]}");
            }
        }
    }

    /// <summary>
    /// ��ȭ ����
    /// </summary>
    private void EndDialogue()
    {
        SMSManager.Instance.ClearChoiceButtons();
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

    /// <summary>
    /// �±� ó�� ������ ��� ������ ��� true
    /// </summary>
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
}
