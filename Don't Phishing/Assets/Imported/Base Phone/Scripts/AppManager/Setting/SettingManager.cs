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

    [SerializeField] private Setting m_setting;
    public Setting Setting { get { return m_setting; } set { m_setting = value; } }

    private void Start()
    {
        SetText();
    }

    // Update is called once per frame
    void Update()
    {

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
}

public struct Setting
{
    bool AirplaneMode;

    bool WiFi;
    List<string> WiFiList;

    bool Bluetooth;
    List<string> BluetoothList;

    bool Cellular;

    bool Battery_Percent;
    bool Battery_LowMode;
    List<short> BatteryGraph;

    bool DarkMode;
    bool AutoMode;
    short TextSize;
    bool TextBold;

    List<string> BackgroundList;
    // Control Center Presets

    // Camera Presets

    // Notification Settings

    bool Haptic;
    AudioClip CallRing;
    AudioClip SMSRing;
    bool LockSound;

    bool UsingPassword;
    string Password;
}