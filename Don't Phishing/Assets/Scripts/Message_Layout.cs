using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Message_Layout : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_TMPMessage;
    [SerializeField]
    private ContentSizeFitter m_ContentSizeFitter;

    public void SetUp(Message message)
    {
        m_TMPMessage.text = message.message;
        if (message.message.Length < 12)
            m_ContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
    }
}
