using UnityEngine;

/// <summary>
/// �÷��̾ ��ȣ�ۿ��� �� DialogueEvent�� �����ϴ� Ʈ����
/// </summary>
public class InteractionEvent : MonoBehaviour
{
    [SerializeField] private DialogueEvent dialogueEvent;  // ����� ��ȭ �̺�Ʈ

    /// <summary>
    /// ��ȣ�ۿ� �� ����Ǵ� ��ȭ ���� �Լ�
    /// </summary>
    public void TriggerEvent()
    {
        DialogueManager.Instance.StartDialogue(dialogueEvent);
    }
}
