using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingManager : BaseAppManager
{
    [SerializeField]
    private Setting_Group[] m_SettingGroups;
    [SerializeField]
    private Profile_Layout m_Profile;
    [SerializeField]
    private TMP_Text m_SettingTitle;
    [SerializeField]
    private LText m_SettingTitleText;

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
        foreach (var settingGroup in m_SettingGroups)
        {
            settingGroup.SetText();
        }
        m_SettingTitle.text = m_SettingTitleText.GetText(OSManager.Instance.GetLanguage());
        m_Profile.SetProfile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
