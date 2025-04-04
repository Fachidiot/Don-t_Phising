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
        if (!OSManager.Instance.HomeScreen.activeSelf)
            OSManager.Instance.HomeScreen.SetActive(true);
        OSManager.Instance.MainScreen.gameObject.SetActive(true);
    }
}
