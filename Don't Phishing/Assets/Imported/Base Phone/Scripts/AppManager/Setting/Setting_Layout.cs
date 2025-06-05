using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Setting_Layout : MonoBehaviour
{
    [SerializeField] private TMP_Text m_TMPText;
    [SerializeField] private LText m_Text;

    [SerializeField] private List<string> m_settingList;
    public List<string> Name { get { return m_settingList; } set { m_settingList = value; } }
    private List<bool> m_valueList;
    public List<bool> Value { get { return m_valueList; } set { m_valueList = value; } }

    private void Start()
    {
        m_valueList = new List<bool>();
        for (int i = 0; i < m_settingList.Count; i++)
            m_valueList.Add(false);
    }

    public void SetText()
    {
        m_TMPText.text = m_Text.GetText(OSManager.Instance.GetLanguage());
    }
}
