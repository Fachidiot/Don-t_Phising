using TMPro;
using UnityEngine;

public class View : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_Title;

    public void SetText(string text)
    {
        if (m_Title != null)
            m_Title.text = text;
    }
}
