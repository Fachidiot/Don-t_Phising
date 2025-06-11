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
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
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
        ShowLine(map[currentId]);
    }

    /// <summary>
    /// 해당 ID의 대사를 출력하고, 선택지 또는 다음으로 진행할지 결정
    /// </summary>
    private void ShowLine(Dialogue d)
    {
        readyForNext = false;

        string type = d.type?.ToLowerInvariant(); // system, etc.
        string speaker = d.speaker?.ToLowerInvariant();

        Action onComplete = () =>
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
        };

        // 시스템 메시지
        if (type == "system")
        {
            ui.ShowSystemMessage(d.text, onComplete);
            return;
        }

        // 캐릭터 이미지 연출용 (스피커 이름 기준)
        //ui.UpdateCharacterVisual(speaker, d.tag); // tag 통해 emotion 처리 가능

        // 일반 메시지 출력
        ui.ShowMessage(d.text, onComplete);
    }

    private void ProceedNext()
    {
        if (map.ContainsKey(currentId) && map[currentId].nextId != 0)
        {
            currentId = map[currentId].nextId;
            ShowLine(map[currentId]);
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
