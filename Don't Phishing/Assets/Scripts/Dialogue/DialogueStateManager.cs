// DialogueStateManager.cs
// 상태 제어를 책임지는 싱글톤 매니저로 태그 기반으로 WAIT, ENDING, FLAG 등을 관리합니다.
using System.Collections.Generic;
using UnityEngine;

public class DialogueStateManager : MonoBehaviour
{
    public static DialogueStateManager Instance { get; private set; }

    [Header("현재 대화 상태")]
    public bool isWaiting = false;          // WAIT 태그 감지 시 true
    public string currentEnding = "";       // 엔딩 태그 저장 (예: ENDING_BAD)

    [Header("게임 내 상태 플래그")]
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
                Debug.Log("[엔딩 감지] " + tag);
            }
            else if (tag == "WAIT")
            {
                isWaiting = true;
                Debug.Log("[WAIT 상태 진입]");
            }
            else if (tag.StartsWith("FLAG_"))
            {
                if (!flags.ContainsKey(tag))
                {
                    flags[tag] = true;
                    Debug.Log("[FLAG 설정] " + tag);
                }
            }
        }
    }

    public bool HasFlag(string flagTag) => flags.TryGetValue(flagTag, out var val) && val;

    public void ClearState() => isWaiting = false;

    public string GetEnding() => currentEnding;
}
