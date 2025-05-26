using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

/// <summary>
/// �±׸� �Ľ��ϰ� ó���ϴ� ��ƿ��Ƽ Ŭ����
/// WAIT, FLAG, CHECK_FLAG, ANIM ���� ó���Ѵ�
/// </summary>
public class TagHandler
{
    private readonly Dictionary<string, bool> flags;
    private readonly Animator animator;
    private readonly CoroutineRunner coroutineRunner;

    public TagHandler(Animator animator = null, CoroutineRunner coroutineRunner = null)
    {
        this.flags = new Dictionary<string, bool>();
        this.animator = animator;
        this.coroutineRunner = coroutineRunner ?? CoroutineRunner.Instance;
    }

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
                Debug.LogWarning($"[TagHandler] �߸��� �±�: {tag}");
                continue;
            }

            string tagType = parts[0].Trim();
            string tagValue = parts.Length > 1 ? parts[1].Trim() : "";

            switch (tagType)
            {
                case "WAIT":
                    if (!float.TryParse(tagValue, out float waitTime) || waitTime <= 0)
                    {
                        Debug.LogWarning($"[TagHandler] �߸��� WAIT ��: {tagValue}, �⺻�� ��� (3��)");
                        waitTime = 3f;
                    }
                    coroutineRunner.StartCoroutine(WaitCoroutine(waitTime, onComplete));
                    return;

                case "FLAG":
                    if (!string.IsNullOrEmpty(tagValue))
                        SetFlag(tagValue, true);
                    else
                        Debug.LogWarning("[TagHandler] FLAG �� ����");
                    break;

                case "CHECK_FLAG":
                    if (!string.IsNullOrEmpty(tagValue) && !CheckFlag(tagValue))
                    {
                        Debug.Log($"[TagHandler] CHECK_FLAG ����: {tagValue}");
                        onComplete?.Invoke();
                        return;
                    }
                    break;

                case "ANIM":
                    if (animator != null && !string.IsNullOrEmpty(tagValue))
                        animator.SetTrigger(tagValue);
                    else
                        Debug.LogWarning($"[TagHandler] ANIM ����: Animator �Ǵ� �� ���� ({tagValue})");
                    break;

                case "Image":
                    if (!string.IsNullOrEmpty(tagValue))
                    {
                        SMSManager.Instance.SaveMessage(tagValue, false);   //�̹��� �̸��� �޼�����
                    }
                    else Debug.Log("taghander �̹��� �±� ����");

                    break;

                default:
                    Debug.LogWarning($"[TagHandler] �������� �ʴ� �±�: {tagType}");
                    break;


            }
        }

        onComplete?.Invoke();
    }

    private IEnumerator WaitCoroutine(float seconds, Action onComplete)
    {
        yield return new WaitForSeconds(seconds);
        onComplete?.Invoke();
    }

    private void SetFlag(string flagName, bool value)
    {
        flags[flagName] = value;
        Debug.Log($"[TagHandler] �÷��� ����: {flagName} = {value}");
    }

    private bool CheckFlag(string flagName)
    {
        return flags.TryGetValue(flagName, out bool value) && value;
    }
}

/// <summary>
/// �ڷ�ƾ ����� �̱��� (���� ����)
/// </summary>
public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
                UnityEngine.Object.DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
}
