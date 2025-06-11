using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : BaseAppManager
{
    [SerializeField] private Setting_Group[] m_settingGroups;
    [SerializeField] private Profile_Layout m_profile;
    [SerializeField] private TMP_Text m_settingTitle;
    [SerializeField] private LText m_settingTitleText;

    private void Start()
    {
        SetText();
    }

    public override void ResetApp()
    {
        return;
    }

    public override void SetText()
    {
        foreach (var settingGroup in m_settingGroups)
        {
            settingGroup.SetText();
        }
        m_settingTitle.text = m_settingTitleText.GetText(OSManager.Instance.GetLanguage());
        m_profile.SetProfile();
    }
}