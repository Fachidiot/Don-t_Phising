using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

// �±׸� �Ľ��ϰ� ó�� (WAIT, FLAG, ANIM ��)
public class TagHandler
{
    // �÷��� ���� ����
    private readonly Dictionary<string, bool> flags;

    // �ִϸ����� ����
    private readonly Animator animator;   // <- ��� ... �ִϸ��̼� ���� �� 

    // �ڷ�ƾ �����
    private readonly CoroutineRunner coroutineRunner;

    // ������: Animator�� CoroutineRunner �ʱ�ȭ
    public TagHandler(Animator animator = null, CoroutineRunner coroutineRunner = null)
    {
        flags = new Dictionary<string, bool>();
        this.animator = animator;
        this.coroutineRunner = coroutineRunner ?? CoroutineRunner.Instance;
    }

    // �±� ���ڿ� �Ľ� �� ó��, �Ϸ� �� �ݹ� ȣ��
    public void ProcessTags(string tagString, Action onComplete)
    {
        if (string.IsNullOrEmpty(tagString))
        {
            onComplete?.Invoke();
            return;
        }

        var tags = tagString.Split(',');

        foreach (var tag in tags)
        {
            var parts = tag.Trim().Split(':');
            if (parts.Length == 0 || string.IsNullOrEmpty(parts[0]))
            {
                Debug.LogWarning($"�߸��� �±�: {tag}");
                continue;
            }

            string tagType = parts[0].Trim();
            string tagValue = parts.Length > 1 ? parts[1].Trim() : "";

            switch (tagType)
            {
                case "WAIT":
                    if (!float.TryParse(tagValue, out float waitTime) || waitTime <= 0)
                    {
                        Debug.LogWarning($"�߸��� WAIT ��: {tagValue}");
                        waitTime = 1f;
                    }
                    coroutineRunner.StartCoroutine(WaitCoroutine(waitTime, onComplete));
                    return;

                case "FLAG":
                    if (!string.IsNullOrEmpty(tagValue))
                        SetFlag(tagValue, true);
                    else
                        Debug.LogWarning("FLAG �� ����");
                    break;

                case "CHECK_FLAG":
                    if (!string.IsNullOrEmpty(tagValue) && !CheckFlag(tagValue))
                    {
                        Debug.Log($"CHECK_FLAG ����: {tagValue}");
                        onComplete?.Invoke();
                        return;
                    }
                    break;

                case "ANIM":
                    if (animator != null && !string.IsNullOrEmpty(tagValue))
                        animator.SetTrigger(tagValue);
                    else
                        Debug.LogWarning($"ANIM ����: Animator �Ǵ� �� ���� ({tagValue})");
                    break;

                default:
                    Debug.LogWarning($"�������� �ʴ� �±�: {tagType}");
                    break;
            }
        }

        onComplete?.Invoke();
    }

    // ��� �ð� �� �ݹ� ȣ��
    private IEnumerator WaitCoroutine(float seconds, Action onComplete)
    {
        yield return new WaitForSeconds(seconds);
        onComplete?.Invoke();
    }

    // �÷��� ����
    private void SetFlag(string flagName, bool value)
    {
        flags[flagName] = value;
        Debug.Log($"�÷��� ����: {flagName} = {value}");
    }

    // �÷��� ���� Ȯ��
    private bool CheckFlag(string flagName)
    {
        return flags.TryGetValue(flagName, out bool value) && value;
    }
}

// �ڷ�ƾ ����� �̱���
public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner instance;

    // �̱��� �ν��Ͻ�, ������ ����
    public static CoroutineRunner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
}