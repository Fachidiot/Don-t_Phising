using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Task_Layout : MonoBehaviour
{
    [SerializeField]
    private Image m_Icon;
    [SerializeField]
    private Image m_Background;
    [SerializeField]
    private TMP_Text m_TMPTitle;
    [SerializeField]
    private ScrollSnap m_ScrollSnap;

    private TaskManager m_TaskManager;


    private void Start()
    {
        m_Background.gameObject.GetComponent<Button>().onClick.AddListener(RunApp);
    }

    private void RunApp()
    {
        AppManager.Instance.RunApp(m_TMPTitle.text);
        m_TaskManager.RunningApp();
    }

    public void SetTaskjLayout(string name, TaskManager manager, ScrollRect scrollRect)
    {
        m_TaskManager = manager;
        LoadTextures(name);
        m_TMPTitle.text = name;
        GetComponent<NestedScrollRect>().SetScrollRect(scrollRect);
    }

    void Update()
    {
        if (m_ScrollSnap.End)
        {// Task Á¦°Å
            AppManager.Instance.RefreshApp(m_TMPTitle.text);
            m_TaskManager.Remove(m_TMPTitle.text);
            Destroy(gameObject);
        }
    }

    private void LoadTextures(string name)
    {
        m_Background.sprite = Resources.Load<Sprite>("Background/" + name);
        m_Icon.sprite = Resources.Load<Sprite>("Sprites/Icons/" + name);
    }
}
