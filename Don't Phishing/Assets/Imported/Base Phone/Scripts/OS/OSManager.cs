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

public enum Status
{
    Idle,
    Notification,
    Control,
    RunApp
}

public class OSManager : Subject
{
    private static OSManager m_Instance;
    public static OSManager Instance { get { return m_Instance; } }

    [Header("Screens")]
    [SerializeField]
    private GameObject m_mainScreen;
    [SerializeField]
    private GameObject m_lockScreen;
    [SerializeField]
    private GameObject m_controlScreen;
    [SerializeField]
    private Image m_brightness;
    [Header("Background")]
    [SerializeField]
    private BackgroundManager m_background;

    [Header("System Language")]
    [SerializeField]
    private Language m_language;

    [SerializeField]
    private bool m_isLocked = true;
    [SerializeField]
    private Status m_currentStatus;

    [SerializeField]
    private GameObject m_bottombar;

    // TODO : 추후에 Control Screen 스크립트로 받아오자
    [Header("Text Mesh Pro")]
    [SerializeField]
    private TMP_Text m_TDate;
    [SerializeField]
    private TMP_Text m_TLanguage;
    [SerializeField]
    private bool m_Debug;

    private float m_Volume = 1f;
    private Profile m_profile;

    private ScrollSnap m_lockSnap;
    private ScrollSnap m_controlSnap;

    private void Awake()
    {
        if (m_Instance != null)
        {   // Singleton
            m_Instance.transform.parent.gameObject.SetActive(true);
            Destroy(transform.parent.gameObject);
            return;
        }
        m_Instance = this;
        DontDestroyOnLoad(gameObject.transform.parent.gameObject);

        SetDate(m_language);
        m_lockSnap = m_lockScreen.GetComponent<ScrollSnap>();
        m_controlSnap = m_controlScreen.GetComponent<ScrollSnap>();

        // Temp Profile
        m_profile = new Profile("User", "Sprites/Icons/channels4_profile");
    }

    private void Update()
    {
        CheckStatus();

        if (m_isLocked)
        {   // screen is locked.

        }
        else
        {   // screen is unlock.

        }

        switch (m_currentStatus)
        {
            case Status.Idle:
                m_bottombar.SetActive(false);
                m_mainScreen.SetActive(true);
                break;
            case Status.Notification:
                m_bottombar.SetActive(true);
                m_mainScreen.SetActive(false);
                break;
            case Status.Control:
                m_bottombar.SetActive(true);
                m_mainScreen.SetActive(false);
                break;
            case Status.RunApp:
                m_bottombar.SetActive(true);
                m_mainScreen.SetActive(false);
                break;
        }
    }

    public Profile GetProfile()
    {
        return m_profile;
    }

    private void CheckStatus()
    {
        if (m_controlSnap.GetCurrentItem() == 2)
        {
            m_currentStatus = Status.Control;
            return;
        }
        else if (m_lockSnap.GetCurrentItem() == 2)
        {
            m_currentStatus = Status.Notification;
            return;
        }
        if (m_currentStatus == Status.RunApp)
            return;
        m_currentStatus = Status.Idle;
    }
    public void RunApp()
    {
        m_currentStatus = Status.RunApp;
    }
    public void EndApp()
    {
        m_currentStatus = Status.Idle;
    }

    #region Language
    public void SetLanguage(int language)
    {
        m_language = (Language)language;
        SetDate(m_language);
        NotifyObservers();
    }

    public Language GetLanguage()
    {
        return m_language;
    }
    #endregion
    #region Screen_Controls
    public void ChangeBackground(int index)
    {
        m_background.UpdateBackground(index);
    }
    public void MainScreenActive(bool active)
    {
        m_mainScreen.gameObject.SetActive(active);
    }
    public void BackgroundActive(bool active)
    {
        m_background.gameObject.SetActive(active);
    }
    #endregion
    #region Media_Controls
    public void SetVolume(float volume)
    {
#if UNITY_EDITOR
        if (m_Debug)
            Debug.Log($"Volume: {volume}");
#endif
        m_Volume = volume;
    }
    public float GetCurrentVolume()
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
        m_brightness.color = color;
    }
    public float GetCurrentBrightness()
    {
        return m_brightness.color.a;
    }
    #endregion
    #region Times
    public string GetTime()
    {
        return TimeUtils.GetTime();
    }
    private void SetDate(Language language)
    {
        m_TDate.text = TimeUtils.GetDate(GetCulture(language));
    }
    private CultureInfo GetCulture(Language language)
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
                return DateTime.Now.ToString(("MM")) + "月";
            case "ko":
                return DateTime.Now.ToString(("MM")) + "월";
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
                return DateTime.Now.ToString(("dd")) + "日";
            case "ko":
                return DateTime.Now.ToString(("dd")) + "일";
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
                        return "月曜日";
                    case DayOfWeek.Tuesday:
                        return "火曜日";
                    case DayOfWeek.Wednesday:
                        return "水曜日";
                    case DayOfWeek.Thursday:
                        return "木曜日";
                    case DayOfWeek.Friday:
                        return "金曜日";
                    case DayOfWeek.Saturday:
                        return "土曜日";
                    case DayOfWeek.Sunday:
                        return "日曜日";
                    default:
                        return null;
                }
            case "ko":
                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return "월요일";
                    case DayOfWeek.Tuesday:
                        return "화요일";
                    case DayOfWeek.Wednesday:
                        return "수요일";
                    case DayOfWeek.Thursday:
                        return "목요일";
                    case DayOfWeek.Friday:
                        return "금요일";
                    case DayOfWeek.Saturday:
                        return "토요일";
                    case DayOfWeek.Sunday:
                        return "일요일";
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