using TMPro;
using UnityEngine;

public class Contact_Layout : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_TMPText;

    private Contact m_Contact;

    public void SetUp(Contact contact)
    {
        m_Contact = contact;

        m_TMPText.text = m_Contact.name;
    }
}
