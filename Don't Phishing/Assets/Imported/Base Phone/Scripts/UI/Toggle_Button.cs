using UnityEngine;
using UnityEngine.UI;

public class Toggle_Button : MonoBehaviour
{
    [SerializeField] private VirtualPhoneSettingType m_settingType;
    [SerializeField] private short m_settingTypeNum;
    [SerializeField] private Color m_OffColor;
    [SerializeField] private Color m_OnColor;
    [SerializeField] private GameObject m_OnImg;
    [SerializeField] private GameObject m_OffImg;

    private bool m_toggle = false;

    private Button m_Button;
    private void Start()
    {
        m_Button = GetComponent<Button>();
    }

    public void SetValue(bool value)
    {
        m_toggle = !value;
        Toggle();
    }

    public void Toggle()
    {
        m_toggle = !m_toggle;
        if (m_toggle)
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

        SettingManager.m_Instance.UpdateSetting(m_settingType, m_toggle, m_settingTypeNum);
    }
}
