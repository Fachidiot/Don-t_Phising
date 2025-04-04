using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    [SerializeField]
    private Image m_Image;
    [SerializeField]
    private TMP_Text m_TMPName;

    public void SetProfile(string name)
    {
        m_TMPName.text = name;
        // TODO : 이미지 추가는 나중에 경로를 통해 추가해주자.
    }
}
