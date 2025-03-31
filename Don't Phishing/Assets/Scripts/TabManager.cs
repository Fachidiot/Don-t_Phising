using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_Views;
    [SerializeField]
    private int m_InitIndex;

    private void Start()
    {
        m_Views[m_InitIndex].SetActive(true);
    }

    public void ResetView()
    {
        foreach (var view in m_Views)
        {
            if (view.activeSelf)
                view.gameObject.SetActive(false);
        }
    }
}
