using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification_Layout : MonoBehaviour
{
    [SerializeField]
    private ScrollSnap m_ScrollSnap;
    [SerializeField]
    private NestedScrollRect m_NestedScrollRect;
    [SerializeField]
    private Button m_Button;

    private ScrollRect m_ScrollRect;

    private void Start()
    {
        m_NestedScrollRect.SetScrollRect(m_ScrollRect, true);
    }

    private void Update()
    {
        if (m_ScrollSnap.End)
        {
            Destroy(gameObject);
        }
    }
}
