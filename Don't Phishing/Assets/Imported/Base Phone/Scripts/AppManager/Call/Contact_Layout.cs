using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Contact_Layout : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_TMPText;
    [SerializeField]
    private Button m_Button;

    private Contact m_Contact;

    public void SetUp(Contact contact, ScrollSnap scrollSnap)
    {
        m_Contact = contact;

        m_Button.onClick.AddListener(() => scrollSnap.SetContentPosition(1));
        m_TMPText.text = m_Contact.name;
    }
}
