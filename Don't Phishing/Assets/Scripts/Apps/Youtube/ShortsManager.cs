using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortsManager : MonoBehaviour
{
    [SerializeField]
    private Shorts_Layout[] m_Layouts;
    [SerializeField]
    private ScrollSnap m_SnapManager;
    [SerializeField]


    void Update()   
    {
        switch (-m_SnapManager.GetContentPosition())
        {
            case 0:
                m_Layouts[0].Play();
                m_Layouts[1].Stop();
                m_Layouts[2].Stop();
                break;
            case 1:
                m_Layouts[0].Stop();
                m_Layouts[1].Play();
                m_Layouts[2].Stop();
                break;
            case 2:
                m_Layouts[0].Stop();
                m_Layouts[1].Stop();
                m_Layouts[2].Play();
                break;
        }
    }
}
