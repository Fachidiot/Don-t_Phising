using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClick_Debuger : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private bool m_debug;

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!m_debug)
            return;
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        Debug.Log(clickedObject);
    }
}