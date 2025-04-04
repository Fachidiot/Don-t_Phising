using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Recent_Layout : MonoBehaviour
{
    [SerializeField]
    private Sprite m_Source;
    [SerializeField]
    private Image m_Image;
    [SerializeField]
    private TMP_Text m_TMPName;
    [SerializeField]
    private TMP_Text m_TMPType;
    [SerializeField]
    private TMP_Text m_TMPTime;

    public void SetUp(Recent recent)
    {
        m_Image.sprite = m_Source;
        m_TMPName.text = recent.name;
        m_TMPType.text = "Mobile";
        m_TMPTime.text = recent.Date;
    }
}
