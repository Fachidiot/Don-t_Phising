using Ink.Parsed;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CalculateManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_tmpInput;
    [SerializeField]
    private TMP_Text m_tmpHistory;

    // -------n e w--------
    private List<OPR> m_oprs;
    private List<double> m_nums;
    [SerializeField]
    private string m_last;

    public void Start()
    {
        m_nums = new List<double>();
        m_oprs = new List<OPR>();
        Clear();
    }

    public void Input(string num)
    {
        m_last += num;
        if (m_tmpInput.text == "0")
            m_tmpInput.text = num;
        else
        {
            m_tmpInput.text += num;
        }
    }

    [VisibleEnum(typeof(OPR))]
    public void EndInput(int _opr)
    {
        if (m_last == string.Empty)
        {
            if (m_oprs.Count > 0)
            {
                if (m_oprs[m_oprs.Count - 1] == OPR.MODULAR && (OPR)_opr == OPR.MULTIPLY)
                {   // %x 일때만.
                    Debug.Log("%x");
                    m_tmpInput.text += "x";
                    int lastindex = m_oprs.Count - 1;
                    m_nums[lastindex] = m_nums[lastindex] * 0.01;
                    m_oprs[lastindex] = OPR.MULTIPLY;
                    return;
                }
            }
            return;
        }
        else if (m_oprs.Count == 0)
        {
            m_nums.Add(double.Parse(m_tmpInput.text));
        }
        else
            m_nums.Add(double.Parse(m_last));
        m_last = string.Empty;

        OPR opr = (OPR)_opr;
        m_tmpInput.text += (
            opr == OPR.MODULAR ? "%" :
            opr == OPR.MULTIPLY ? "x" :
            opr == OPR.DIVIDE ? "/" :
            opr == OPR.PLUS ? "+" : "-");
        m_oprs.Add(opr);
    }

    public void Equal()
    {
        if (m_last != string.Empty)
        {
            m_nums.Add(double.Parse(m_last));
            m_last = string.Empty;
        }
        else
        {
            int lastindex = m_oprs.Count - 1;
            if (m_oprs[lastindex] == OPR.MODULAR)
            {
                m_oprs.RemoveAt(lastindex);
                if (lastindex != 0)
                {
                    lastindex -= 1;
                }
                m_nums[lastindex] = m_nums[lastindex] * 0.01;
            }
        }
        double result = 0;

        if (m_oprs.Count == 0)
            result = m_nums[0];
        while (m_oprs.Count > 0)
        {
            int index = 0;
            if (m_oprs.Contains(OPR.MULTIPLY))
                index = m_oprs.IndexOf(OPR.MULTIPLY);
            else if (m_oprs.Contains(OPR.DIVIDE))
                index = m_oprs.IndexOf(OPR.DIVIDE);
            else if (m_oprs.Contains(OPR.MODULAR))
                index = m_oprs.IndexOf(OPR.MODULAR);

            result = Calculate(m_nums[index], m_nums[index + 1], m_oprs[index]);

            m_nums.RemoveAt(index);
            m_nums[index] = result;
            m_oprs.RemoveAt(index);
        }

        m_tmpHistory.text = m_tmpInput.text;
        m_tmpInput.text = string.Format("{0:#,#0.##########}", result).ToString();
        Clear();
        m_last = m_tmpInput.text;
    }

    public void Reverse()
    {
        if (m_tmpInput.text[0] != '-')
            m_tmpInput.text.Insert(0, "-");
        else
            m_tmpInput.text.Remove(0);
    }

    public void AllClear()
    {
        m_tmpInput.text = "0";
        m_tmpHistory.text = "";
        Clear();
    }

    private void Clear()
    {
        m_nums.Clear();
        m_oprs.Clear();
        m_last = string.Empty;
    }

    private double Calculate(double n1, double n2, OPR opr)
    {
        switch (opr)
        {
            case OPR.PLUS:
                return n1 + n2;
            case OPR.MINUS:
                return n1 - n2;
            case OPR.MULTIPLY:
                return n1 * n2;
            case OPR.DIVIDE:
                return n1 / n2;
            case OPR.MODULAR:
                return n1 % n2;
        }
        return 0;
    }
}

public enum OPR
{
    NONE,
    PLUS,
    MINUS,
    MULTIPLY,
    DIVIDE,
    MODULAR
}