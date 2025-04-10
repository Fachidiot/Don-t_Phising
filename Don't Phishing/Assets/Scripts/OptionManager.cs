using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    //메뉴가 켜져 있는 상태인가?
    private static bool m_IsOpenMenu = false;

    //메뉴가 담길 부모 오브젝트
    [SerializeField]
    private GameObject m_MenuParent;

    [Header("Graphic Setting")]
    [SerializeField]
    private TMPro.TMP_Dropdown m_QualityDropdown;

    [Header("Audio Setting")]
    [SerializeField]
    private Slider m_MasterSoundSlider;
    [SerializeField]
    private Slider m_BGMSoundSlider;
    [SerializeField]
    private Slider m_EffectSoundSlider;

    [Header("System Setting")]
    [SerializeField]
    private TMPro.TMP_Dropdown m_LanguageDropdown;

    [Header("Customize Setting")]
    [SerializeField]
    private GameObject mFPSButton;

    public void TryOpenMenu()
    {
        //현재 메뉴가 열려있는 상태라면?
        if (m_IsOpenMenu)
        {
            //메뉴 닫기
            CloseMenu();
        }
        else
        {
            //메뉴 열기
            OpenMenu();
        }
        m_IsOpenMenu = !m_IsOpenMenu;
    }

    public void InitMenuLayouts()
    {
        m_MasterSoundSlider.SetValueWithoutNotify(OptionDataManager.Instance.OptionData.m_MasterVolume);
        m_BGMSoundSlider.SetValueWithoutNotify(OptionDataManager.Instance.OptionData.m_BgmVolume);
        m_EffectSoundSlider.SetValueWithoutNotify(OptionDataManager.Instance.OptionData.m_EffectVolume);
        m_QualityDropdown.SetValueWithoutNotify(OptionDataManager.Instance.OptionData.m_CurrentSelectQualityID);

        switch (OptionDataManager.Instance.OptionData.language)
        {
            case SystemLanguage.Korean: //한국어 > 0번 인덱스
                {
                    m_LanguageDropdown.SetValueWithoutNotify(0);
                    break;
                }
            case SystemLanguage.Japanese:
                {
                    m_LanguageDropdown.SetValueWithoutNotify(2);
                    break;
                }
            case SystemLanguage.English:    //미지원이나 영어면 > 1번 인덱스 (ENG)
            default:
                {
                    m_LanguageDropdown.SetValueWithoutNotify(1);
                    break;
                }
        }

        m_MenuParent.SetActive(false);
    }

    public void OpenMenu()
    {
        m_MenuParent.SetActive(true);

        //UtilityManager.UnlockCursor();
        //PlayerController.Lock();
        //Camera_FPS_TPS.Lock();
    }

    public void CloseMenu()
    {
        m_MenuParent.SetActive(false);

        //만약 액션액티브(Detailed View)가 활성화 되어있는경우에는 변동이 없다. 
        //if (!DetailedViewActionManager.GetIsActionActive())
        //{
        //    UtilityManager.LockCursor();
        //    PlayerController.Unlock();
        //    Camera_FPS_TPS.Unlock();
        //}
    }

    public void SelectQualityDropdown()
    {
        QualitySettings.SetQualityLevel(m_QualityDropdown.value, true);
        OptionDataManager.Instance.OptionData.m_CurrentSelectQualityID = m_QualityDropdown.value;
        OptionDataManager.Instance.SaveOptionData();
    }

    public void SelectLangDropdown()
    {
        switch (m_LanguageDropdown.value)
        {
            //한국어
            case 0:
                {
                    OptionDataManager.Instance.OptionData.language = SystemLanguage.Korean;
                    break;
                }
            case 1:
                {
                    OptionDataManager.Instance.OptionData.language = SystemLanguage.English;
                    break;
                }
            case 2:
                {
                    OptionDataManager.Instance.OptionData.language = SystemLanguage.Japanese;
                    break;
                }
        }

        OptionDataManager.Instance.SaveOptionData();
        //LangDataManager.Instance.InitLang(OptionDataManager.Instance.OptionData.language);
    }

    public void MasterValueChanged()
    {
        //UtilityManager.mSound.SetBGMVolume(mBGMSoundSlider.value);
        OptionDataManager.Instance.OptionData.m_MasterVolume = m_MasterSoundSlider.value;
        OptionDataManager.Instance.SaveOptionData();
    }

    public void BGMValueChanged()
    {
        //UtilityManager.mSound.SetBGMVolume(mBGMSoundSlider.value);
        OptionDataManager.Instance.OptionData.m_BgmVolume = m_BGMSoundSlider.value;
        OptionDataManager.Instance.SaveOptionData();
    }

    public void EffectValueChanged()
    {
        //UtilityManager.mSound.SetEffectVolume(mEffectSoundSlider.value);
        OptionDataManager.Instance.OptionData.m_EffectVolume = m_EffectSoundSlider.value;
        OptionDataManager.Instance.SaveOptionData();
    }

    public static bool GetIsMenuOpen()
    {
        return m_IsOpenMenu;
    }
}