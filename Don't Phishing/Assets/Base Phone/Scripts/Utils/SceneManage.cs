using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private string sceneName;
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
