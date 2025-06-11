using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

/// <summary>
/// 인게임 내에서 출력되는 대사 흐름을 제어하는 컨트롤러
/// 플레이어 시점 내레이션 or 컷씬 대사 등에 사용
/// </summary>
public class IngameDialogueController : MonoBehaviour
{
    [Header("UI 매니저 참조")]
    [SerializeField] private IngameDialogueUIManager ui;

    [Header("대화 이벤트 데이터")]
    [SerializeField] private DialogueEvent eventData;

    private Dictionary<int, Dialogue> map;   // ID -> Dialogue 맵
    private int currentId;                   // 현재 대화 ID
    private bool readyForNext = false;       // 다음 대사로 넘어갈 준비 완료 여부

    private void Update()
    {
        if (!ui.IsTyping())
        {
            if (Input.GetKeyDown(KeyCode.E) /*|| Input.GetMouseButtonDown(0)*/)
            {
                if (readyForNext)
                {
                    Debug.Log("입력 성공 대사 출력해야함");
                    ProceedNext();
                }
                else
                {
                    // ui.SkipTyping(); // 입력 시 타이핑 모두 출력
                }
            }
        }
    }

    /// <summary>
    /// 대화를 시작하는 함수 - 외부에서 호출됨
    /// </summary>
    public void StartDialogue(DialogueEvent e)
    {
        eventData = e;
        map = new Dictionary<int, Dialogue>();
        foreach (var d in e.lines)
            map[d.id] = d;

        currentId = e.lines[0].id;
        Debug.Log($"[StartDialogue] 첫 대사 ID: {currentId}");
        ShowLine(map[currentId]);
    }

    /// <summary>
    /// 해당 ID의 대사를 출력하고, 선택지 또는 다음으로 진행할지 결정
    /// </summary>
    private void ShowLine(Dialogue d)
    {
        Debug.Log($"[ShowLine] ID: {d.id}, speaker: {d.speaker}, text: {d.text}, tag: {d.tag}");
        readyForNext = false;

        string type = d.type?.ToLowerInvariant();      // system, etc.
        string speaker = d.speaker?.ToLowerInvariant();
        string tag = d.tag?.ToLowerInvariant();         // 태그 소문자 처리

        if (tag == "app:message_start")
        {
            Debug.Log("[Ingame] 메시지 시작 태그 감지됨 → 메시지 모드 진입");
            ui.HideDialogueBox();  // 대사창 숨기기
            GameObject.FindObjectOfType<GameFlowManager>()?.OnAppMessageTag();
            return;
        }

        if (tag == "app:message_end")
        {
            Debug.Log("[Ingame] 메시지 종료 태그 감지됨 → 인게임 복귀");
            GameObject.FindObjectOfType<GameFlowManager>()?.OnMessageDialogueEnd();
            return;
        }


        // 구현 예정 태그 처리 로그 출력
        if (tag == "dialogue_stop" || tag == "app:alarm" || tag == "app:cameraqr")
        {
            Debug.Log($"[IngameDialogueController] 처리 대기 태그 감지됨: {tag}");
            return;
        }

        // 시스템 메시지 출력
        if (type == "system")
        {
            ui.ShowSystemMessage(d.text, () =>
            {
                HandleNextStep(d);
            });
            return;
        }

        // 일반 메시지 출력
        ui.ShowMessage(d.text, () =>
        {
            HandleNextStep(d);
        });
    }

    /// <summary>
    /// 다음 선택지, ID 진행 분기 공통 처리
    /// </summary>
    private void HandleNextStep(Dialogue d)
    {
        if (!string.IsNullOrEmpty(d.choices))
        {
            ui.ShowChoices(ParseChoices(d.choices), id =>
            {
                ui.HideChoices();
                currentId = id;
                ShowLine(map[id]);
            });
        }
        else if (d.nextId != 0)
        {
            readyForNext = true;
        }
        else
        {
            ui.HideChoices();
        }
    }

    private void ProceedNext()
    {
        if (map.ContainsKey(currentId) && map[currentId].nextId != 0)
        {
            Debug.Log("proceedNext 이동 완료");
            currentId = map[currentId].nextId;
            ShowLine(map[currentId]);
        }
    }
    public void ProceedToNextLine()
    {
        if (map.ContainsKey(currentId) && map[currentId].nextId != 0)
        {
            Debug.Log("[IngameDialogueController] ProceedToNextLine() → 다음 대사 출력");
            currentId = map[currentId].nextId;
            ShowLine(map[currentId]);
        }
        else
        {
            Debug.LogWarning("[IngameDialogueController] 다음 대사가 없거나 유효하지 않음");
        }
    }



    private List<(string, int)> ParseChoices(string raw)
    {
        var list = new List<(string, int)>();
        foreach (var s in raw.Split(','))
        {
            var parts = s.Trim().Split(':');
            if (parts.Length == 2 && int.TryParse(parts[1], out int id))
            {
                string choiceText = parts[0].Trim().Trim('[', ']', '"');
                list.Add((choiceText, id));
            }
        }
        return list;
    }
}
