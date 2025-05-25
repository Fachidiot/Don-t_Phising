using UnityEngine;
using UnityEngine.EventSystems;

public class TaskAction : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private GameObject m_TaskBar;

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.delta.y > 0)
        {
            m_TaskBar.SetActive(true);
        }
    }
}
