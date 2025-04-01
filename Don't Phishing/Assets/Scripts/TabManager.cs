using UnityEngine;

public class TabManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_Views;
    [SerializeField]
    private Tab[] m_Tabs;
    [SerializeField]
    private LText[] m_TabsText;
    [SerializeField]
    private int m_InitIndex;

    private void Start()
    {
        SetText();
        ResetTab();
        m_Views[m_InitIndex].SetActive(true);
    }

    public void SetText()
    {
        Language currentLanguage = OSManager.Instance.GetLanguage();
        for (int i = 0; i < m_Tabs.Length; i++)
        {
            var text = m_TabsText[i].GetText(currentLanguage);
            m_Tabs[i].SetText(text);
            m_Views[i].GetComponent<View>().SetText(text);
        }
    }

    public void ResetTab()
    {
        foreach (var view in m_Views)
        {
            if (view.activeSelf)
                view.SetActive(false);
        }
    }
}
