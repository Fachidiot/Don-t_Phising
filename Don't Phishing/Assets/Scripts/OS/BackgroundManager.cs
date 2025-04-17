using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> m_Backgrounds;
    [SerializeField]
    private int m_BackgroundIndex = 0;
    public int Index { get { return m_BackgroundIndex; } }
    [SerializeField]
    private RectTransform m_ContentPanel;
    [SerializeField]
    private Sprite[] m_CustomBackgrounds = new Sprite[8];

    private Image m_Image;
    private int m_CustomIndex = 0;
    private float m_PrevAlpha;

    void Start()
    {
        m_Image = GetComponent<Image>();
    }

    private void Update()
    {
        if (m_ContentPanel != null)
        {
            float alpha = 0 - m_ContentPanel.localPosition.y / m_ContentPanel.rect.height;
            if (alpha != m_PrevAlpha)
            {
                ChangeAlpha(alpha);
                m_PrevAlpha = alpha;
            }
        }
    }

    public void AddBackground(Sprite sprite)
    {
        if (m_CustomIndex >= 8)
            m_CustomIndex = -1;
        m_CustomBackgrounds[m_CustomIndex++] = sprite;
    }

    public void UpdateBackground(int m_index)
    {
        m_BackgroundIndex = m_index;

        if (m_Backgrounds.Count > m_BackgroundIndex)
            m_Image.sprite = m_Backgrounds[m_BackgroundIndex];  
    }

    public void ChangeAlpha(float alpha)
    {
        Color color = new Color(1, 1, 1, alpha);
        m_Image.color = color;
    }
}
