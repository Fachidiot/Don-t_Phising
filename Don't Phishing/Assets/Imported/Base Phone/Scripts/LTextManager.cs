using TMPro;

public class LTextManager : Observer
{
    private TMP_Text m_text;
    private void Awake()
    {
        m_text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        //SystemSetting.Instance.Attach(this);
    }

    private void TextBold()
    {
        m_text.fontStyle = SystemSetting.Instance.GetTextBold();
    }

    private void TextSize()
    {
        m_text.fontSize = SystemSetting.Instance.GetTextSize();
    }

    private void TextFont()
    {
        m_text.font = SystemSetting.Instance.GetTextFont();
    }

    public override void Notify(Subject subject)
    {
        TextBold();
        TextFont();
    }
}
