using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeScrollImageChanger : MonoBehaviour
{
    [SerializeField] Sprite image00;
    [SerializeField] Sprite image03;
    [SerializeField] Sprite image06;
    [SerializeField] Sprite image10;

    [SerializeField] Color lowColor;
    [SerializeField] Color highColor;

    private float currentVolume;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (currentVolume != OSManager.Instance.GetCurrentVolume())
        {
            currentVolume = OSManager.Instance.GetCurrentVolume();
            SetValue(currentVolume);
        }
    }

    private void SetValue(float _value)
    {
        if (_value > 0.6f)
        {
            if (image10)
            {
                image.sprite = image10;
                image.color = highColor;
            }
        }
        else if (_value > 0.3f)
        {
            if (image06)
            {
                image.sprite = image06;
                image.color = highColor;
            }
        }
        else if (_value > 0)
        {
            if (image03)
            {
                image.sprite = image03;
                image.color = highColor;
            }
        }
        else if (_value <= 0)
        {
            if (image00)
            {
                image.sprite = image00;
                image.color = lowColor;
            }
        }
    }
}
