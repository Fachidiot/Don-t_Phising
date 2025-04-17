// DialogueStateManager.cs
// ���� ��� å������ �̱��� �Ŵ����� �±� ������� WAIT, ENDING, FLAG ���� �����մϴ�.
using System.Collections.Generic;
using UnityEngine;

public class DialogueStateManager : MonoBehaviour
{
    public static DialogueStateManager Instance { get; private set; }

    [Header("���� ��ȭ ����")]
    public bool isWaiting = false;          // WAIT �±� ���� �� true
    public string currentEnding = "";       // ���� �±� ���� (��: ENDING_BAD)

    [Header("���� �� ���� �÷���")]
    private Dictionary<string, bool> flags = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

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
        }
    }

    public bool HasFlag(string flagTag) => flags.TryGetValue(flagTag, out var val) && val;

    public void ClearState() => isWaiting = false;

    public string GetEnding() => currentEnding;
}
