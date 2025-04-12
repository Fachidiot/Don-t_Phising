using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_TaskBar;
    [SerializeField]
    private Transform m_TaskParent;
    [SerializeField]
    private GameObject m_TaskLayout;

    private List<string> m_Tasks;

    private void Awake()
    {
        m_Tasks = new List<string>();
    }

    private void Update()
    {
        if (m_Tasks.Count == 0)
            m_TaskBar.SetActive(false);
    }

    public void RunningApp()
    {
        m_TaskBar.SetActive(false);
    }

    public void AddTask(string name)
    {
        if (!CheckValidate(name))
            return;
        m_Tasks.Add(name);

        InstantiateTask(name);
    }

    private bool CheckValidate(string name)
    {
        if (m_Tasks.Count <= 0)
            return true;
        foreach (var task in m_Tasks)
        {
            if (task == name)
                return false;
        }
        return true;
    }

    public void Remove(string name)
    {
        m_Tasks.Remove(name);
    }

    private void InstantiateTask(string name)
    {
        GameObject go = Instantiate(m_TaskLayout, m_TaskParent);
        go.GetComponent<Task_Layout>().SetTaskjLayout(name, this, m_TaskBar.GetComponent<ScrollRect>());
    }
}
