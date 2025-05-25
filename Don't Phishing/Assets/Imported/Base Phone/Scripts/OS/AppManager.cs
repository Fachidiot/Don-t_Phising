using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AppManager : Observer
{
    private static AppManager m_Instance;
    public static AppManager Instance { get { return m_Instance; } }

    [SerializeField]
    private GameObject[] m_Apps;
    [SerializeField]
    private GameObject m_background;

    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(this);
            return;
        }
        m_Instance = this;
    }

    private void Start()
    {
        OSManager.Instance.Attach(this);
        ResetApps();

    }

    private void SetText()
    {
        foreach (var view in m_Apps)
        {
            var tmp = view.GetComponent<BaseAppManager>();
            if (tmp)
                view.GetComponent<BaseAppManager>().SetText();
        }
    }

    public string GetCurrentApp()
    {
        foreach (var app in m_Apps)
        {
            if (app.activeSelf)
                return app.GetComponent<BaseAppManager>().GetName();
        }
        return string.Empty;
    }

    public void RunApp(string name)
    {
        OSManager.Instance.RunApp();
        foreach (var app in m_Apps)
        {
            if (app.name == name + " Screen")
            {
                app.SetActive(true);
                break;
            }
        }
        m_background.SetActive(true);
    }

    public void ResetApps()
    {
        foreach (var app in m_Apps)
        {
            app.gameObject.SetActive(false);
        }
        m_background.SetActive(false);
    }

    public override void Notify(Subject subject)
    {
        SetText();
    }

    public void RefreshApp(string name)
    {
        foreach (var app in m_Apps)
        {
            if (app.name == name)
            {
                app.GetComponent<BaseAppManager>().ResetApp();
                return;
            }
        }
    }
}
