using TMPro;
using UnityEngine;

public class Message_Layout : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_TMPMessage;

    public void SetUp(Message message)
    {
        m_TMPMessage.text = message.message;
    }
}
