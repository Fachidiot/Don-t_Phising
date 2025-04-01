using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink Script")]
    [SerializeField] private TextAsset inkJSON; // 연결된 Ink JSON 파일

    // 버튼에서 이 함수를 호출
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
