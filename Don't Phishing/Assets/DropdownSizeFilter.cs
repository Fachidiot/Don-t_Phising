using TMPro;
using UnityEngine;

public class DropdownSizeFilter : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown m_Dropdown;
    [SerializeField]
    private Vector2 m_Size;

    void Update()
    {
        if (m_Dropdown.Dropdown != null)
            m_Dropdown.Dropdown.GetComponent<RectTransform>().sizeDelta = m_Size;
    }
}
