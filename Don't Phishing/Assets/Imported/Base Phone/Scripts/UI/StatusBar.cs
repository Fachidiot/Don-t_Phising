using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Time;
    [SerializeField] private GameObject m_wifi;

    void Update()
    {
        if (m_Time.text != OSManager.Instance.GetTime())
            SetTime();
    }

    private void SetTime()
    {
        m_Time.text = OSManager.Instance.GetTime();
    }
}
