using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CallManager : MonoBehaviour
{
    private static CallManager m_Instance;
    public static CallManager Instance { get { return m_Instance; } }

    [SerializeField]
    private GameObject[] m_Views;
    [Header("Favorites")]
    [Header("Recents")]
    [Header("Contacts")]
    [Header("Keypad")]
    [SerializeField]
    private TMP_Text m_Number;
    [Header("Voicemail")]
    [SerializeField]
    private GameObject test;

    private string m_SNumber = "";
    private Dictionary<string, string> m_Contacts;

    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(this);
            return;
        }
        m_Instance = this;
    }

    #region Keypad
    public void SetNumber(TMP_Text text)
    {
        switch (text.text)
        {
            case "X":
                m_SNumber = m_SNumber.Substring(0, m_SNumber.Length - 1);
                break;
            default:
                m_SNumber += text.text;
                break;
        }
        m_Number.text = m_SNumber;
    }

    public void CallButton()
    {

    }
    #endregion

    public void ResetView()
    {
        foreach (var view in m_Views)
        {
            view.gameObject.SetActive(false);
        }
    }
}
