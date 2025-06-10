using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SMS 메시지 프리뷰 슬롯 구성 및 클릭 시 대화 실행
/// </summary>
public class SMS_Layout : MonoBehaviour
{
    [SerializeField] private Button m_Button;

    [Header("TMP_Text")]
    [SerializeField] private TMP_Text m_TMPName;
    [SerializeField] private TMP_Text m_TMPMessage;
    [SerializeField] private TMP_Text m_TMPDate;
    [SerializeField] private DialogueEvent m_DialogueEvent;

    private List<Message> m_Message;
    private int m_Index = -1;

    private void Start()
    {
        m_Message = new List<Message>();

        // 버튼 클릭 시 SMSManager를 통해 메시지 전체 출력
        m_Button.onClick.AddListener(() => SMSManager.Instance.LoadMessage(m_Message));

        // 버튼 클릭 시 대화 이벤트 실행 시도
        m_Button.onClick.AddListener(() =>
        {
            DialogueController controller = FindObjectOfType<DialogueController>();
            if (controller != null && m_DialogueEvent != null)
                controller.StartDialogue(m_DialogueEvent);
            else
                Debug.LogWarning("[SMS_Layout] DialogueController 또는 DialogueEvent가 비어 있습니다.");
        });
    }

    /// <summary>
    /// 메시지 데이터를 UI에 반영하고 내부 리스트에 저장
    /// </summary>
    public void SetUp(Message message)
    {
        // 메시지 누적 저장
        m_Message.Add(message);
        m_Index++;

        // 텍스트 UI 세팅
        m_TMPName.text = m_Message[m_Index].name;

        // 이미지 메시지는 문자열 대신 "Image"로 대체 출력
        m_TMPMessage.text = message.message.Contains("/") ? "Image" : m_Message[m_Index].message;

        m_TMPDate.text = m_Message[m_Index].date;
    }

    /// <summary>
    /// 마지막에 추가된 메시지를 반환
    /// </summary>
    public Message GetMessage()
    {
        return m_Message[m_Index];
    }
}
