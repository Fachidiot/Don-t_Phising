using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class SettingManager : BaseAppManager
{
    public static SettingManager m_Instance;
    [SerializeField] private Setting_Group[] m_SettingGroups;
    [SerializeField] private Profile_Layout m_Profile;
    [SerializeField] private TMP_Text m_SettingTitle;
    [SerializeField] private LText m_SettingTitleText;

    [SerializeField] private VirtualPhoneSetting m_setting;
    [SerializeField] public Toggle_Button AirplaneModeToggle;
    [SerializeField] public Toggle_Button WiFiToggle;
    [SerializeField] public Toggle_Button BluetoothToggle;
    [SerializeField] public Toggle_Button CellularToggle;
    [SerializeField] public Toggle_Button Battery_PercentToggle;
    [SerializeField] public Toggle_Button Battery_LowModeToggle;
    [SerializeField] public Toggle_Button VPNToggle;
    [SerializeField] public Toggle_Button DarkModeToggle;
    [SerializeField] public Toggle_Button AutoModeToggle;
    [SerializeField] public Toggle_Button TextBoldToggle;
    [SerializeField] public Toggle_Button HapticToggle;
    [SerializeField] public Toggle_Button LockSoundToggle;
    [SerializeField] public Toggle_Button UsingPasswordToggle;

    private string OptionDataFileName = "\\VirtualPhoneSetting.json";

    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(this);
            return;
        }
        m_Instance = this;

        LoadOptionData();
        SaveOptionData();
        InitMenuLayouts();

        SetText();
    }

    private void InitMenuLayouts()
    {
        //Slider.SetValueWithoutNotify(value);
        //Dropdown.SetValueWithoutNotify(value);

        // Airplane
        AirplaneModeToggle.SetValue(m_setting.AirplaneMode);
        // WiFi
        WiFiToggle.SetValue(m_setting.WiFi);
        // Bluetooth
        BluetoothToggle.SetValue(m_setting.Bluetooth);
        // Cellular
        CellularToggle.SetValue(m_setting.Cellular);
        // Battery
        Battery_PercentToggle.SetValue(m_setting.Battery_Percent);
        Battery_LowModeToggle.SetValue(m_setting.Battery_LowMode);
        // Vpn
        VPNToggle.SetValue(m_setting.VPN);
        // Display
        //DarkModeToggle.SetValue(m_setting.DarkMode);
        AutoModeToggle.SetValue(m_setting.AutoMode);
        TextBoldToggle.SetValue(m_setting.TextBold);
        // Sound
        //HapticToggle.SetValue(m_setting.Haptic);
        //LockSoundToggle.SetValue(m_setting.LockSound);
        // Password
        //UsingPasswordToggle.SetValue(m_setting.UsingPassword);
    }

    public void UpdateSetting(VirtualPhoneSettingType _type, bool _value, short _sort)
    {
        switch(_type)
        {
            case VirtualPhoneSettingType.Airplane:
                m_setting.AirplaneMode = _value; break;
            case VirtualPhoneSettingType.WiFi:
                m_setting.WiFi = _value; break;
            case VirtualPhoneSettingType.Bluetooth:
                m_setting.Bluetooth = _value; break;
            case VirtualPhoneSettingType.Cellular:
                m_setting.Cellular = _value; break;
            case VirtualPhoneSettingType.Battery:
                if (0 == _sort)
                    m_setting.Battery_Percent = _value;
                else if (1 == _sort)
                    m_setting.Battery_LowMode = _value;
                break;
            case VirtualPhoneSettingType.Vpn:
                m_setting.VPN = _value; break;
            case VirtualPhoneSettingType.General:

                break;
            case VirtualPhoneSettingType.Display:
                if (0 == _sort)
                    m_setting.DarkMode = _value;
                else if (1 == _sort)
                    m_setting.AutoMode = _value;
                else if (2 == _sort)
                    m_setting.TextBold = _value;
                break;
            case VirtualPhoneSettingType.Background:

                break;
            case VirtualPhoneSettingType.ControlCenter:

                break;
            case VirtualPhoneSettingType.Camera:

                break;
            case VirtualPhoneSettingType.HomeScreen:

                break;
            case VirtualPhoneSettingType.Notification:

                break;
            case VirtualPhoneSettingType.Sound:

                break;
            case VirtualPhoneSettingType.Password:
                m_setting.UsingPassword = _value;
                break;
        }
        SaveOptionData();
    }

    private void LoadOptionData()
    {
        string filePath = Application.persistentDataPath + OptionDataFileName;

        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            m_setting = JsonUtility.FromJson<VirtualPhoneSetting>(FromJsonData);
        }

        // 저장된 게임이 없다면
        else
        {
            ResetOptionData();
        }
    }

    // 옵션 데이터 저장하기
    public void SaveOptionData()
    {
        string ToJsonData = JsonUtility.ToJson(m_setting);
        string filePath = Application.persistentDataPath + OptionDataFileName;

        // 이미 저장된 파일이 있다면 덮어쓰기
        File.WriteAllText(filePath, ToJsonData);
    }

    // 데이터를 초기화(새로 생성 포함)하는경우
    public void ResetOptionData()
    {
        print("새로운 옵션 파일 생성");
        m_setting = new VirtualPhoneSetting();

        //새로 생성하는 데이터들은 이곳에 선언하기


        //옵션 데이터 저장
        SaveOptionData();
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

[Serializable]
public struct VirtualPhoneSetting
{
    // Connection
    public bool AirplaneMode;
    public bool WiFi;
    public string[] WiFiList;
    public bool Bluetooth;
    public string[] BluetoothList;
    public bool Cellular;
    public bool Battery_Percent;
    public bool Battery_LowMode;
    public short[] BatteryGraph;
    public bool VPN;

    // Setting Group1
    public bool DarkMode;
    public bool AutoMode;
    public short TextSize;
    public bool TextBold;
    public string[] BackgroundList;

    // Setting Group2
    public bool Haptic;
    public string CallRing;
    public string SMSRing;
    public bool LockSound;
    public bool UsingPassword;
    public string Password;
}

public enum VirtualPhoneSettingType
{
    Airplane,
    WiFi,
    Bluetooth,
    Cellular,
    Battery,
    Vpn,
    General,
    Display,
    Background,
    ControlCenter,
    Camera,
    HomeScreen,
    Notification,
    Sound,
    Password
}