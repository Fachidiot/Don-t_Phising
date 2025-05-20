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

    void Update()
    {
        if (!m_scrollSnap.IsSnapped && m_scrollSnap.CurrentItem != m_enableIndex)
        {
            OSManager.Instance.MainScreen.SetActive(true);
        }
        else
        {
            if (m_scrollSnap.CurrentItem != m_enableIndex)
            {
                OSManager.Instance.MainScreen.SetActive(false);
            }
        }
    }
}
