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

    private string m_AppName;
    Texture2D m_texture2D;

    public void EndApp()
    {
        RenderTexture.active = m_Texture;
        m_texture2D = new Texture2D(m_Texture.width, m_Texture.height);
        m_texture2D.ReadPixels(new Rect(0, 0, m_Texture.width, m_Texture.height), 0, 0);
        m_texture2D.Apply();

        m_AppName = AppManager.Instance.GetCurrentApp();
        if (m_AppName == string.Empty)
        {
            ResetApps();
            return;
        }
        StartCoroutine(ScreenCapture());
        m_TaskBar.transform.parent.gameObject.GetComponent<TaskManager>().AddTask(m_AppName);
        AppManager.Instance.ResetApps();
        OSManager.Instance.EndApp();
    }

    private void ResetApps()
    {
        AppManager.Instance.ResetApps();
    }

    private IEnumerator ScreenCapture()
    {
        yield return new WaitForEndOfFrame();

        byte[] byteArray = m_texture2D.EncodeToPNG();
        string savePath = Application.dataPath + "/Resources/Background/" + m_AppName + ".png";
        File.WriteAllBytes(savePath, byteArray);

        Debug.LogFormat("Capture Complete! Location : {0}", savePath);

        m_BackgroundCaptureCamera.gameObject.SetActive(false);

        if (Application.isPlaying)
            Destroy(m_texture2D);

        m_AppName = string.Empty;

        //End App
        ResetApps();
    }
}
