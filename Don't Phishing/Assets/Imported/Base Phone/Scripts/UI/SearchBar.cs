using TMPro;
using UnityEngine;

public class SearchBar : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField m_TMPText;
    [SerializeField]
    private TMP_Text m_PlaceholderText;
    [SerializeField]
    private LText m_PlaceholderLanguage;

    public void SetText()
    {
        m_PlaceholderText.text = m_PlaceholderLanguage.GetText(OSManager.Instance.GetLanguage());
    }

    public string GetText()
    {
        return m_TMPText.text;
    }

    public void SetText(string text)
    {
        m_TMPText.text = text;
    }

    public void ResetText()
    {
        m_TMPText.text = "";
    }
}
