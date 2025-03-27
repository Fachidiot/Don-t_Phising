using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

[Serializable]
public class LText
{
    public Language m_Language;
    public string[] m_Text;

    public string GetText(Language language)
    {
        return m_Text[(int)language];
    }
}

public class AppManager : Observer
{
    [SerializeField]
    private Image m_Image;
    [SerializeField]
    private Sprite m_Source;
    [SerializeField]
    private Button m_Button;
    [Header("Title")]
    [SerializeField]
    private bool m_Title;
    [SerializeField]
    private LText m_Text;
    [SerializeField]
    private TMP_Text m_TitleText;
    [Header("Stack")]
    [SerializeField]
    private GameObject m_Stack;
    [SerializeField]
    private TMP_Text m_StackText;

    private int m_Count = 0;

    private void Start()
    {
        OSManager.Instance.Attach(this);

        m_Button.onClick.AddListener(RunApp);

        m_Image.sprite = m_Source;
        UpdateText();
        UpdateStack();
    }

    public void OnStack(int count)
    {
        m_Stack.SetActive(true);
        m_Count = count;
        m_StackText.text = count.ToString();
    }

    private void UpdateText()
    {
        if (m_Title)
            m_TitleText.text = m_Text.GetText(OSManager.Instance.GetLanguage());
        else
            m_TitleText.text = "";
    }

    private void UpdateStack()
    {
        if (m_Count != 0)
            OnStack(m_Count);
        else
            EraseStack();
    }

    private void RunApp()
    {
        OSManager.Instance.OS.gameObject.SetActive(false);
        OSManager.Instance.App.gameObject.SetActive(true);
    }

    private void CloseApp()
    {
        OSManager.Instance.OS.gameObject.SetActive(true);
        OSManager.Instance.App.gameObject.SetActive(false);
    }

    public void EraseStack()
    {
        m_Stack.SetActive(false);
        m_StackText.text = 1.ToString();
    }

    public override void Notify(Subject subject)
    {
        UpdateText();
    }
}
