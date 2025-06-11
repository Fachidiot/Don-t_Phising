using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessScrollImageChanger : MonoBehaviour
{
    [SerializeField] Color lowColor;
    [SerializeField] Color highColor;

    private float currentBrightness;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (currentBrightness != SystemSetting.Instance.GetCurrentBrightness())
        {
            currentBrightness = SystemSetting.Instance.GetCurrentBrightness();
            SetValue(currentBrightness);
        }
    }

    private void SetValue(float _value)
    {
        if (_value > 0.2f)
        {
            image.color = highColor;
        }
        else if (_value <= 0.2f)
        {
            image.color = lowColor;
        }
    }
}
