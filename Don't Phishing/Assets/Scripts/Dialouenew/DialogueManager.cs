using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// DialogueEvent�� ������� �� �پ� ��縦 ����ϰ� �������� �����ϴ� �Ŵ���
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI speakerText;         // ȭ�� �̸� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI dialogueText;        // ��� �ؽ�Ʈ
    [SerializeField] private GameObject choiceButtonPrefab;       // ������ ��ư ������
    [SerializeField] private Transform choiceParent;              // ������ ��ư ���� ��ġ
    private TagHandler tagHandler;

    private DialogueEvent currentEvent;                           // ���� ���� ���� ��ȭ �̺�Ʈ
    private Dictionary<int, Dialogue> dialogueMap;                // ID �� Dialogue ����
    private int currentId;                                        // ���� ��� ID

    private bool isWaiting = false;


    private void Awake()
    {
        Instance = this;

        tagHandler = new TagHandler(animator : null);
    }

    /// <summary>
    /// ��ȭ �̺�Ʈ ����
    /// </summary>
    public void StartDialogue(DialogueEvent dialogueEvent)
    {
        currentEvent = dialogueEvent;
        dialogueMap = new Dictionary<int, Dialogue>();

        foreach (var line in currentEvent.lines)
            dialogueMap[line.id] = line;

        ProceedNext(0); // ���� 0�� ID���� ����
    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    public void ProceedNext(int id)
    {
        if (!dialogueMap.TryGetValue(id, out var line)) return;

        currentId = id;
        speakerText.text = line.speaker;
        dialogueText.text = line.text;

        // �޽��� �ý��ۿ� ��� (�÷��̾� ���ο� ���� ����)
        SMSManager.Instance.SaveMessage(line.text, line.speaker.ToLower() == "player");

        //�±� ���� ó�� �� ���� �� ���� �帧���� �ѱ�
        tagHandler.ProcessTags(line.tag, () =>
        {
            // ������ ��� �Ǵ� ���� �ٷ� �̵�
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
    /// �������� �����ϰ� ��ư�� �̺�Ʈ ���ε�
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
    /// ��ȭ ���� �� ó��
    /// </summary>
    private void EndDialogue()
    {
        Debug.Log("��ȭ �����");
        // ���� UI ��Ȱ��ȭ �� ó�� ����
    }

    public void SetWait(bool wait)
    {
        isWaiting = wait;
        // UI �󿡼� "����" ��ư�� Ȱ��ȭ�ϰų� ��� ���� ó�� ����
    }
}
