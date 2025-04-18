using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification_Layout : MonoBehaviour
{
    [SerializeField]
    private ScrollSnap m_ScrollSnap;
    [SerializeField]
    private NestedHorizontalScrollRect m_NestedScrollRect;
    [SerializeField]
    private Button m_Button;

    private ScrollRect m_ScrollRect;

    private void Start()
    {
        if (!m_NestedScrollRect.HasScrollRect())
            m_NestedScrollRect.SetScrollRect(m_ScrollRect);
    }

    private void Update()
    {
        if (m_ScrollSnap.End)
        {
            Destroy(gameObject);
        }
    }
}
