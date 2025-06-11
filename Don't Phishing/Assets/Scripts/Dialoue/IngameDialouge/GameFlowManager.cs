using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// 전체 스토리 흐름 상태를 관리하고, 실행은 각 컨트롤러에 위임하는 FSM 구조
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
    [SerializeField] private DialogueEvent[] dialogueEvents; // 다중 이벤트 지원

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
            Debug.LogError("[GameFlowManager] 컨트롤러가 할당되지 않았습니다.");
            return;
        }
        TransitionToState(startState);
    }

    public void TransitionToState(GameState newState)
    {
        if (currentState == newState) return; // 동일 상태 무시

        currentState = newState;
        Debug.Log($"[GameFlowManager] 상태 전환됨: {currentState}");

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
                Debug.LogWarning($"[GameFlowManager] 처리되지 않은 상태: {currentState}");
                break;
        }
    }

    private void HandleIntro()
    {
        Debug.Log("[GameFlowManager] 인트로 상태 진입");
        // 인트로 연출 로직 (예: 애니메이션, 대기)
        StartCoroutine(IntroCoroutine());
    }

    private IEnumerator IntroCoroutine()
    {
        yield return new WaitForSeconds(2f); // 예: 2초 대기
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
            Debug.LogError("[GameFlowManager] 컨트롤러 또는 DialogueEvent가 null입니다.");
        }
    }

    private DialogueEvent GetDialogueEvent(string eventName)
    {
        foreach (var evt in dialogueEvents)
        {
            if (evt.name == eventName)
                return evt;
        }
        Debug.LogError($"[GameFlowManager] DialogueEvent '{eventName}'를 찾을 수 없습니다.");
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