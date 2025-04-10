using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : BaseAppManager
{
    [SerializeField]
    private GameObject m_Camera;
    [SerializeField]
    private GameObject m_Photo;
    [SerializeField]
    private RenderTexture m_Texture;
    [SerializeField]
    private GameObject m_PhotoPrefab;
    [SerializeField]
    private GameObject m_PhotoParent;
    [SerializeField]
    private TMP_Text m_Title;
    [SerializeField]
    private ScrollSnap m_Snap;

    private List<GameObject> m_Photos;
    private List<string> m_Titles;
    private int m_CurIndex = -1;

    private void Start()
    {
        m_Camera.SetActive(true);
        m_Photo.SetActive(false);

        m_Photos = new List<GameObject>();
        m_Titles = new List<string>();
    }

    private void Update()
    {
        if (m_Photo.activeSelf)
        {
            if (m_CurIndex != m_Snap.GetContentPosition())
            {
                m_CurIndex = m_Snap.GetContentPosition();
                if (m_CurIndex != -1)
                    m_Title.text = m_Titles[m_CurIndex];
            }
        }
    }

    public void SaveTexture()
    {
        StartCoroutine(ScreenCapture());
    }

    public void LoadTextures()
    {
        string savePath = Application.dataPath + "/Resources/Photos";
        string[] pngFiles = Directory.GetFiles(savePath, "*.png");

        if (pngFiles.Length != m_Photos.Count)
        {
            DestroyOrigin();
            foreach (string pngFile in pngFiles)
            {
                GameObject go = Instantiate(m_PhotoPrefab, m_PhotoParent.transform);

                byte[] bytes = File.ReadAllBytes(pngFile);
                Texture2D tex = new Texture2D(m_Texture.width, m_Texture.height);
                tex.LoadImage(bytes);

                go.GetComponent<Image>().sprite = Sprite.Create(tex,
                                                                new Rect(0, 0, tex.width, tex.height),
                                                                new Vector2(0.5f, 0.5f),
                                                                1200f);

                var path = pngFile.Replace('\\', '/');
                int lastSplashIndex = path.LastIndexOf('/');
                string name = path.Substring(lastSplashIndex + 1);

                m_Titles.Add(name);
                m_Photos.Add(go);
            }
        }
    }

    public void DeleteTexture()
    {
        DeleteTexture(m_Title.text);
        Destroy(m_Photos[m_CurIndex]);
        m_Photos.RemoveAt(m_CurIndex);
        m_Titles.RemoveAt(m_CurIndex);
    }

    private void DeleteTexture(string filename)
    {
        string path = Application.dataPath + "/Resources/Photos/" + filename;
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.LogFormat("File has been successfully deleted : {0}", path);
        }
        else
            Debug.LogWarningFormat("File didn't exist to delete : {0}", path);
    }

    private void DestroyOrigin()
    {
        foreach (var photo in m_Photos)
        {
            Destroy(photo);
        }
        m_Photos.Clear();
        m_Titles.Clear();
    }

    private IEnumerator ScreenCapture()
    {
        yield return new WaitForEndOfFrame();

        RenderTexture.active = m_Texture;
        var texture2D = new Texture2D(m_Texture.width, m_Texture.height);
        texture2D.ReadPixels(new Rect(0, 0, m_Texture.width, m_Texture.height), 0, 0);
        texture2D.Apply();

        byte[] byteArray = texture2D.EncodeToPNG();
        //string savePath = Application.persistentDataPath + "/Photos.png";
        string savePath = Application.dataPath + "/Resources/Photos/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";
        File.WriteAllBytes(savePath, byteArray);

        Debug.LogFormat("Capture Complete! Location : {0}", savePath);

        if (Application.isPlaying)
            Destroy(texture2D);
    }

    public override void SetText()
    {
        return;
    }

    public override void ResetApp()
    {
        return;
    }
}
