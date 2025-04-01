using TMPro;
using UnityEngine;

public class NavigationBar : MonoBehaviour
{
    [SerializeField]
    private TMP_Text[] m_TMPTexts;
    [SerializeField]
    private LText[] m_Texts;

    public void SetText()
    {
        for (int i = 0; i < m_TMPTexts.Length; i++)
        {
            m_TMPTexts[i].text = m_Texts[i].GetText(OSManager.Instance.GetLanguage());
        }
    }
}
