using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    [SerializeField]
    private Image m_Image;
    [SerializeField]
    private Sprite m_Source;
    [Header("Title")]
    [SerializeField]
    private string m_Title;
    [SerializeField]
    private TMP_Text m_TitleText;
    [Header("Stack")]
    [SerializeField]
    private GameObject m_Stack;
    [SerializeField]
    private TMP_Text m_StackText;

    private int m_Count = 0;

    private void Start()
    {
        m_Image.sprite = m_Source;
        m_TitleText.text = m_Title;

        if (m_Count != 0)
            OnStack(m_Count);
        else
            EraseStack();
    }

    public void OnStack(int count)
    {
        m_Stack.SetActive(true);
        m_Count = count;
        m_StackText.text = count.ToString();
    }

    public void EraseStack()
    {
        m_Stack.SetActive(false);
        m_StackText.text = 1.ToString();
    }
}
