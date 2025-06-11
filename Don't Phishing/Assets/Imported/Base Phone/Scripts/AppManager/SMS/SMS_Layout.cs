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
    [SerializeField] private DialogueEvent m_DialogueEvent; // 유지하지만 사용 안 함

    [SerializeField] private List<Message> m_Message;
    private int m_Index = -1;

    private void Awake()
    {
        m_Message = new List<Message>();

        // 클릭 시 메시지 내용만 로드 (대화 실행은 GameFlowManager가 처리함)
        m_Button.onClick.AddListener(() =>
        {
            Debug.Log("[SMS_Layout] 메시지 클릭 → 내용만 로드");
            SMSManager.Instance.LoadMessage(m_Message);
        });

        // 이전 대화 실행 코드는 제거됨
    }

    public void SetUp(Message message)
    {
        m_Message.Add(message);
        m_Index++;

        m_TMPName.text = m_Message[m_Index].name;
        m_TMPMessage.text = message.message.Contains("/") ? "Image" : m_Message[m_Index].message;
        m_TMPDate.text = m_Message[m_Index].date;
    }

    public Message GetMessage()
    {
        return m_Message[m_Index];
    }
}


/*using System.Collections.Generic;
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

    [SerializeField] private List<Message> m_Message;
    private int m_Index = -1;

    private void Awake()
    {
        m_Message = new List<Message>();

        // 클릭 시 메시지 로드
        m_Button.onClick.AddListener(() => SMSManager.Instance.LoadMessage(m_Message));

        // 클릭 시 대화 실행
        m_Button.onClick.AddListener(() =>
        {
            DialogueController controller = FindObjectOfType<DialogueController>();
            if (controller != null && m_DialogueEvent != null)
                controller.StartDialogue(m_DialogueEvent);
            else
                Debug.LogWarning("[SMS_Layout] DialogueController 또는 DialogueEvent가 비어 있습니다.");
        });
    }

    public void SetUp(Message message)
    {
        m_Message.Add(message);
        m_Index++;

        m_TMPName.text = m_Message[m_Index].name;
        m_TMPMessage.text = message.message.Contains("/") ? "Image" : m_Message[m_Index].message;
        m_TMPDate.text = m_Message[m_Index].date;
    }

    public Message GetMessage()
    {
        return m_Message[m_Index];
    }
}
*/