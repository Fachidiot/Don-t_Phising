using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 대사 출력, 선택지 표시 등 UI 처리 담당 매니저
/// </summary>
public class DialogueUIManager : MonoBehaviour
{
    [Header("Message Prefabs")]
    [SerializeField] private GameObject playerMessagePrefab;
    [SerializeField] private GameObject npcMessagePrefab;
    [SerializeField] private GameObject imageMessagePrefab;
    [SerializeField] private Transform messageParent;

    [Header("Choice Buttons")]
    [SerializeField] private Button[] choiceButtons;

    private GameObject lastMessageObj;

    /// <summary>
    /// 메시지 출력 (텍스트 or 이미지)
    /// </summary>
    public void ShowMessage(string message, bool isPlayer)
    {
        GameObject prefab;

        if (isPlayer)
        {
            prefab = playerMessagePrefab;
        }
        else
        {
            // 이미지 메시지인지 판단
            var sprite = Resources.Load<Sprite>(message);
            prefab = (sprite != null) ? imageMessagePrefab : npcMessagePrefab;
        }

        var go = Instantiate(prefab, messageParent);
        lastMessageObj = go;

        var layout = go.GetComponent<Message_Layout>();
        layout?.SetUp(message);
    }

    /// <summary>
    /// 마지막 출력된 메시지를 업데이트 (타자 효과용)
    /// </summary>
    public void UpdateLastMessage(string updatedText)
    {
        if (lastMessageObj == null) return;

        var layout = lastMessageObj.GetComponent<Message_Layout>();
        layout?.UpdateText(updatedText);
    }

    /// <summary>
    /// 선택지 버튼 표시
    /// </summary>
    public void ShowChoices(List<(string text, int nextId)> choices)
    {
        ClearChoices(); // 기존 버튼 초기화

        int max = Mathf.Min(choices.Count, choiceButtons.Length);

        for (int i = 0; i < max; i++)
        {
            var (text, nextId) = choices[i];
            var button = choiceButtons[i];
            var tmp = button.GetComponentInChildren<TMP_Text>();

            if (tmp != null)
                tmp.text = text;

            button.gameObject.SetActive(true);
            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(() =>
            {
                DialogueController controller = FindObjectOfType<DialogueController>();
                ShowMessage(text, true); // 선택한 메시지를 플레이어 말풍선으로 출력
                ClearChoices();
                controller.ProceedNext(nextId);
            });
        }
    }

    /// <summary>
    /// 선택지 버튼 숨김
    /// </summary>
    public void ClearChoices()
    {
        foreach (var button in choiceButtons)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
    }
}
