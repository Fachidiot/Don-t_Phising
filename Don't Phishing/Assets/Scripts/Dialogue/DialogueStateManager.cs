using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStateManager : MonoBehaviour
{
    public static DialogueStateManager Instance { get; private set; }

    public bool isWaiting = false;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
            Instance = this;
    }

    public void ProcessTags(List<string> tags)
    {
        foreach (string tag in tags)
        {
            if (tag == "WAIT") isWaiting = true;

            if (tag == "ENDING_BAD") isWaiting = true;

            if (tag == "ENDING_GOOD") isWaiting = true;

            if (tag == "ENDING_NEUTRAL") isWaiting = true;
       }
    }

    public void TriggerBadEnding()
    {
        Debug.Log("Bad Ending triggered");
        // TODO: 씬 전환 / UI 표시 / 저장 처리 등
    }

    public void TriggerGoodEnding()
    {
        Debug.Log("Good Ending triggered");
    }

    public void TriggerNeutralEnding()
    {
        Debug.Log("Neutral Ending triggered");
    }

    public void ClearState()
    {
        isWaiting = false;
    }
}
