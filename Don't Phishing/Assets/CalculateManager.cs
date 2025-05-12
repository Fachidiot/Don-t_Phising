using TMPro;
using UnityEngine;

enum Calc
{
    None,
    Plus,
    Minus,
    Multi,
    Subdiv,
    Percent
}

public class CalculateManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_tmpInput;
    [SerializeField]
    private TMP_Text m_tmpHistory;

    [SerializeField]
    private double m_result;
    [SerializeField]
    private string m_input;
    private Calc m_calc = Calc.None;

    public void Start()
    {
        Clear();
    }

    public void Multiply()
    {
        if (m_input != string.Empty)
        {
            if (m_result == 0)
                m_result = CalcLastNum();
            else
                m_result *= CalcLastNum();
        }
        m_tmpInput.text += "x";
        m_calc = Calc.Multi;
    }

    public void Subdivide()
    {
        if (m_input != string.Empty)
        {
            if (m_result == 0)
                m_result = CalcLastNum();
            else
                m_result /= CalcLastNum();
        }
        m_tmpInput.text += "/";
        m_calc = Calc.Subdiv;
    }

    public void Plus()
    {
        if (m_input != string.Empty)
        {
            if (m_result == 0)
                m_result = CalcLastNum();
            else
                m_result += CalcLastNum();
        }
        m_tmpInput.text += "+";
        m_calc = Calc.Plus;
    }

    public void Minus()
    {
        if (m_input != string.Empty)
        {
            if (m_result == 0)
                m_result = CalcLastNum();
            else
                m_result -= CalcLastNum();
        }
        m_tmpInput.text += "-";
        m_calc = Calc.Minus;
    }

    public void Percent()
    {
        if (m_input != string.Empty)
        {
            if (m_result == 0)
                m_result = CalcLastNum();
            else
                m_result %= CalcLastNum();
        }
        m_tmpInput.text += "%";
        m_calc = Calc.Percent;
    }

    public void Result()
    {
        switch (m_calc)
        {
            case Calc.Plus:
                m_result += CalcLastNum();
                break;
            case Calc.Minus:
                m_result -= CalcLastNum();
                break;
            case Calc.Multi:
                m_result *= CalcLastNum();
                break;
            case Calc.Subdiv:
                m_result /= CalcLastNum();
                break;
            case Calc.Percent:
                m_result %= CalcLastNum();
                break;
        }
        m_tmpHistory.text = m_tmpInput.text;
        m_tmpInput.text = m_result.ToString();
    }

    public void AllClear()
    {
        m_tmpInput.text = "0";
        m_tmpHistory.text = "";
        Clear();
    }

    private void Clear()
    {
        m_result = 0;
        m_input = string.Empty;
    }

    public void InputNumber(float number)
    {
        if (m_tmpInput.text == "0")
        {
            m_tmpInput.text = number.ToString();
        }
        else
            m_tmpInput.text += number;

        m_input += number;
    }

    private double CalcLastNum()
    {
        double last = 0;
        int length = m_input.Length;
        for (int i = 0; i < length; i++)
        {
            last += double.Parse(m_input[i].ToString()) * (Mathf.Pow(10, length - (i + 1)));
        }
        m_input = string.Empty;

        return last;
    }
}
