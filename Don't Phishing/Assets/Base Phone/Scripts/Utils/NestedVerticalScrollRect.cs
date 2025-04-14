using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NestedVerticalScrollRect : ScrollRect
{
    private ScrollRect m_ScrollRect;
    private bool m_isHorizontalDrag;

    public void SetScrollRect(ScrollRect scrollRect)
    {
        m_ScrollRect = scrollRect;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        m_isHorizontalDrag = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);

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
