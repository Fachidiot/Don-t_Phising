using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Setting_Layout : MonoBehaviour
{
    [Header("LText")]
    [SerializeField] private TMP_Text m_TMPText;
    [SerializeField] private LText m_Text;

    [Header("Settings")]
    [SerializeField] private GameObject m_ViewPanel;

    private void Start()
    {
        if (null != m_ViewPanel)    // Toggle is not need to ViewPanel Event.
            GetComponentInChildren<Button>().onClick.AddListener(() => m_ViewPanel.SetActive(true));
    }

    public void SetText()
    {
        m_TMPText.text = m_Text.GetText(OSManager.Instance.GetLanguage());
    }
}
