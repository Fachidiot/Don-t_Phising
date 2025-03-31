using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationAction : MonoBehaviour
{
    [SerializeField]
    private Button m_Button;

    void Start()
    {
        m_Button.onClick.AddListener(EndApp);
    }

    private void EndApp()
    {
        AppManager.Instance.ResetView();
        OSManager.Instance.OS.gameObject.SetActive(true);
        OSManager.Instance.App.gameObject.SetActive(false);
    }
}
