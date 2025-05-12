using TMPro;
using UnityEngine;

enum Calc
{
    None,
    Plus,
    Minus,
    Multi,
    Subdiv,
    Modular
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
    private double m_temp;
    private Calc m_prevCalc = Calc.None;
    private Calc m_savedCalc = Calc.None;

    public void Start()
    {
        Clear();
    }

    public void Multiply()
    {
        if (m_input != string.Empty)
        {
            if (m_result == 0)
                m_result = FindLastNum();
            else
            {
                if (m_prevCalc != Calc.None)
                    UpdateResult(Calc.Multi); 
            }
        }
        m_tmpInput.text += "x";
        m_prevCalc = Calc.Multi;
    }

    public void Subdivide()
    {
        if (m_input != string.Empty)
        {
            if (m_result == 0)
                m_result = FindLastNum();
            else
            {
                if (m_prevCalc != Calc.None)
                    UpdateResult(Calc.Subdiv);
            }
        }
        m_tmpInput.text += "/";
        m_prevCalc = Calc.Subdiv;
    }

    public void Plus()
    {
        if (m_input != string.Empty)
        {
            if (m_result == 0)
                m_result = FindLastNum();
            else
            {
                if (m_prevCalc != Calc.None)
                    UpdateResult(Calc.Plus);
            }
        }
        m_tmpInput.text += "+";
        m_prevCalc = Calc.Plus;
    }

    public void Minus()
    {
        if (m_input != string.Empty)
        {
            if (m_result == 0)
                m_result = FindLastNum();
            else
            {
                if (m_prevCalc != Calc.None)
                    UpdateResult(Calc.Minus);
            }
        }
        m_tmpInput.text += "-";
        m_prevCalc = Calc.Minus;
    }

    public void Modular()
    {
        if (m_input != string.Empty)
        {
            if (m_result == 0)
                m_result = FindLastNum();
            else
            {
                if (m_prevCalc != Calc.None)
                    UpdateResult(Calc.Modular);
            }
        }
        m_tmpInput.text += "%";
        m_prevCalc = Calc.Modular;
    }

    public void Result()
    {
        UpdateResult(Calc.None);
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

    private double FindLastNum()
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

    private void UpdateResult(Calc calc)
    {
        if (m_savedCalc != Calc.None)
        {
            m_temp = Calculate(m_temp, FindLastNum(), m_prevCalc);
            m_result = Calculate(m_temp, m_result, m_savedCalc);
            m_savedCalc = Calc.None;
            //m_temp = 0;
            return;
        }

        // 이전의 연산자와 현재 연산자를 비교
        if (m_prevCalc >= calc)
        {   // 전 연산자가 우선순위가 높을 때
            m_result = Calculate(m_result, FindLastNum(), m_prevCalc);
        }
        else
        {   // 현재 연산자가 우선순위가 높을 때
            m_temp = FindLastNum();

            if (calc == Calc.None)
                m_result = Calculate(m_result, m_temp, m_prevCalc);
            else
                m_savedCalc = m_prevCalc;
        }
    }

    private double Calculate(double n1, double n2, Calc calc)
    {
        switch (calc)
        {
            case Calc.Plus:
                return n1 + n2;
            case Calc.Minus:
                return n1 - n2;
            case Calc.Multi:
                return n1 * n2;
            case Calc.Subdiv:
                return n1 / n2;
            case Calc.Modular:
                return n1 % n2;
        }
        return 0;
    }
}
