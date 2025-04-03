using UnityEngine;
using UnityEngine.UI;

public class Toggle_Button : MonoBehaviour
{
    [SerializeField]
    private Button m_Button;
    [SerializeField]
    private Color m_OffColor;
    [SerializeField]
    private Color m_OnColor;
    [SerializeField]
    private GameObject m_OnImg;
    [SerializeField]
    private GameObject m_OffImg;

    private bool m_Toggle = false;

    private void Start()
    {
        m_OnImg.SetActive(false);
    }

    public void Toggle()
    {
        m_Toggle = !m_Toggle;
        if (m_Toggle)
        {
            GetComponent<Image>().color = m_OnColor;
            m_OnImg.SetActive(true);
            m_OffImg.SetActive(false);
        }
        else
        {
            GetComponent<Image>().color = m_OffColor;
            m_OnImg.SetActive(false);
            m_OffImg.SetActive(true);
        }
    }
}
