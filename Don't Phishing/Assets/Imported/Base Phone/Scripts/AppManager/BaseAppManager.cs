using UnityEngine;

public abstract class BaseAppManager : MonoBehaviour
{
    [SerializeField]
    private string m_AppName;

    public string GetName()
    {
        return m_AppName;
    }

    public void InitApp()
    {
        if (m_AppName == string.Empty)
            m_AppName = gameObject.name;
    }

    public abstract void SetText();

    public abstract void ResetApp();
}
