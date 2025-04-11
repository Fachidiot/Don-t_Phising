using TMPro;
using UnityEngine;

public class DropdownSizeFilter : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown m_Dropdown;
    [SerializeField]
    private Vector2 m_Size;

    void Start()
    {
        // Template의 크기를 직접 설정
        RectTransform template = m_Dropdown.template;
        template.sizeDelta = m_Size;

        // 스크롤 뷰의 크기도 조정하려면
        RectTransform scrollRect = template.Find("Viewport").GetComponent<RectTransform>();
        if (scrollRect != null)
        {
            scrollRect.sizeDelta = new Vector2(scrollRect.sizeDelta.x, m_Size.y - 20); // 여백 고려
        }
    }
}
