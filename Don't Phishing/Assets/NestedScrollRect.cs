using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NestedScrollRect : ScrollRect
{
    private ScrollRect m_ScrollRect;
    private bool m_isHorizontalDrag;
    private bool m_isInverse;

    public void SetScrollRect(ScrollRect scrollRect, bool inverse = false)
    {
        m_ScrollRect = scrollRect;
        m_isInverse = inverse;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        m_isHorizontalDrag = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);
        if (m_isInverse)
            m_isHorizontalDrag = !m_isHorizontalDrag;

        if (m_isHorizontalDrag && m_ScrollRect != null)
            m_ScrollRect.OnBeginDrag(eventData);
        else
            base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (m_isHorizontalDrag && m_ScrollRect != null)
            m_ScrollRect.OnDrag(eventData);
        else
            base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (m_isHorizontalDrag && m_ScrollRect != null)
            m_ScrollRect.OnEndDrag(eventData);
        else
            base.OnEndDrag(eventData);
    }
}
