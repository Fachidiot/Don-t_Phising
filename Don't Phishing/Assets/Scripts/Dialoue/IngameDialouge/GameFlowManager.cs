using UnityEngine;
using System.Collections.Generic;

public class GameFlowManager : MonoBehaviour
{
    public enum GameState { Day1, Day2, Day3 }

    [Header("Controllers")]
    public IngameDialogueController storyController;
    public DialogueController messageController; // 안 쓰지만 참조만 남겨둠

    [Header("Events")]
    public List<DialogueEvent> dialogueEvents;

    private GameState currentState;

    public static GameFlowManager Instance { get; private set; }
    public GameState _CurrentState => currentState;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        Debug.Log("[GameFlowManager] 게임 시작됨 - Start() 호출");
        SetState(GameState.Day1);
    }

    public void SetState(GameState newState)
    {
        currentState = newState;
        Debug.Log($"[GameFlowManager] 상태 전환됨 → {newState}");

        switch (newState)
        {
            case GameState.Day1:
                StartStory("ch01");
                break;
            case GameState.Day2:
                StartStory("ch02");
                break;
            case GameState.Day3:
                StartStory("ch03");
                break;
        }
    }

    private void StartStory(string chapterKeyword)
    {
        var story = GetDialogueEvent(chapterKeyword);
        if (story != null && storyController != null)
        {
            Debug.Log($"[GameFlowManager] 스토리 시작: {chapterKeyword}");
            storyController.StartDialogue(story);
        }
        else
        {
            Debug.LogError($"[GameFlowManager] 스토리 데이터 또는 컨트롤러 없음: {chapterKeyword}");
        }
    }

    private DialogueEvent GetDialogueEvent(string partialName)
    {
        foreach (var evt in dialogueEvents)
        {
            Debug.Log("[GameFlowManager] 이벤트 검색 중: " + (evt != null ? evt.name : "null"));
            if (evt != null && evt.name.ToLower().Contains(partialName.ToLower()))
                return evt;
        }
        Debug.LogError("[GameFlowManager] 이벤트를 찾을 수 없음: " + partialName);
        return null;
    }

    public void OnAppMessageTag()
    {
        // 메시지 시작 태그 (app:message_start) → 실행 중단
        Debug.Log("[GameFlowManager] 메시지 시작 태그 감지됨 → 스토리 정지 (대사 흐름은 유지)");
        // 아무 동작 없음
    }

    public void OnMessageDialogueEnd()
    {
        Debug.Log("[GameFlowManager] 메시지 종료 → 스토리 복귀");

        // 기존 인게임 흐름 재시작
        var story = GetDialogueEvent("ch01"); // 혹은 복귀할 챕터
        if (story != null && storyController != null)
        {
            storyController.StartDialogue(story);
        }
        else
        {
            Debug.LogError("[GameFlowManager] 스토리 복귀 실패: 이벤트 또는 컨트롤러 없음");
        }
    }

}
