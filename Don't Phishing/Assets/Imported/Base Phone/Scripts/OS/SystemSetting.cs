using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SystemSetting : Subject
{
    public static SystemSetting Instance;
    [SerializeField] private TMP_FontAsset[] FontAssets;
    [SerializeField] private Image m_brightness;
    [SerializeField] private StatusBar m_statusManager;
    [SerializeField] private VirtualPhoneSetting m_setting;
    [SerializeField] private Toggle_Button AirplaneModeToggle;
    [SerializeField] private Toggle_Button WiFiToggle;
    [SerializeField] private Toggle_Button BluetoothToggle;
    [SerializeField] private Toggle_Button CellularToggle;
    [SerializeField] private Toggle_Button Battery_PercentToggle;
    [SerializeField] private Toggle_Button Battery_LowModeToggle;
    [SerializeField] private Toggle_Button VPNToggle;
    [SerializeField] private Toggle_Button DarkModeToggle;
    [SerializeField] private Toggle_Button AutoModeToggle;
    [SerializeField] private Toggle_Button TextBoldToggle;
    [SerializeField] private Toggle_Button HapticToggle;
    [SerializeField] private Toggle_Button LockSoundToggle;
    [SerializeField] private Toggle_Button UsingPasswordToggle;

    private string OptionDataFileName = "\\VirtualPhoneSetting.json";
    private float m_volume = 1f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        LoadOptionData();
        SaveOptionData();
        InitMenuLayouts();
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

    #region Media_Controls
    public void SetVolume(float _volume)
    {
        Debug.Log($"Volume: {_volume}");
        m_setting.Volume = _volume;
        m_volume = _volume;
    }
    public float GetCurrentVolume()
    {
        return m_volume;
    }
    public void SetBrightness(float _brightness)
    {
        Color color = new Color(0, 0, 0, (1 - Mathf.Clamp(_brightness, 0.1f, 1)));
        m_brightness.color = color;
    }

    public float GetCurrentBrightness()
    {
        return 1 - m_brightness.color.a;
    }

    #endregion

    public void UpdateSetting(VirtualPhoneSettingType _type, bool _value, short _sort)
    {
        switch (_type)
        {
            case VirtualPhoneSettingType.Airplane:
                m_setting.AirplaneMode = _value;
                m_statusManager.AirplaneMode(_value);
                break;
            case VirtualPhoneSettingType.WiFi:
                m_setting.WiFi = _value;
                m_statusManager.WiFiMode(_value);
                break;
            case VirtualPhoneSettingType.Bluetooth:
                m_setting.Bluetooth = _value;
                break;
            case VirtualPhoneSettingType.Cellular:
                m_setting.Cellular = _value;
                m_statusManager.CellularMode(_value);
                break;
            case VirtualPhoneSettingType.Battery:
                if (0 == _sort)
                {
                    m_setting.Battery_Percent = _value;
                    m_statusManager.BatteryPercent(_value); 
                }
                else if (1 == _sort)
                    m_setting.Battery_LowMode = _value;
                break;
            case VirtualPhoneSettingType.Vpn:
                m_setting.VPN = _value; break;
            case VirtualPhoneSettingType.General:
                if (0 == _sort)
                    m_setting.TimeFormat = _value ? TimeFormat.Army : TimeFormat.Normal;
                break;
            case VirtualPhoneSettingType.Display:
                if (0 == _sort)
                    m_setting.DarkMode = _value;
                else if (1 == _sort)
                    m_setting.AutoMode = _value;
                else if (2 == _sort)
                {
                    m_setting.TextBold = _value;
                    NotifyObservers();
                }
                    break;
            case VirtualPhoneSettingType.ControlCenter:
                m_setting.ControlCenterAccessToApp = _value;
                break;
            case VirtualPhoneSettingType.Camera:
                if (0 == _sort)
                    m_setting.QR = _value;
                if (1 == _sort)
                    m_setting.GuideLine = _value;
                break;
            case VirtualPhoneSettingType.HomeScreen:
                m_setting.NotificationShow = _value;
                break;
            case VirtualPhoneSettingType.Sound:
                if (0 == _sort)
                    m_setting.Haptic = _value;
                if (1 == _sort)
                    m_setting.LockSound = _value;
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

    public TimeFormat GetTimeSetting()
    {
        return m_setting.TimeFormat;
    }

    public TMP_FontAsset GetTextFont()
    {
        return FontAssets[m_setting.TextFont];
    }

    public float GetTextSize()
    {
        return m_setting.TextSize;
    }

    public FontStyles GetTextBold()
    {
        return m_setting.TextBold ? FontStyles.Bold : FontStyles.Normal;
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
    public TimeFormat TimeFormat;
    public Language SystemLanguage;
    public bool DarkMode;
    public bool AutoMode;
    public short TextFont;
    public short TextSize;
    public bool TextBold;
    public string[] BackgroundList;
    public bool ControlCenterAccessToApp;
    public ScreenResolution VideoRecordResolution;
    public ScreenResolution CameraCaptureResolution;
    public bool QR;
    public bool GuideLine;
    public bool NotificationShow;

    // Setting Group2
    public float Volume;
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

public enum ScreenResolution
{
    UHD,
    QHD,
    FHD,
    HD,
    SD
}

public enum TimeFormat
{
    Normal,
    Army
}