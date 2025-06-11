using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

/// <summary>
/// �ΰ��� ������ ��µǴ� ��� �帧�� �����ϴ� ��Ʈ�ѷ�
/// �÷��̾� ���� �����̼� or �ƾ� ��� � ���
/// </summary>
public class IngameDialogueController : MonoBehaviour
{
    [Header("UI �Ŵ��� ����")]
    [SerializeField] private IngameDialogueUIManager ui;

    [Header("��ȭ �̺�Ʈ ������")]
    [SerializeField] private DialogueEvent eventData;

    private Dictionary<int, Dialogue> map;   // ID -> Dialogue ��
    private int currentId;                   // ���� ��ȭ ID
    private bool readyForNext = false;       // ���� ���� �Ѿ �غ� �Ϸ� ����

    private void Update()
    {
        if (!ui.IsTyping())
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                if (readyForNext)
                {
                    Debug.Log("�Է� ���� ��� ����ؾ���");
                    ProceedNext();
                }
                else
                {
                   // ui.SkipTyping(); // �Է� �� Ÿ���� ��� ���
                }
            }
        }
    }

    /// <summary>
    /// ��ȭ�� �����ϴ� �Լ� - �ܺο��� ȣ���
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
    /// �ش� ID�� ��縦 ����ϰ�, ������ �Ǵ� �������� �������� ����
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

        // �ý��� �޽���
        if (type == "system")
        {
            ui.ShowSystemMessage(d.text, onComplete);
            return;
        }

        // ĳ���� �̹��� ����� (����Ŀ �̸� ����)
        //ui.UpdateCharacterVisual(speaker, d.tag); // tag ���� emotion ó�� ����

        // �Ϲ� �޽��� ���
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
