using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CalculateManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_tmpNumber;

    private int m_num1;
    private int m_num2;

    public void Plus()
    {
        m_tmpNumber.text += " + ";
    }

    public void AllClear()
    {
        m_tmpNumber.text = "0";
    }

    public void InputNumber(int number)
    {
        if (m_tmpNumber.text == "0")
            m_tmpNumber.text = number.ToString();
        m_tmpNumber.text += number;
    }
}
