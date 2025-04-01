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

    public void EndApp()
    {
        AppManager.Instance.ResetApps();
        OSManager.Instance.OS.gameObject.SetActive(true);
    }
}
