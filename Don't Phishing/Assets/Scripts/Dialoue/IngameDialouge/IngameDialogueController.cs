using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        // 대사 진행 중이 아니고, 사용자가 입력한 경우 다음으로 진행
        if (readyForNext && !ui.IsTyping() &&
           (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            ProceedNext();
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

        // UI에 메시지 출력 (타자 애니메이션)
        ui.ShowMessage(d.text, () =>
        {
            // 선택지가 있는 경우
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
                // 다음 대사 ID가 있을 경우 → 다음 입력까지 대기
                readyForNext = true;
            }
            else
            {
                // 종료
                ui.HideChoices();
            }
        });
    }

    /// <summary>
    /// 다음 ID로 진행 (사용자 입력에 의해 호출)
    /// </summary>
    private void ProceedNext()
    {
        if (map.ContainsKey(currentId) && map[currentId].nextId != 0)
        {
            currentId = map[currentId].nextId;
            ShowLine(map[currentId]);
        }
    }

    /// <summary>
    /// 선택지 텍스트를 파싱하여 (텍스트, 다음 ID) 리스트로 반환
    /// ex: "네:2,아니오:3"
    /// </summary>
    private List<(string, int)> ParseChoices(string raw)
    {
        var list = new List<(string, int)>();
        foreach (var s in raw.Split(','))
        {
            var parts = s.Split(':');
            if (parts.Length == 2 && int.TryParse(parts[1], out int id))
                list.Add((parts[0], id));
        }
        return list;
    }
}