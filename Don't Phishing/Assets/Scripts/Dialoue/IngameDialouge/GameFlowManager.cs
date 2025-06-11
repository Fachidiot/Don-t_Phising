using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// ��ü ���丮 �帧 ���¸� �����ϰ�, ������ �� ��Ʈ�ѷ��� �����ϴ� FSM ����
/// </summary>
public class GameFlowManager : MonoBehaviour
{
    public enum GameState
    {
        Intro,
        Story,
        Message,
        PostMessage
    }

    [SerializeField] private GameState startState = GameState.Intro;
    [SerializeField] private IngameDialogueController storyController;
    [SerializeField] private DialogueController messageController;
    [SerializeField] private DialogueEvent[] dialogueEvents; // ���� �̺�Ʈ ����

    private GameState currentState;
    public static GameFlowManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (storyController == null || messageController == null)
        {
            Debug.LogError("[GameFlowManager] ��Ʈ�ѷ��� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }
        TransitionToState(startState);
    }

    public void TransitionToState(GameState newState)
    {
        if (currentState == newState) return; // ���� ���� ����

        currentState = newState;
        Debug.Log($"[GameFlowManager] ���� ��ȯ��: {currentState}");

        switch (currentState)
        {
            case GameState.Intro:
                HandleIntro();
                break;

            case GameState.Story:
                StartDialogue(storyController, GetDialogueEvent("Capstone - ch01_event"));
                break;

            case GameState.Message:
                StartDialogue(messageController, GetDialogueEvent("Message_Event"));
                break;

            case GameState.PostMessage:
                StartDialogue(storyController, GetDialogueEvent("Capstone - ch01_Post"));
                break;

            default:
                Debug.LogWarning($"[GameFlowManager] ó������ ���� ����: {currentState}");
                break;
        }
    }

    private void HandleIntro()
    {
        Debug.Log("[GameFlowManager] ��Ʈ�� ���� ����");
        // ��Ʈ�� ���� ���� (��: �ִϸ��̼�, ���)
        StartCoroutine(IntroCoroutine());
    }

    private IEnumerator IntroCoroutine()
    {
        yield return new WaitForSeconds(2f); // ��: 2�� ���
        TransitionToState(GameState.Story);
    }

    private void StartDialogue(MonoBehaviour controller, DialogueEvent dialogueEvent)
    {
        if (controller is IngameDialogueController ingameController && dialogueEvent != null)
        {
            ingameController.StartDialogue(dialogueEvent);
        }
        else if (controller is DialogueController dialogueController && dialogueEvent != null)
        {
            dialogueController.StartDialogue(dialogueEvent);
        }
        else
        {
            Debug.LogError("[GameFlowManager] ��Ʈ�ѷ� �Ǵ� DialogueEvent�� null�Դϴ�.");
        }
    }

    private DialogueEvent GetDialogueEvent(string eventName)
    {
        foreach (var evt in dialogueEvents)
        {
            if (evt.name == eventName)
                return evt;
        }
        Debug.LogError($"[GameFlowManager] DialogueEvent '{eventName}'�� ã�� �� �����ϴ�.");
        return null;
    }

    public void OnAppMessageTag()
    {
        TransitionToState(GameState.Message);
    }

    public void OnMessageDialogueEnd()
    {
        TransitionToState(GameState.PostMessage);
    }
}