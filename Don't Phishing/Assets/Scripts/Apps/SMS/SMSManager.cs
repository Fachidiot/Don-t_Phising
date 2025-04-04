using System.Collections.Generic;
using System.IO;
using System;
using TMPro;
using UnityEngine;
using System.Reflection;

[Serializable]
public class Message
{
    public string name;
    public string message;
    public string date;
}
public class MessageDB
{
    public List<Message> messages;
}

public class SMSManager : BaseAppManager
{
    private static SMSManager m_Instance;
    public static SMSManager Instance { get { return m_Instance; } }

    [Header("View & Bar")]
    [SerializeField]
    private GameObject m_MainBar;
    [SerializeField]
    private GameObject m_MessageBar;
    [SerializeField]
    private GameObject m_PopupView;
    [SerializeField]
    private GameObject m_HorizontalSnapScrollView;
    [SerializeField]
    private Profile m_SMSProfile;
    [Space]
    [Header("Text")]
    [SerializeField]
    private TMP_Text m_TMPSMS;
    [SerializeField]
    private LText m_SMSText;
    [Header("Messages")]
    [SerializeField]
    private GameObject m_TopMsgParent;
    [SerializeField]
    private GameObject m_TopMsgPrefab;
    [SerializeField]
    private GameObject m_MsgParent;
    [SerializeField]
    private GameObject m_MsgPrefab;
    [SerializeField]
    private GameObject m_MyMsgPrefab;

    // TODO : 추후 게임 전체(휴대폰 제외) Localization을 위해 Messages-en, Messages-kr, Messages-jp이런식으로 구현하면 좋을듯 싶음.
    private string m_FileName = "Messages.json";
    private string m_Path = Application.dataPath + "/Json/Messages/";

    private MessageDB m_MessageDB;
    private List<GameObject> m_TopMsgList;
    private List<GameObject> m_MessageList;

    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        m_Instance = this;
    }

    public void Start()
    {
        SetText();
        ResetApp();
        Init();
    }

    private void Init()
    {
        m_MessageDB = new MessageDB();
        m_TopMsgList = new List<GameObject>();
        m_MessageList = new List<GameObject>();
        LoadMessages();
    }

    #region BASE_APP
    public override void SetText()
    {
        m_TMPSMS.text = m_SMSText.GetText(OSManager.Instance.GetLanguage());
    }

    public override void ResetApp()
    {
        m_MainBar.SetActive(true);
        m_MessageBar.SetActive(false);
        m_PopupView.SetActive(false);
    }
    #endregion

    #region JSON
    private void LoadMessages()
    {
        if (File.Exists(m_Path + m_FileName))
        {
            string data = File.ReadAllText(m_Path + m_FileName);
            m_MessageDB = JsonUtility.FromJson<MessageDB>(data);

            foreach (var message in m_MessageDB.messages)
            {
                InstantiateTopMessage(message);
            }
        }
        else
        {
            Debug.LogWarning("There is no Messages");
        }
    }

    private void SaveMessages()
    {
        string data = JsonUtility.ToJson(m_MessageDB, true /* prettyPrint */);
        File.WriteAllText(m_Path + m_FileName, data);
        Debug.Log(data);
    }
    #endregion

    #region UTILS
    private void InstantiateTopMessage(Message message)
    {
        for (int i = 0; i < m_TopMsgList.Count; i++)
        {
            var layout = m_TopMsgList[i].GetComponent<SMS_Layout>();
            if (layout.GetMessage().name == message.name)
            {
                Debug.Log("중복");
                layout.SetUp(message);
                return;
            }
        }

        GameObject go = Instantiate(m_TopMsgPrefab, m_TopMsgParent.transform);
        go.GetComponent<SMS_Layout>().SetUp(message);
        m_TopMsgList.Add(go);
    }

    private void InstantiateMessage(Message message)
    {
        GameObject go = Instantiate(m_MsgPrefab, m_MsgParent.transform);
        go.GetComponent<Message_Layout>().SetUp(message);
        m_MessageList.Add(go);
    }
    #endregion

    public void LoadMessage(List<Message> list)
    {
        DeletePrev();

        m_MainBar.SetActive(false);
        m_MessageBar.SetActive(true);
        m_HorizontalSnapScrollView.GetComponent<ScrollSnap>().SetContentPosition(1);

        for (int i = 0;i < list.Count; i++)
        {
            InstantiateMessage(list[i]);
        }

        m_SMSProfile.SetProfile(list[0].name);
    }

    private void DeletePrev()
    {
        if (m_MessageList.Count <= 0)
            return;
        foreach (var item in m_MessageList)
        {
            Destroy(item.gameObject);
        }
        m_MessageList.Clear();
    }
}
