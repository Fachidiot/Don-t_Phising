using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollSnapEvent : MonoBehaviour
{
    [SerializeField]
    private int m_enableIndex = 1;

    private ScrollSnap m_scrollSnap;

    void Start()
    {
        m_scrollSnap = GetComponent<ScrollSnap>();
    }

    public bool EventCheck()
    {
        // TODO : 인덱스에 따라서 애니메이션 결정해야함.
        if (m_scrollSnap.GetCurrentItem() == m_enableIndex)
            return false;    // 잠금
        else
            return true;    // 잠금 해제
    }

    //void Update()
    //{
    //    if (!m_scrollSnap.IsSnapped && m_scrollSnap.GetCurrentItem() != m_enableIndex)
    //    {   // 
    //        OSManager.Instance.MainScreen.SetActive(true);
    //    }
    //    else
    //    {
    //        if (m_scrollSnap.GetCurrentItem() != m_enableIndex)
    //        {
    //            OSManager.Instance.MainScreen.SetActive(false);
    //        }
    //    }
    //}
}
