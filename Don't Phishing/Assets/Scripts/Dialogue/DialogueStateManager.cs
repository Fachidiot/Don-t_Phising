using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 대화 태그 기반 상태를 관리하는 싱글톤 매니저
/// - WAIT 멈춤 처리
/// - 엔딩 감지
/// - FLAG로 분기 저장
/// - 태그 로그 및 확장 대응
/// </summary>
public class DialogueStateManager : DialogueManager
{
    public static DialogueStateManager Instance { get; private set; }

    [Header("현재 대화 상태")]
    public bool isWaiting = false;            // WAIT 태그 감지 시 true
    public string currentEnding = "";         // 감지된 엔딩 태그 (예: ENDING_GOOD)

    [Header("게임 내 상태 플래그")]
    private Dictionary<string, bool> flags = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Ink 태그 목록 처리
    /// </summary>
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
            else if (tag == "npc" || tag == "player" || tag == "alarm")
            {
                // 역할 태그는 처리만
                Debug.Log("[태그 감지] " + tag);
            }
            else
            {
                Debug.LogWarning(" 처리되지 않은 태그: " + tag);
            }
        }
    }

    /// <summary>
    /// FLAG_X 여부 확인
    /// </summary>
    public bool HasFlag(string flagTag) => flags.TryGetValue(flagTag, out var val) && val;

    /// <summary>
    /// WAIT 상태 해제
    /// </summary>
    public void ClearState() => isWaiting = false;

    /// <summary>
    /// 현재 감지된 엔딩 반환
    /// </summary>
    public string GetEnding() => currentEnding;
}
