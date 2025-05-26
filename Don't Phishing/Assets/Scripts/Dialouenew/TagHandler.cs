using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

/// <summary>
/// 태그를 파싱하고 처리하는 유틸리티 클래스
/// WAIT, FLAG, CHECK_FLAG, ANIM 등을 처리한다
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
                Debug.LogWarning($"[TagHandler] 잘못된 태그: {tag}");
                continue;
            }

            string tagType = parts[0].Trim();
            string tagValue = parts.Length > 1 ? parts[1].Trim() : "";

            switch (tagType)
            {
                case "WAIT":
                    if (!float.TryParse(tagValue, out float waitTime) || waitTime <= 0)
                    {
                        Debug.LogWarning($"[TagHandler] 잘못된 WAIT 값: {tagValue}, 기본값 사용 (3초)");
                        waitTime = 3f;
                    }
                    coroutineRunner.StartCoroutine(WaitCoroutine(waitTime, onComplete));
                    return;

                case "FLAG":
                    if (!string.IsNullOrEmpty(tagValue))
                        SetFlag(tagValue, true);
                    else
                        Debug.LogWarning("[TagHandler] FLAG 값 누락");
                    break;

                case "CHECK_FLAG":
                    if (!string.IsNullOrEmpty(tagValue) && !CheckFlag(tagValue))
                    {
                        Debug.Log($"[TagHandler] CHECK_FLAG 실패: {tagValue}");
                        onComplete?.Invoke();
                        return;
                    }
                    break;

                case "ANIM":
                    if (animator != null && !string.IsNullOrEmpty(tagValue))
                        animator.SetTrigger(tagValue);
                    else
                        Debug.LogWarning($"[TagHandler] ANIM 실패: Animator 또는 값 없음 ({tagValue})");
                    break;

                case "Image":
                    if (!string.IsNullOrEmpty(tagValue))
                    {
                        SMSManager.Instance.SaveMessage(tagValue, false);   //이미지 이름을 메세지로
                    }
                    else Debug.Log("taghander 이미지 태그 누락");

                    break;

                default:
                    Debug.LogWarning($"[TagHandler] 지원하지 않는 태그: {tagType}");
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
        Debug.Log($"[TagHandler] 플래그 설정: {flagName} = {value}");
    }

    private bool CheckFlag(string flagName)
    {
        return flags.TryGetValue(flagName, out bool value) && value;
    }
}

/// <summary>
/// 코루틴 실행용 싱글톤 (변경 없음)
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
