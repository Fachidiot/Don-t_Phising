using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NestedHorizontalScrollRect : ScrollRect
{
    [SerializeField]
    private ScrollRect m_ScrollRect;
    private bool m_isVerticalDrag;

    public void SetScrollRect(ScrollRect scrollRect)
    {
        m_ScrollRect = scrollRect;
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        m_isVerticalDrag = Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y);

        if (m_isVerticalDrag && m_ScrollRect != null)
            m_ScrollRect.OnBeginDrag(eventData);
        else
            base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (m_isVerticalDrag && m_ScrollRect != null)
            m_ScrollRect.OnDrag(eventData);
        else
            base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (m_isVerticalDrag && m_ScrollRect != null)
            m_ScrollRect.OnEndDrag(eventData);
        else
            base.OnEndDrag(eventData);
    }
}
