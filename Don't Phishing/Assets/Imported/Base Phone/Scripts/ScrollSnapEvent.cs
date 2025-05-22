using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollSnapEvent : MonoBehaviour
{
    [SerializeField]
    private int m_enableIndex = 1;

    private ScrollSnap m_scrollSnap;
    private int m_curIndex;

    void Start()
    {
        m_scrollSnap = GetComponent<ScrollSnap>();
    }

    public bool EventCheck()
    {
        if (m_curIndex == m_scrollSnap.GetCurrentItem())
            return false;
        m_curIndex = m_scrollSnap.GetCurrentItem();
        return true;
    }

    public bool GetEvent()
    {
        // TODO : �ε����� ���� �ִϸ��̼� �����ؾ���.
        if (m_scrollSnap.GetCurrentItem() == m_enableIndex)
            return true;    // ���
        else
            return false;    // ��� ����
    }
}
