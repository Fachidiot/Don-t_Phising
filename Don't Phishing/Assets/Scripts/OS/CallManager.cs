using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CallManager : Observer
{
    [Header("Favorites")]
    [SerializeField]
    private TMP_Text m_Favorite;
    [SerializeField]
    private TMP_Text m_FavoriteTab;
    [SerializeField]
    private LText m_FavoriteText;
    [Header("Recents")]
    [SerializeField]
    private TMP_Text m_Recents;
    [SerializeField]
    private TMP_Text m_RecentsTab;
    [SerializeField]
    private LText m_RecentsText;
    [Header("Contacts")]
    [SerializeField]
    private TMP_Text m_Contacts;
    [SerializeField]
    private TMP_Text m_ContactsTab;
    [SerializeField]
    private LText m_ContactsText;
    [Header("Keypad")]
    [SerializeField]
    private TMP_Text m_KeypadTab;
    [SerializeField]
    private LText m_KeypadText;
    [SerializeField]
    private TMP_Text m_Number;
    [Header("Voicemail")]
    [SerializeField]
    private TMP_Text m_Voicemail;
    [SerializeField]
    private TMP_Text m_VoicemailTab;
    [SerializeField]
    private LText m_VoicemailText;

    private string m_SNumber = "";
    private Dictionary<string, string> m_DContacts;

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
                m_SNumber = m_SNumber.Substring(0, m_SNumber.Length - 1);
                break;
            default:
                m_SNumber += text.text;
                break;
        }
        m_Number.text = m_SNumber;
    }

    public void CallButton()
    {

    }
    #endregion

    private void UpdateText()
    {
        Language currentLanguage = OSManager.Instance.GetLanguage();
        m_Favorite.text = m_FavoriteText.GetText(currentLanguage);
        m_FavoriteTab.text = m_Favorite.text;
        m_Recents.text = m_RecentsText.GetText(currentLanguage);
        m_RecentsTab.text = m_Recents.text;
        m_Contacts.text = m_ContactsText.GetText(currentLanguage);
        m_ContactsTab.text = m_Contacts.text;
        m_KeypadTab.text = m_KeypadText.GetText(currentLanguage);
        m_Voicemail.text = m_VoicemailText.GetText(currentLanguage);
    }

    public override void Notify(Subject subject)
    {
        UpdateText();
    }
}
