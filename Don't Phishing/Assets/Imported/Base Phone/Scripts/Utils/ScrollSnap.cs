using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSnap : MonoBehaviour
{
    [Header("SnapSetting")]
    [SerializeField]
    private ScrollRect m_ScrollRect;
    [SerializeField]
    private RectTransform m_ContentPanel;
    [SerializeField]
    private RectTransform m_SampleListItme;
    [SerializeField]
    private float m_SnapForce;
    [SerializeField]
    private int m_EndItem;
    [SerializeField]
    private bool m_Popup = false;
    [SerializeField]
    private bool m_Swaping = false;
    [SerializeField]
    private bool m_NonRayCast = false;
    public bool NonRayCast { set {  m_NonRayCast = value; } }
    [Header("Horizontal")]
    [SerializeField]
    private HorizontalLayoutGroup m_HorizontalLayoutGroup;
    [SerializeField]
    private ScrollIndicator m_Indicator;
    [Header("Vertical")]
    [SerializeField]
    private VerticalLayoutGroup m_VerticalLayoutGroup;
    [SerializeField]
    private BackgroundManager m_BackgroundManager;

    [Header("ItemSetting")]
    [SerializeField]
    private TMP_Text m_NameLabel;
    [SerializeField]
    private string[] m_ItemNames;
    [SerializeField]
    private bool m_Debug;

    [SerializeField]
    private bool m_IsSnapped;
    private float m_SnapSpeed;
    [SerializeField]
    private int m_CurrentItem;
    public int CurrentItem { get { return m_CurrentItem; } }
    public bool End = false;

    void Start()
    {
        m_IsSnapped = false;
    }

    void Update()
    {
        if (m_Popup && m_IsSnapped)
        {
            if (m_CurrentItem == m_EndItem && m_VerticalLayoutGroup != null)
            {
                m_ContentPanel.localPosition = new Vector3(m_ContentPanel.localPosition.x, m_SampleListItme.rect.height + m_VerticalLayoutGroup.spacing, m_ContentPanel.localPosition.z);
                gameObject.SetActive(false);
            }
            else if (m_CurrentItem == m_EndItem && m_HorizontalLayoutGroup != null)
            {
                m_ContentPanel.localPosition = new Vector3(m_SampleListItme.rect.width + m_HorizontalLayoutGroup.spacing, m_ContentPanel.localPosition.y, m_ContentPanel.localPosition.z);
                gameObject.SetActive(false);
            }
        }

        else if (m_Swaping && m_IsSnapped)
        {
            if (m_CurrentItem == m_EndItem && m_VerticalLayoutGroup != null)
            {
                End = true;
            }
            else if (m_CurrentItem == m_EndItem && m_HorizontalLayoutGroup != null)
            {
                End = true;
            }
        }

        if (m_HorizontalLayoutGroup != null)
            Horizontal();
        if (m_VerticalLayoutGroup != null)
            Vertical();
    }

    public void SetContentPosition(int item)
    {
        if (m_Debug)
            Debug.Log(item);
        m_CurrentItem = item;
        m_IsSnapped = false;
        if (m_HorizontalLayoutGroup != null)
            Horizontal(m_CurrentItem);
        if (m_VerticalLayoutGroup != null)
            Vertical(m_CurrentItem);
    }

    public int GetContentPosition()
    {
        return m_CurrentItem;
    }

    private void Horizontal(int item = -1)
    {
        if (!m_NonRayCast)
        {
            if (item == -1)
                m_CurrentItem = Mathf.RoundToInt(0 - m_ContentPanel.localPosition.x / (m_SampleListItme.rect.width + m_HorizontalLayoutGroup.spacing));
            else
                m_CurrentItem = item;
        }

        if (m_ScrollRect.velocity.magnitude < 200 && !m_IsSnapped)
        {
            m_ScrollRect.velocity = Vector2.zero;
            m_SnapSpeed += m_SnapForce * Time.deltaTime;
            m_ContentPanel.localPosition = new Vector3(
                Mathf.MoveTowards(m_ContentPanel.localPosition.x, 0 - (m_CurrentItem * (m_SampleListItme.rect.width + m_HorizontalLayoutGroup.spacing)), m_SnapSpeed),
                m_ContentPanel.localPosition.y,
                m_ContentPanel.localPosition.z);
            SetItemName(m_CurrentItem);
            if (m_Indicator != null)
                m_Indicator.ChangeIndicator(m_CurrentItem);
            if (m_ContentPanel.localPosition.x == 0 - (m_CurrentItem * (m_SampleListItme.rect.width + m_HorizontalLayoutGroup.spacing)))
            {
                m_SnapSpeed = 0;
                m_IsSnapped = true;
            }
        }
        if (m_ScrollRect.velocity.magnitude > 200)
        {
            SetItemName("_________");
            m_IsSnapped = false;
            m_SnapSpeed = 0;
        }
    }

    private void Vertical(int item = 0)
    {
        if (!m_NonRayCast)
        {
            if (item == 0)
                m_CurrentItem = Mathf.RoundToInt(0 - m_ContentPanel.localPosition.y / (m_SampleListItme.rect.height + m_VerticalLayoutGroup.spacing));
            else
                m_CurrentItem = item;
        }

        if (m_ScrollRect.velocity.magnitude < 200 && !m_IsSnapped)
        {
            m_ScrollRect.velocity = Vector2.zero;
            m_SnapSpeed += m_SnapForce * Time.deltaTime;
            m_ContentPanel.localPosition = new Vector3(
                m_ContentPanel.localPosition.x,
                Mathf.MoveTowards(m_ContentPanel.localPosition.y, 0 - (m_CurrentItem * (m_SampleListItme.rect.height + m_VerticalLayoutGroup.spacing)), m_SnapSpeed),
                m_ContentPanel.localPosition.z);

            SetItemName(m_CurrentItem);
            if (m_Indicator != null)
                m_Indicator.ChangeIndicator(m_CurrentItem);

            //if (m_BackgroundManager != null)
            //    m_BackgroundManager.ChangeAlpha(0 - m_ContentPanel.localPosition.y / (m_SampleListItme.rect.height + m_VerticalLayoutGroup.spacing));

            if (m_ContentPanel.localPosition.y == 0 - (m_CurrentItem * (m_SampleListItme.rect.height + m_VerticalLayoutGroup.spacing)))
            {
                m_SnapSpeed = 0;
                m_IsSnapped = true;
            }
        }
        if (m_ScrollRect.velocity.magnitude > 200)
        {
            SetItemName("_________");
            m_IsSnapped = false;
            m_SnapSpeed = 0;
        }
    }

    private void SetItemName(string name)
    {
        if (m_NameLabel != null)
        m_NameLabel.text = name;
    }

    private void SetItemName(int index)
    {
        if (m_ItemNames.Length != 0)
            m_NameLabel.text = m_ItemNames[index];
    }
}
