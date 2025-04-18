using System.Collections;
using System.IO;
using UnityEngine;

public class NavigationAction : MonoBehaviour
{
    [SerializeField]
    private GameObject m_TaskBar;
    [SerializeField]
    private Camera m_BackgroundCaptureCamera;
    [SerializeField]
    private RenderTexture m_Texture;

    private float m_ClickTime;
    private float m_MinClickTime = 0.7f;
    private bool m_IsClicked;
    private bool m_TaskDone = true;
    private string m_AppName;

    private void Update()
    {
        if (m_IsClicked)
            m_ClickTime += Time.deltaTime;
        else
            m_ClickTime = 0f;
    }

    public void ButtonDown()
    {
        m_IsClicked = true;
    }

    public void ButtonUp()
    {
        m_IsClicked = false;
        if (m_ClickTime >= m_MinClickTime)
        {
            m_TaskBar.SetActive(true);
        }
    }

    public void EndApp()
    {
        if (!m_TaskDone)    // Screen Capture Coroutine중엔 return.
            return;
        if (m_ClickTime < m_MinClickTime)   // 홈화면 이동
        {
            m_AppName = AppManager.Instance.GetCurrentApp();
            if (m_AppName == string.Empty)
            {
                ResetApps();
                return;
            }
            StartCoroutine(ScreenCapture());
            m_TaskBar.transform.parent.gameObject.GetComponent<TaskManager>().AddTask(m_AppName);
        }
    }

    private void ResetApps()
    {
        AppManager.Instance.ResetApps();
        if (!OSManager.Instance.HomeScreen.activeSelf)
            OSManager.Instance.HomeScreen.SetActive(true);
        OSManager.Instance.MainScreen.gameObject.SetActive(true);
        m_TaskDone = true;
    }

    private IEnumerator ScreenCapture()
    {
        m_TaskDone = false;
        yield return new WaitForEndOfFrame();

        RenderTexture.active = m_Texture;
        var texture2D = new Texture2D(m_Texture.width, m_Texture.height);
        texture2D.ReadPixels(new Rect(0, 0, m_Texture.width, m_Texture.height), 0, 0);
        texture2D.Apply();

        byte[] byteArray = texture2D.EncodeToPNG();
        string savePath = Application.dataPath + "/Resources/Background/" + m_AppName + ".png";
        File.WriteAllBytes(savePath, byteArray);

        Debug.LogFormat("Capture Complete! Location : {0}", savePath);

        if (Application.isPlaying)
            Destroy(texture2D);

        m_AppName = string.Empty;

        //End App
        ResetApps();
    }
}
