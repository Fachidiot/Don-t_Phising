using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoutubeManager : BaseAppManager
{
    [SerializeField]
    private TabManager m_TabManager;
    public override void SetText()
    {
        m_TabManager.SetText();
    }

    public override void ResetApp()
    {
        return;
    }
}
