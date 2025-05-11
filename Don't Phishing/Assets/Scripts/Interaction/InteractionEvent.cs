using UnityEngine;

/// <summary>
/// 플레이어가 상호작용할 때 DialogueEvent를 실행하는 트리거
/// </summary>
public class InteractionEvent : MonoBehaviour
{
    [SerializeField] private DialogueEvent dialogueEvent;  // 연결된 대화 이벤트

    /// <summary>
    /// 상호작용 시 실행되는 대화 시작 함수
    /// </summary>
    public void TriggerEvent()
    {
        DialogueManager.Instance.StartDialogue(dialogueEvent);
    }
}
