using UnityEngine;
using UnityEngine.Video;

public class Shorts_Layout : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer m_Player;

    [SerializeField]
    private float m_CurrentVolume = 1;

    private void Update()
    {
        if (m_CurrentVolume != SystemSetting.Instance.GetCurrentVolume())
        {
            m_CurrentVolume = SystemSetting.Instance.GetCurrentVolume();
            m_Player.SetDirectAudioVolume(0, m_CurrentVolume);
        }
    }

    public void Play()
    {
        m_Player.Play();
    }

    public void Pause()
    {
        m_Player.Pause();
    }

    public void Stop()
    {
        m_Player.Stop();
    }
}
