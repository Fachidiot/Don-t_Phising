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

    [SerializeField]
    private GameObject m_OS;
    public GameObject OS { get { return m_OS; } }
    [SerializeField]
    private GameObject m_HomeScreen;
    public GameObject HomeScreen { get { return m_HomeScreen; } }
    [SerializeField]
    private GameObject m_ControlScreen;
    public GameObject ControlScreen { get { return m_ControlScreen; } }
    [SerializeField]
    private Image m_Brightness;
    [SerializeField]
    private Language m_Language;
    [SerializeField]
    private BackgroundManager[] m_Backgrounds;
    public int m_BackgroundIndex = 0;
    [Header("Text Mesh Pro")]
    [SerializeField]
    private TMP_Text m_TDate;
    [SerializeField]
    private TMP_Text m_TLanguage;
    [Space(10)]
    [SerializeField]
    private bool m_Debug;

    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        m_Instance = this;
        DontDestroyOnLoad(gameObject.transform.parent.gameObject);
    }

    private void Start()
    {
        //InitLanguage();
        m_HomeScreen.SetActive(true);
        SetDate(m_Language);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (m_Backgrounds[0].Index != m_BackgroundIndex)
            ChangeBackground(m_BackgroundIndex);
#endif
    }

    public string GetTime()
    {
        return TimeUtils.GetTime();
    }

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

    public void SetDate(Language language)
    {
        m_TDate.text = TimeUtils.GetDate(GetCulture(language));
    }

    public void ChangeBackground(int index)
    {
        foreach(var background in m_Backgrounds)
        {
            background.UpdateBackground(index);
        }
    }

    public void SetVolume(float volume)
    {
#if UNITY_EDITOR
        if (m_Debug)
            Debug.Log($"Volume: {volume}");
#endif
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

    private void InitLanguage()
    {
        if (PlayerPrefs.GetInt("Language", 9) != 9)
        {
            m_Language = (Language)PlayerPrefs.GetInt("Language");
            return;
        }

        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;

        switch (cultureInfo.TwoLetterISOLanguageName)
        {
            case "en":
                m_Language = Language.English;
                break;
            case "ko":
                m_Language = Language.Korean;
                break;
            case "ja":
                m_Language = Language.Japanese;
                break;
            default:
                m_Language = Language.English;
                break;
        }
        PlayerPrefs.SetInt("Language", (int)m_Language);
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
                return DateTime.Now.ToString(("MM")) + "Í≈";
            case "ko":
                return DateTime.Now.ToString(("MM")) + "ø˘";
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
                return DateTime.Now.ToString(("dd")) + "ÏÌ";
            case "ko":
                return DateTime.Now.ToString(("dd")) + "¿œ";
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
                        return "Í≈Ë¯ÏÌ";
                    case DayOfWeek.Tuesday:
                        return "˚˝Ë¯ÏÌ";
                    case DayOfWeek.Wednesday:
                        return "‚©Ë¯ÏÌ";
                    case DayOfWeek.Thursday:
                        return "Ÿ Ë¯ÏÌ";
                    case DayOfWeek.Friday:
                        return "–›Ë¯ÏÌ";
                    case DayOfWeek.Saturday:
                        return "˜œË¯ÏÌ";
                    case DayOfWeek.Sunday:
                        return "ÏÌË¯ÏÌ";
                    default:
                        return null;
                }
            case "ko":
                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return "ø˘ø‰¿œ";
                    case DayOfWeek.Tuesday:
                        return "»≠ø‰¿œ";
                    case DayOfWeek.Wednesday:
                        return "ºˆø‰¿œ";
                    case DayOfWeek.Thursday:
                        return "∏Òø‰¿œ";
                    case DayOfWeek.Friday:
                        return "±›ø‰¿œ";
                    case DayOfWeek.Saturday:
                        return "≈‰ø‰¿œ";
                    case DayOfWeek.Sunday:
                        return "¿œø‰¿œ";
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