
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_OptionUI;
    [SerializeField]
    private GameObject m_PauseUI;
    private bool m_Paused = false;
    [SerializeField]
    private GameObject m_TutorialUI;
    private bool m_Init = false;
    [SerializeField]
    private GameObject m_Phone;
    private bool m_UsePhone = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (m_Phone == null)
            m_Phone = OSManager.Instance.transform.parent.gameObject;

        Inputs();
    }

    private void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!m_Init)
            {
                m_TutorialUI.SetActive(false);
                m_Init = true;
            }
            m_UsePhone = !m_UsePhone;
            m_Phone.SetActive(m_UsePhone);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_OptionUI.activeSelf)
                m_OptionUI.SetActive(false);
            else
            {
                m_Paused = !m_Paused;
                m_PauseUI.SetActive(m_Paused);
            }
        }
    }
}
