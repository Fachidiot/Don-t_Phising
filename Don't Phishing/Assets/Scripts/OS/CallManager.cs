using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

struct Contact
{
    string name;
    public string Name { get { return name; } }
    string number;
    public string Number { get { return number; } }
    string memo;
    public string Memo { get { return memo; } }

    public Contact(string name, string number, string memo)
    {
        this.name = name;
        this.number = number;
        this.memo = memo;
    }
}

public class CallManager : Observer
{
    [Header("Favorites")]
    [SerializeField]
    private TMP_Text m_TMPFavorite;
    [SerializeField]
    private TMP_Text m_TMPFavoriteTab;
    [SerializeField]
    private LText m_FavoriteText;
    [Header("Recents")]
    [SerializeField]
    private TMP_Text m_TMPRecents;
    [SerializeField]
    private TMP_Text m_TMPRecentsTab;
    [SerializeField]
    private LText m_RecentsText;
    [Header("Contacts")]
    [SerializeField]
    private TMP_Text m_TMPContacts;
    [SerializeField]
    private TMP_Text m_TMPContactsTab;
    [SerializeField]
    private LText m_ContactsText;
    [Header("Keypad")]
    [SerializeField]
    private TMP_Text m_TMPKeypadTab;
    [SerializeField]
    private LText m_KeypadText;
    [SerializeField]
    private TMP_Text m_TMPNumber;
    [Header("Voicemail")]
    [SerializeField]
    private TMP_Text m_TMPVoicemail;
    [SerializeField]
    private TMP_Text m_TMPVoicemailTab;
    [SerializeField]
    private LText m_VoicemailText;

    private string m_Name = "";
    private string m_Number = "";
    private string m_Memo = "";
    private List<Contact> m_Contacts;
    private Dictionary<Contact, DateTime> m_Recents;

    private void Start()
    {
        UpdateText();
        OSManager.Instance.Attach(this);
    }

    #region Keypad
    public void SetNumber(TMP_Text text)
    {
        switch (text.text)
        {
            case "X":
                if (m_Number.Length > 0)
                    m_Number = m_Number.Substring(0, m_Number.Length - 1);
                break;
            default:
                m_Number += text.text;
                break;
        }
        m_TMPNumber.text = m_Number;
    }

    public void AddContact()
    {
        Contact newCont = new Contact(m_Name, m_Number, m_Memo);
        m_Name = "";
        m_Memo = "";
        m_Contacts.Add(newCont);
    }

    public void CallButton()
    {

    }
    #endregion

    private void UpdateText()
    {
        Language currentLanguage = OSManager.Instance.GetLanguage();
        m_TMPFavorite.text = m_FavoriteText.GetText(currentLanguage);
        m_TMPFavoriteTab.text = m_TMPFavorite.text;
        m_TMPRecents.text = m_RecentsText.GetText(currentLanguage);
        m_TMPRecentsTab.text = m_TMPRecents.text;
        m_TMPContacts.text = m_ContactsText.GetText(currentLanguage);
        m_TMPContactsTab.text = m_TMPContacts.text;
        m_TMPKeypadTab.text = m_KeypadText.GetText(currentLanguage);
        m_TMPVoicemail.text = m_VoicemailText.GetText(currentLanguage);
    }

    public override void Notify(Subject subject)
    {
        UpdateText();
    }
}
