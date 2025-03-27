using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Text
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
    [Header("Title")]
    [SerializeField]
    private bool m_Title;
    [SerializeField]
    private Text m_Text;
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
