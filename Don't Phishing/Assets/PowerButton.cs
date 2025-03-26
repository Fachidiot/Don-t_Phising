using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerButton : MonoBehaviour
{
    public OSManager m_GameManager;
    private BoxCollider m_Collider;

    private void Start()
    {
        m_Collider = GetComponent<BoxCollider>();
    }

    private void OnCollisionStay(Collision collision)
    {
    }
}
