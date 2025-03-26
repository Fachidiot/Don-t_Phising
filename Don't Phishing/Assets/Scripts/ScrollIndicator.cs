using UnityEngine.UI;
using UnityEngine;

public class ScrollIndicator : MonoBehaviour
{
    [SerializeField]
    private Image[] m_Indicators;

    public void ChangeIndicator(int currentItem)
    {
        ResetIndicator();
        m_Indicators[currentItem].color = new Color(1, 1, 1, 1);
    }

    private void ResetIndicator()
    {
        foreach (var image in m_Indicators)
        {
            image.color = new Color(1, 1, 1, 0.3f);
        }
    }
}
