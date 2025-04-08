using Ink.UnityIntegration;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Profile
{
    private string m_Name;
    private string m_Image;

    public Profile(string name, string image)
    {
        m_Name = name;
        m_Image = image;
    }

    public string GetName()
    {
        return m_Name;
    }

    public Sprite GetImage()
    {
        return Resources.Load<Sprite>(m_Image);
    }
}

public class Profile_Layout : MonoBehaviour
{
    [SerializeField]
    private Image m_Image;
    [SerializeField]
    private TMP_Text m_TMPName;

    public void SetProfile()
    {
        Profile profile = OSManager.Instance.GetProfile();
        if (profile != null)
        {
            m_TMPName.text = profile.GetName();
            m_Image.sprite = profile.GetImage();
        }
    }
}
