using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

[Serializable]
public class Contact
{
    public string name;
    public string number;
    public string memo;

    public Contact(string name, string number, string memo)
    {
        this.name = name;
        this.number = number;
        this.memo = memo;
    }
}
public class ContactDB
{
    public List<Contact> contacts = new List<Contact>();
}

[Serializable]
public class Recent
{
    public string name;
    public string Date;

    public Recent(string name, string date)
    {
        this.name = name;
        this.Date = date;
    }
}
public class RecentDB
{
    public List<Recent> recents = new List<Recent>();
}

public class CallManager : BaseAppManager
{
    [Header("Contacts")]
    [SerializeField]
    private GameObject m_ContactParent;
    [SerializeField]
    private GameObject m_ContactPrefab;
    [SerializeField]
    private ScrollSnap m_Horizontal_Snap;
    [Header("Keypads")]
    [SerializeField]
    private TMP_Text m_InputNumber;
    [SerializeField]
    private TabManager m_TabManager;
    [SerializeField]
    private NavigationBar m_NavigationBar;
    [SerializeField]
    private GameObject m_PopupView;
    [SerializeField]
    private SearchBar m_NumInput;
    [SerializeField]
    private SearchBar[] m_SearchBars;

    // TODO : 추후 게임 전체(휴대폰 제외) Localization을 위해 Contacts-en, Contacts-kr, Contacts-jp이런식으로 구현하면 좋을듯 싶음.
    private string m_FileName = "Contacts.json";
    private string m_Path = Application.dataPath + "/Json/Contacts/";

    private ContactDB m_ContactDB = new ContactDB();
    private Dictionary<Contact, DateTime> m_Recents;

    private void Start()
    {
        SetText();
        ResetApp();
        Init();
    }

    private void Init()
    {
        m_ContactDB = new ContactDB();
        m_Recents = new Dictionary<Contact, DateTime>();
        LoadContacts();
    }

    public override void SetText()
    {
        m_TabManager.SetText();
        m_NavigationBar.SetText();
        m_NumInput.SetText();
        foreach (SearchBar searchBar in m_SearchBars)
        {
            searchBar.SetText();
        }
    }

    public override void ResetApp()
    {
        m_PopupView.SetActive(false);
    }

    #region Keypad
    public void GetNumber()
    {
        m_NumInput.SetText(m_InputNumber.text);
    }

    public void SetNumber(TMP_Text text)
    {
        switch (text.text)
        {
            case "X":
                if (m_InputNumber.text.Length > 0)
                    m_InputNumber.text = m_InputNumber.text.Substring(0, m_InputNumber.text.Length - 1);
                break;
            default:
                m_InputNumber.text += text.text;
                break;
        }
    }

    public void AddContact()
    {
        string number = m_SearchBars[0].GetText();
        string name = m_SearchBars[1].GetText();
        string memo = m_SearchBars[2].GetText();
        Contact newCont = new Contact(name, number, memo);

        m_ContactDB.contacts.Add(newCont);

        m_NumInput.ResetText();
        foreach (SearchBar searchBar in m_SearchBars)
        {
            searchBar.ResetText();
        }

        InstantiateContact(newCont);
        SaveContacts();
    }

    public void RemoveContact()
    {

    }

    public void CallButton()
    {

    }

    private void InstantiateContact(Contact contact)
    {
        GameObject go = Instantiate(m_ContactPrefab, m_ContactParent.transform);
        go.GetComponent<Contact_Layout>().SetUp(contact, m_Horizontal_Snap);
    }
    #endregion

    #region JSON
    private void LoadContacts()
    {
        if (File.Exists(m_Path + m_FileName))
        {
            string data = File.ReadAllText(m_Path + m_FileName);
            m_ContactDB = JsonUtility.FromJson<ContactDB>(data);

            foreach (var contact in m_ContactDB.contacts)
            {
                InstantiateContact(contact);
            }
        }
        else
        {
            Debug.LogWarning("There is no Contacts");
        }
    }

    private void SaveContacts()
    {
        string data = JsonUtility.ToJson(m_ContactDB, true /* prettyPrint */);
        File.WriteAllText(m_Path + m_FileName, data);
        Debug.Log(data);
    }
    #endregion
}
