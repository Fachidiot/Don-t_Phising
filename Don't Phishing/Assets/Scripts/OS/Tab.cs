using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    [Header("Icon")]
    [SerializeField]
    private Image m_Image;
    [SerializeField]
    private Sprite m_Sprite;
    [Header("View")]
    [SerializeField]
    private Button m_Button;
    [SerializeField]
    private GameObject m_View;
    [Header("Text")]
    [SerializeField]
    private TMP_Text m_Title;
    [Header("Options")]
    [SerializeField]
    private bool m_Popup;
    [SerializeField]
    private int m_Index;

    private TabManager m_TabManager;

    void Start()
    {
        m_TabManager = gameObject.transform.parent.GetComponent<TabManager>();
        if (m_Popup)
            m_Button.onClick.AddListener(() => PopupView());
        else
            m_Button.onClick.AddListener(EnableView);

        m_Image.sprite = m_Sprite;
    }

    private void EnableView()
    {
        m_TabManager.ResetTab();
        m_View.gameObject.SetActive(true);
    }

    private void PopupView()
    {
        m_View.gameObject.SetActive(true);
        m_View.GetComponent<ScrollSnap>().SetContentPosition(m_Index);
    }

    public void SetText(string text)
    {
        m_Title.text = text;
    }
}
