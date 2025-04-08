using TMPro;
using UnityEngine;

public class Setting_Layout : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_TMPText;
    [SerializeField]
    private LText m_Text;
    public void SetText()
    {
        m_TMPText.text = m_Text.GetText(OSManager.Instance.GetLanguage());
    }
}
