using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class CalculateManager : BaseAppManager
{
    [SerializeField]    // Title Text
    private TMP_Text m_Title;
    [SerializeField]    // Title Language
    private LText m_LTitle;
    [SerializeField]    // 입력값 Text
    private TMP_Text m_tmpInput;
    [SerializeField]    // 기록값 Text
    private TMP_Text m_tmpHistory;

    private List<OPR> m_oprs;   // 연산자 리스트
    private List<double> m_nums;// 피연산자 리스트
    private string m_last;      // 피연산자 입력

    private void Start()
    {
        m_nums = new List<double>();
        m_oprs = new List<OPR>();
        Clear();
    }

    /// <summary>
    /// TODO: 지우기 기능 만들기.
    /// 1. m_last의 value확인 (Empty or not)
    /// 2. 비어있지 않다면 last값 지우기
    /// 3. else 비어져 있다면 m_oprs 마지막값 지우기 + m_last에 m_nums의 마지막값 할당.
    /// 4. 반복하다가 m_nums랑 m_oprs가 모두 비워지면 return;
    /// </summary>

    #region Button Funcs
    // 수 입력 함수
    public void Input(string num)
    {
        m_last += num;

        // Update TMP_text
        InputTextUpdate(num);
    }

    // 연산자 입력 함수 -> 수 입력이 끝남.
    [VisibleEnum(typeof(OPR))]
    public void EndInput(int _opr)
    {
        if (m_last == string.Empty)
        {   // 수가 입력이 안되어 있을 때
            if (m_oprs.Count > 0)
            {
                if (m_oprs[m_oprs.Count - 1] == OPR.MODULAR && (OPR)_opr == OPR.MULTIPLY)
                {   // 연산자가 %x 일 때
                    m_tmpInput.text += "x";
                    int lastindex = m_oprs.Count - 1;
                    m_nums[lastindex] = m_nums[lastindex] * 0.01;
                    m_oprs[lastindex] = OPR.MULTIPLY;
                    return;
                }
            }
            return;
        }
        else if (m_oprs.Count == 0 && m_tmpInput.text != "0")
        {   // 
            m_nums.Add(double.Parse(m_tmpInput.text));
        }
        else
            m_nums.Add(double.Parse(m_last));
        m_last = string.Empty;

        m_tmpInput.text += OPRToString((OPR)_opr);
        m_oprs.Add((OPR)_opr);
    }

    // 계산 결과 실행 함수.
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
        m_tmpInput.text = FormattingNumber(result.ToString());
        Clear();
        m_last = m_tmpInput.text;
    }

    // 지우기 함수
    public void Delete()
    {
        if ((m_nums.Count == 0 && m_oprs.Count == 0) && m_last == string.Empty)
            return;
        if (m_last != string.Empty)
        {
            m_last = m_last.Substring(0, m_last.Length - 1);
            InputTextUpdate(m_last);
            if (m_last.Length == 0)
                m_last = string.Empty;
        }
        else
        {
            m_oprs.RemoveAt(m_oprs.Count - 1);
            int lastIndex = m_nums.Count - 1;
            m_last = m_nums[lastIndex].ToString();
            m_nums.RemoveAt(lastIndex);
            InputTextUpdate(m_last);
        }
    }

    // 음수 양수 전환 함수
    public void Reverse()
    {
        return;
        if (m_last.StartsWith('('))
        {   // 음수
            m_last = m_last.Substring(2, m_last.Length - 1);
            InputTextUpdate(m_last);
        } else
        {   // 양수
            m_last = "(-" + m_last + ")";
            InputTextUpdate(m_last);
        }
    }

    // AC함수
    public void AllClear()
    {
        m_tmpInput.text = "0";
        m_tmpHistory.text = "";
        Clear();
    }
    #endregion
    #region Local Funcs
    // 초기값으로 Clear함수
    private void Clear()
    {
        m_nums.Clear();
        m_oprs.Clear();
        m_last = string.Empty;
    }
    // 연산자 함수
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
    // 연산자 -> String
    private string OPRToString(OPR opr)
    {
        return (opr == OPR.MODULAR ? "%" : opr == OPR.MULTIPLY ? "x" : opr == OPR.DIVIDE ? "/" : opr == OPR.PLUS ? "+" : "-");
    }
    // Input TMP_Text Refresh
    private void InputTextUpdate(string num)
    {
        if (m_tmpInput.text == "0")
            m_tmpInput.text = num;
        else
        {
            m_last = FormattingNumber(m_last);
            m_tmpInput.text = FormattingNumbers();
            m_tmpInput.text += m_last;
        }
    }
    // 000,000.000 포멧 변환 함수
    private string FormattingNumber(string num)
    {
        if (num.LastIndexOf('.') == num.Length - 1)
            return num;
        else
            return string.Format("{0:#,#0.##########}", double.Parse(num)).ToString();
    }
    // 000,000.000 + 연산자 포멧 함#
    private string FormattingNumbers()
    {
        string format = "";
        for (int i = 0; i < m_nums.Count; i++)
        {
            format += FormattingNumber(m_nums[i].ToString());
            if (m_oprs.Count > i)
            {
                format += OPRToString(m_oprs[i]);
            }
        }
        return format;
    }
    #endregion

    public override void SetText()
    {
        m_Title.text = m_LTitle.GetText(OSManager.Instance.GetLanguage());
    }

    public override void ResetApp()
    {
        AllClear();
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