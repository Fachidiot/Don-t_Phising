using System;
using System.Globalization;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Language
{
    English,
    Korean,
    Japanese
}

//[ExecuteAlways]
public class OSManager : Subject
{
    private static OSManager m_Instance;
    public static OSManager Instance { get { return m_Instance; } }

    [Header("Screens")]
    [SerializeField]
    private Mask m_MaskScreen;
    [SerializeField]
    private GameObject m_MainScreen;
    [SerializeField]
    private GameObject m_HomeScreen;
    [SerializeField]
    private GameObject m_ControlScreen;
    [SerializeField]
    private Image m_Brightness;
    [SerializeField]
    private GameObject m_ActionBar;
    [Space]
    [Header("System Language")]
    [SerializeField]
    private Language m_Language;
    [Header("Background")]
    [SerializeField]
    private BackgroundManager m_Background;
    [Header("Text Mesh Pro")]
    [SerializeField]
    private TMP_Text m_TDate;
    [SerializeField]
    private TMP_Text m_TLanguage;
    [Space(10)]
    [SerializeField]
    private bool m_Debug;

    private float m_Volume = 1f;
    private Profile m_profile;

    private void Awake()
    {
        if (m_Instance != null)
        {
            m_Instance.transform.parent.gameObject.SetActive(true);
            Destroy(transform.parent.gameObject);
            return;
        }
        m_Instance = this;
        DontDestroyOnLoad(gameObject.transform.parent.gameObject);

        // Temp Profile
        m_profile = new Profile("User", "Sprites/Icons/channels4_profile");
    }

    private void Start()
    {
        //InitLanguage();
        m_HomeScreen.SetActive(true);
        if (!m_MaskScreen.IsActive())
            m_MaskScreen.enabled = true;
        SetDate(m_Language);
    }

    public void HomeScreenActive(bool active)
    {
        m_HomeScreen.SetActive(active);
    }

    public void MainScreenActive(bool active)
    {
        m_MainScreen.gameObject.SetActive(active);
    }

    public Profile GetProfile()
    {
        return m_profile;
    }

    #region Language
    public void SetLanguage(int language)
    {
        m_Language = (Language)language;
        SetDate(m_Language);
        NotifyObservers();
    }

    public Language GetLanguage()
    {
        return m_Language;
    }
    #endregion

    #region Controls
    public void ChangeBackground(int index)
    {
        m_Background.UpdateBackground(index);
    }

    public void BackgroundActive(bool active)
    {
        m_Background.gameObject.SetActive(active);
    }

    public void SetVolume(float volume)
    {
#if UNITY_EDITOR
        if (m_Debug)
            Debug.Log($"Volume: {volume}");
#endif
        m_Volume = volume;
    }

    public float GetVolume()
    {
        return m_Volume;
    }

    public void SetBrightness(float brightness)
    {
#if UNITY_EDITOR
        if (m_Debug)
            Debug.Log($"Brightness: {brightness}");
#endif
        Color color = new Color(0, 0, 0, (1 - Mathf.Clamp(brightness, 0.1f, 1)));
        m_Brightness.color = color;
    }
    #endregion

    #region Times
    public string GetTime()
    {
        return TimeUtils.GetTime();
    }

    public void SetDate(Language language)
    {
        m_TDate.text = TimeUtils.GetDate(GetCulture(language));
    }

    public CultureInfo GetCulture(Language language)
    {
        switch (language)
        {
            case Language.English:
                return new CultureInfo("en-US");
            case Language.Korean:
                return new CultureInfo("ko-KR");
            case Language.Japanese:
                return new CultureInfo("ja-JP");
            default:
                return new CultureInfo("en-US");
        }
    }
#endregion
}

public class TimeUtils
{
    public static string GetTime()
    {
        return GetHour() + ":" + GetMinute();
    }

    public static string GetDate(CultureInfo cultureInfo)
    {
        switch (cultureInfo.TwoLetterISOLanguageName)
        {
            case "en":
                return GetDayOfWeek(cultureInfo) + ", " + GetMonth(cultureInfo) + " " + GetDay(cultureInfo);
            case "ja":
                return GetMonth(cultureInfo) + GetDay(cultureInfo) + GetDayOfWeek(cultureInfo);
            case "ko":
                return GetMonth(cultureInfo) + " " + GetDay(cultureInfo) + " " + GetDayOfWeek(cultureInfo);
            default:
                return null;
        }
    }

    public static string GetMonth(CultureInfo cultureInfo)
    {
        switch (cultureInfo.TwoLetterISOLanguageName)
        {
            case "en":
                return DateTime.Now.ToString(("MM"));
            case "ja":
                return DateTime.Now.ToString(("MM")) + "��";
            case "ko":
                return DateTime.Now.ToString(("MM")) + "��";
            default:
                return null;

        }
    }

    public static string GetDay(CultureInfo cultureInfo)
    {
        switch (cultureInfo.TwoLetterISOLanguageName)
        {
            case "en":
                return DateTime.Now.ToString(("dd"));
            case "ja":
                return DateTime.Now.ToString(("dd")) + "��";
            case "ko":
                return DateTime.Now.ToString(("dd")) + "��";
            default:
                return null;

        }
    }

    public static string GetDayOfWeek(CultureInfo cultureInfo)
    {
        switch (cultureInfo.TwoLetterISOLanguageName)
        {
            case "en":
                return DateTime.Now.DayOfWeek.ToString();
            case "ja":
                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return "������";
                    case DayOfWeek.Tuesday:
                        return "������";
                    case DayOfWeek.Wednesday:
                        return "�����";
                    case DayOfWeek.Thursday:
                        return "������";
                    case DayOfWeek.Friday:
                        return "������";
                    case DayOfWeek.Saturday:
                        return "������";
                    case DayOfWeek.Sunday:
                        return "������";
                    default:
                        return null;
                }
            case "ko":
                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return "������";
                    case DayOfWeek.Tuesday:
                        return "ȭ����";
                    case DayOfWeek.Wednesday:
                        return "������";
                    case DayOfWeek.Thursday:
                        return "�����";
                    case DayOfWeek.Friday:
                        return "�ݿ���";
                    case DayOfWeek.Saturday:
                        return "�����";
                    case DayOfWeek.Sunday:
                        return "�Ͽ���";
                    default:
                        return null;
                }
            default:
                return null;
        }
    }

    public static string GetHour()
    {
        return DateTime.Now.ToString(("HH"));
    }

    public static string GetMinute()
    {
        return DateTime.Now.ToString(("mm"));
    }
}