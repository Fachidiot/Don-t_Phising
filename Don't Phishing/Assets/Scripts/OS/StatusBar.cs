using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusBar : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_Time;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void SetTime()
    {
        m_Time.text = OSManager.Instance.GetTime();
    }
}
