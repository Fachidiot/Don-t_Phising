using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ȭ �±� ��� ���¸� �����ϴ� �̱��� �Ŵ���
/// - WAIT ���� ó��
/// - ���� ����
/// - FLAG�� �б� ����
/// - �±� �α� �� Ȯ�� ����
/// </summary>
public class DialogueStateManager : DialogueManager
{
    public static DialogueStateManager Instance { get; private set; }

    [Header("���� ��ȭ ����")]
    public bool isWaiting = false;            // WAIT �±� ���� �� true
    public string currentEnding = "";         // ������ ���� �±� (��: ENDING_GOOD)

    [Header("���� �� ���� �÷���")]
    private Dictionary<string, bool> flags = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Ink �±� ��� ó��
    /// </summary>
    public void HandleTags(List<string> tags)
    {
        foreach (string tag in tags)
        {
            if (tag.StartsWith("ENDING_"))
            {
                currentEnding = tag;
                Debug.Log("[���� ����] " + tag);
            }
            else if (tag == "WAIT")
            {
                isWaiting = true;
                Debug.Log("[WAIT ���� ����]");
            }
            else if (tag.StartsWith("FLAG_"))
            {
                if (!flags.ContainsKey(tag))
                {
                    flags[tag] = true;
                    Debug.Log("[FLAG ����] " + tag);
                }
            }
            else if (tag == "npc" || tag == "player" || tag == "alarm")
            {
                // ���� �±״� ó����
                Debug.Log("[�±� ����] " + tag);
            }
            else
            {
                Debug.LogWarning(" ó������ ���� �±�: " + tag);
            }
        }
    }

    /// <summary>
    /// FLAG_X ���� Ȯ��
    /// </summary>
    public bool HasFlag(string flagTag) => flags.TryGetValue(flagTag, out var val) && val;

    /// <summary>
    /// WAIT ���� ����
    /// </summary>
    public void ClearState() => isWaiting = false;

    /// <summary>
    /// ���� ������ ���� ��ȯ
    /// </summary>
    public string GetEnding() => currentEnding;
}
