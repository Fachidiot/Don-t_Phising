using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    private static AppManager m_Instance;
    public static AppManager Instance { get { return m_Instance; } }

    [SerializeField]
    private GameObject[] m_Views;

    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(this);
            return;
        }
        m_Instance = this;
    }

    private void Start()
    {
        ResetView();
    }

    public void ResetView()
    {
        foreach (var view in m_Views)
        {
            view.gameObject.SetActive(false);
        }
    }
}
