using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Message_Layout : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_TMPMessage;
    [SerializeField]
    private Image m_Image;
    [SerializeField]
    private ContentSizeFitter m_ContentSizeFitter;

    public void SetUp(Message message)
    {
        if (m_TMPMessage)
        {
            m_TMPMessage.text = message.message;
            if (message.message.Length < 12 && m_ContentSizeFitter != null)
                m_ContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        else
        {
            if (m_Image)
                m_Image.sprite = Resources.Load<Sprite>(message.message);
        }
    }
}
