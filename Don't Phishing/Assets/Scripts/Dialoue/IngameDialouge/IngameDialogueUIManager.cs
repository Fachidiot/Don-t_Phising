using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// 인게임 대사 UI를 관리하는 클래스
/// 대사 출력, 타자 애니메이션, 선택지 버튼 표시/숨김 처리
/// </summary>
public class IngameDialogueUIManager : MonoBehaviour
{
    [Header("UI 컴포넌트")]
    [SerializeField] private TMP_Text messageText;           // 대사 출력 텍스트
    [SerializeField] private GameObject dialogueBox;         // 대사 박스 전체
    [SerializeField] private Button[] choiceButtons;         // 선택지 버튼들

    private bool isTyping = false; // 현재 타자 애니메이션 중 여부

    /// <summary>
    /// 메시지를 출력하고 타자 효과 적용
    /// </summary>
    public void ShowMessage(string text, Action onComplete)
    {
        StartCoroutine(TypeLine(text, onComplete));
    }

    /// <summary>
    /// 텍스트를 한 글자씩 출력하는 코루틴
    /// </summary>
    private IEnumerator TypeLine(string text, Action onComplete)
    {
        isTyping = true;
        messageText.text = "";

        foreach (char c in text)
        {
            messageText.text += c;
            yield return new WaitForSeconds(0.03f); // 타자 속도 조절
        }

        isTyping = false;
        onComplete?.Invoke();
    }

    /// <summary>
    /// 현재 메시지 출력 중인지 확인
    /// </summary>
    public bool IsTyping() => isTyping;

    /// <summary>
    /// 선택지를 화면에 표시하고, 버튼 클릭 시 콜백 실행
    /// </summary>
    public void ShowChoices(List<(string, int)> choices, Action<int> onChoice)
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Count)
            {
                var (text, id) = choices[i];
                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = text;
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => onChoice(id));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 모든 선택지 버튼을 숨깁니다
    /// </summary>
    public void HideChoices()
    {
        foreach (var b in choiceButtons)
        {
            b.gameObject.SetActive(false);
        }
    }
}