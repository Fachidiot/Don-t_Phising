using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour
{
    private bool m_Toggled = false;
    public void Toggled()
    {
        m_Toggled = !m_Toggled;
        gameObject.SetActive(m_Toggled);
    }
}
