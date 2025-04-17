using System.Collections.Generic;
using System.IO;
using System;
using TMPro;
using UnityEngine;
using static Message;

[Serializable]
public class Message
{
    public string name;
    public string message;
    public MsgType type;
    public string date;

    public enum MsgType
    {
        None,
        Mine
    }

    public Message(string name, string message, MsgType type, string date)
    {
        this.name = name;
        this.message = message;
        this.type = type;
        this.date = date;
    }
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
    private Profile_Layout m_SMSProfile;
    [Space]
    [Header("Text")]
    [SerializeField]
    private TMP_Text m_TMPSMS;
    [SerializeField]
    private LText m_SMSText;
    [SerializeField]
    private TMP_Text m_TMPEdit;
    [SerializeField]
    private LText m_EditText;
    [SerializeField]
    private SearchBar[] m_SearchBars;
    [Header("Messages")]
    [SerializeField]
    private bool m_AutoSave = true;
    [SerializeField]
    private GameObject m_MessagePreviewParent;
    [SerializeField]
    private GameObject m_MessagePreviewPrefab;
    [SerializeField]
    private GameObject m_MessageParent;
    [SerializeField]
    private GameObject m_NPCMessagePrefab;
    [SerializeField]
    private GameObject m_IMGMessagePrefab;
    [SerializeField]
    private GameObject m_PlayerMessagePrefab;

    private Message_Layout _lastNPCLayout;

    // TODO : 추후 게임 전체(휴대폰 제외) Localization을 위해 Messages-en, Messages-kr, Messages-jp이런식으로 구현하면 좋을듯 싶음.
    private string m_FileName = "Messages.json";
    private string m_Path = Application.dataPath + "/Json/Messages/";

    private MessageDB m_MessageDB;
    private List<GameObject> m_TopMsgList;
    private List<GameObject> m_MessageList;
    private string m_CurrentName;

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

    public void SaveMessage(string message, bool isMine)
    {
        var type = isMine ? MsgType.Mine : MsgType.None;
        Message _message = new Message(m_CurrentName, message, type, DateTime.Now.ToString("yyyy-MM-dd-HH:mm"));
        
        GameObject go = InstantiateMessage(_message, isMine);

        m_MessageDB.messages.Add(_message);
        if (m_AutoSave)
            SaveMessages();

        // NPC일 경우 마지막 메시지 캐싱
        if (!isMine && go != null)
            _lastNPCLayout = go.GetComponent<Message_Layout>();
    }

    public void LoadMessage(List<Message> list)
    {
        DeletePrev();

        // View 설정
        m_MainBar.SetActive(false);
        m_MessageBar.SetActive(true);
        m_HorizontalSnapScrollView.GetComponent<ScrollSnap>().SetContentPosition(1);

        // 메세지 생성
        for (int i = 0; i < list.Count; i++)
        {
            InstantiateMessage(list[i], list[i].name == "Mine");
        }

        m_CurrentName = list[0].name;
        m_SMSProfile.SetProfile(m_CurrentName);
    }

    #region BASE_APP
    public override void SetText()
    {
        m_TMPSMS.text = m_SMSText.GetText(OSManager.Instance.GetLanguage());
        m_TMPEdit.text = m_EditText.GetText(OSManager.Instance.GetLanguage());
        foreach (SearchBar searchBar in m_SearchBars)
        {
            searchBar.SetText();
        }
    }

    public override void ResetApp()
    {
        m_MainBar.SetActive(true);
        m_MessageBar.SetActive(false);
        m_PopupView.SetActive(false);
    }
    #endregion

    #region JSON
    public void LoadMessages()
    {
        if (m_TopMsgList.Count > 0)
        {
            foreach (var topmsg in m_TopMsgList)
            {
                Destroy(topmsg);
            }
            m_TopMsgList.Clear();
        }

        if (File.Exists(m_Path + m_FileName))
        {
            string data = File.ReadAllText(m_Path + m_FileName);
            m_MessageDB = JsonUtility.FromJson<MessageDB>(data);

            foreach (var message in m_MessageDB.messages)
            {
                InstantiatePreview(message);
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
    private void InstantiatePreview(Message message)
    {
        for (int i = 0; i < m_TopMsgList.Count; i++)
        {
            var layout = m_TopMsgList[i].GetComponent<SMS_Layout>();
            if (layout.GetMessage().name == message.name)
            {
                layout.SetUp(message);
                return;
            }
        }

        GameObject go = Instantiate(m_MessagePreviewPrefab, m_MessagePreviewParent.transform);
        go.GetComponent<SMS_Layout>().SetUp(message);
        m_TopMsgList.Add(go);
    }

    private GameObject InstantiateMessage(Message message, bool isMine)
    {
        GameObject go;
        if (isMine)
            go = Instantiate(m_PlayerMessagePrefab, m_MessageParent.transform);
        else
        {
            if (message.message.Contains('/'))
                go = Instantiate(m_IMGMessagePrefab, m_MessageParent.transform);
            else
                go = Instantiate(m_NPCMessagePrefab, m_MessageParent.transform);
        }

        if (go != null)
        {
            go.GetComponent<Message_Layout>().SetUp(message);
            m_MessageList.Add(go);
        }

        return go;
    }
    #endregion

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

    // SMSManager.cs 안에 추가
    public void UpdateLastNPCMessage(string message)
    {
        if (m_MessageList.Count == 0)
            return;

        GameObject lastMessage = m_MessageList[m_MessageList.Count - 1];
        Message_Layout layout = lastMessage.GetComponent<Message_Layout>();
        layout.UpdateText(message);
    }
}
