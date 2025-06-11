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

            if (m_ContentSizeFitter != null)
            {
                if (message.message.Length < 15)
                    m_ContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                else
                    m_ContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            }
        }
        else if (m_Image)
        {
            m_Image.sprite = Resources.Load<Sprite>(message.message);
        }

        ForceLayoutRefresh();
    }

    public void SetUp(string text)
    {
        if (m_TMPMessage)
        {
            m_TMPMessage.text = text;

            if (m_ContentSizeFitter != null)
            {
                if (text.Length < 15)
                    m_ContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                else
                    m_ContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            }

            ForceLayoutRefresh();
        }
    }

    private void ForceLayoutRefresh()
    {
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    public void UpdateText(string updated)
    {
        if (m_TMPMessage)
        {
            m_TMPMessage.text = updated;
        }
    }
}
