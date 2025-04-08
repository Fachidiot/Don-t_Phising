using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Setting_Group : MonoBehaviour
{
    [SerializeField]
    private Setting_Layout[] m_SettingLayouts;

    public void SetText()
    {
        foreach (var settingLayout in m_SettingLayouts)
        {
            settingLayout.SetText();
        }
    }
}
