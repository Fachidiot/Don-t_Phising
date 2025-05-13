using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneController : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;

    private bool m_enable = false;
    private bool m_disable = false;

    private string m_animIDEnable = "Enable";
    private string m_animIDDisable = "Disable";

    private void Update()
    {
        if (m_disable)
        {
            if (CheckEndAnimation(m_animIDDisable))
            {
                m_disable = false;
                gameObject.SetActive(false);
            }
        }
        if (m_enable)
        {
            if (CheckEndAnimation(m_animIDEnable))
                m_enable = false;
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        m_animator.SetBool(m_animIDEnable, true);
        m_enable = true;
        m_disable = false;
    }
    public void Disable()
    {
        m_animator.SetBool(m_animIDEnable, false);
        m_disable = true;
        m_enable = false;
    }

    private bool CheckEndAnimation(string name)
    {
        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(name) == true)
        {
            float _animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (_animTime == 0)
                return false;
            if (_animTime > 0 && _animTime < 1.0f)
                return false;
            else if (_animTime >= 1.0f)
            {
                return true;
            }
        }
        return false;
    }
}
