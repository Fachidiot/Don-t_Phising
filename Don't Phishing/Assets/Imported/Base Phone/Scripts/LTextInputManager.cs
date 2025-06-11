using TMPro;
using UnityEngine;

public class LTextInputManager : Observer
{
    private TMP_InputField m_inputField;
    [SerializeField] private TMP_Text m_plcaeholdertext;
    [SerializeField] private TMP_Text m_text;
    private void Awake()
    {
        m_inputField = GetComponent<TMP_InputField>();
        OSManager.Instance.Attach(this);
    }

    private void TextBold()
    {
        m_text.fontStyle = SystemSetting.Instance.GetTextBold();
        m_plcaeholdertext.fontStyle = SystemSetting.Instance.GetTextBold();
    }

    private void TextSize()
    {
        m_text.fontSize = SystemSetting.Instance.GetTextSize();
        m_plcaeholdertext.fontSize = SystemSetting.Instance.GetTextSize();
    }

    private void TextFont()
    {
        m_text.font = SystemSetting.Instance.GetTextFont();
        m_plcaeholdertext.font = SystemSetting.Instance.GetTextFont();
        m_inputField.fontAsset = SystemSetting.Instance.GetTextFont();
    }

    public override void Notify(Subject subject)
    {
        TextBold();
        TextSize();
        TextFont();
    }
}
