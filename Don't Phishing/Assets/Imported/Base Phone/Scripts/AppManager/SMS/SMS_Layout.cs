using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SMS_Layout : MonoBehaviour
{
    [SerializeField]
    private Button m_Button;
    [Header("TMP_Text")]
    [SerializeField]
    private TMP_Text m_TMPName;
    [SerializeField]
    private TMP_Text m_TMPMessage;
    [SerializeField]
    private TMP_Text m_TMPDate;
    [SerializeField]
    private DialogueEvent m_DialogueEvent;
    
    private List<Message> m_Message; 
    private int m_Index = -1;

    private void Awake()
    {
        m_Message = new List<Message>();
        m_Button.onClick.AddListener(() => SMSManager.Instance.LoadMessage(m_Message));
        m_Button.onClick.AddListener(() => DialogueManager.Instance.StartDialogue(m_DialogueEvent));
    }


    public void SetUp(Message message)
    {
        m_Message.Add(message);
        m_Index++;

        m_TMPName.text = m_Message[m_Index].name;
        if (message.message.Contains('/'))
            m_TMPMessage.text = "Image";
        else
            m_TMPMessage.text = m_Message[m_Index].message;

        m_TMPDate.text = m_Message[m_Index].date;
    }

    public Message GetMessage()
    {
        return m_Message[m_Index];
    }
}
