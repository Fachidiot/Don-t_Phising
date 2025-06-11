using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public IngameDialogueController controller;
    public DialogueEvent dialogue;

    public void StartDialogue()
    {
        controller.StartDialogue(dialogue);
    }
}
