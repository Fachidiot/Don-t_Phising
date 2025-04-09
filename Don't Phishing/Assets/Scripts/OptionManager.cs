using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    //메뉴가 켜져 있는 상태인가?
    private static bool msIsOpenMenu = false;

    //메뉴가 담길 부모 오브젝트
    [SerializeField]
    private GameObject mMenuParent;

    [Header("Graphic Setting")]
    [SerializeField]
    private TMPro.TMP_Dropdown mQualityDropdown;

    [Header("Audio Setting")]
    [SerializeField]
    private Slider mBGMSoundSlider;
    [SerializeField]
    private Slider mEffectSoundSlider;

    [Header("System Setting")]
    [SerializeField]
    private TMPro.TMP_Dropdown mLangDropdown;

    [Header("Customize Setting")]
    [SerializeField]
    private GameObject mFPSButton;

    public void TryOpenMenu()
    {
        //현재 메뉴가 열려있는 상태라면?
        if (msIsOpenMenu)
        {
            //메뉴 닫기
            CloseMenu();
        }
        else
        {
            //메뉴 열기
            OpenMenu();
        }
        msIsOpenMenu = !msIsOpenMenu;
    }

    public void InitMenuLayouts()
    {
        mBGMSoundSlider.SetValueWithoutNotify(OptionDataManager.Instance.OptionData.bgmVolume);
        mEffectSoundSlider.SetValueWithoutNotify(OptionDataManager.Instance.OptionData.effectVolume);
        mQualityDropdown.SetValueWithoutNotify(OptionDataManager.Instance.OptionData.currentSelectQualityID);

        switch (OptionDataManager.Instance.OptionData.language)
        {
            case SystemLanguage.Korean: //한국어 > 0번 인덱스
                {
                    mLangDropdown.SetValueWithoutNotify(0);
                    break;
                }
            case SystemLanguage.Japanese:
                {
                    mLangDropdown.SetValueWithoutNotify(2);
                    break;
                }
            case SystemLanguage.English:    //미지원이나 영어면 > 1번 인덱스 (ENG)
            default:
                {
                    mLangDropdown.SetValueWithoutNotify(1);
                    break;
                }
        }

        mMenuParent.SetActive(false);
    }

    public void OpenMenu()
    {
        mMenuParent.SetActive(true);

        //UtilityManager.UnlockCursor();
        //PlayerController.Lock();
        //Camera_FPS_TPS.Lock();
    }

    public void CloseMenu()
    {
        mMenuParent.SetActive(false);

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
        QualitySettings.SetQualityLevel(mQualityDropdown.value, true);
        OptionDataManager.Instance.OptionData.currentSelectQualityID = mQualityDropdown.value;
        OptionDataManager.Instance.SaveOptionData();
    }

    public void SelectLangDropdown()
    {
        switch (mLangDropdown.value)
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

    public void BGMValueChanged()
    {
        //UtilityManager.mSound.SetBGMVolume(mBGMSoundSlider.value);
        OptionDataManager.Instance.OptionData.bgmVolume = mBGMSoundSlider.value;
        OptionDataManager.Instance.SaveOptionData();
    }

    public void EffectValueChanged()
    {
        //UtilityManager.mSound.SetEffectVolume(mEffectSoundSlider.value);
        OptionDataManager.Instance.OptionData.effectVolume = mEffectSoundSlider.value;
        OptionDataManager.Instance.SaveOptionData();
    }

    public static bool GetIsMenuOpen()
    {
        return msIsOpenMenu;
    }
}