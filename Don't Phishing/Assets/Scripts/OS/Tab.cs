using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tab : Observer
{
    [Header("Icon")]
    [SerializeField]
    private Image m_Image;
    [SerializeField]
    private Sprite m_Sprite;
    [Header("Title")]
    [SerializeField]
    private TMP_Text m_Title;
    [SerializeField]
    private LText m_Text;
    [Header("View")]
    [SerializeField]
    private Button m_Button;
    [SerializeField]
    private GameObject m_View;

    void Start()
    {
        OSManager.Instance.Attach(this);

        m_Button.onClick.AddListener(EnableView);

        m_Image.sprite = m_Sprite;
        UpdateText();
    }

    private void EnableView()
    {
        CallManager.Instance.ResetView();
        m_View.gameObject.SetActive(true);
    }

    private void UpdateText()
    {
        m_Title.text = m_Text.GetText(OSManager.Instance.GetLanguage());
    }
    public override void Notify(Subject subject)
    {
        UpdateText();
    }
}
