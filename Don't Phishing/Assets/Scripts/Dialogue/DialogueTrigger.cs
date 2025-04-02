using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink Script")]
    [SerializeField] private TextAsset inkJSON; // ����� Ink JSON ����

    // ��ư���� �� �Լ��� ȣ��
    public void TriggerDialogue()
    {
        if (DialogueManager.Instance == null)
        {
            Debug.LogWarning("DialogueManager instance not found.");
            return;
        }

        if (inkJSON == null)
        {
            Debug.LogWarning("No Ink JSON file assigned.");
            return;
        }

        DialogueManager.Instance.EnterDialogueMode(inkJSON);
    }
}
