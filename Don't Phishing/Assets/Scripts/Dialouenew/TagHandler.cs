using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

// 태그를 파싱하고 처리 (WAIT, FLAG, ANIM 등)
public class TagHandler
{
    // 플래그 상태 저장
    private readonly Dictionary<string, bool> flags;

    // 애니메이터 참조
    private readonly Animator animator;   // <- 대기 ... 애니메이션 같은 거 

    // 코루틴 실행용
    private readonly CoroutineRunner coroutineRunner;

    // 생성자: Animator와 CoroutineRunner 초기화
    public TagHandler(Animator animator = null, CoroutineRunner coroutineRunner = null)
    {
        flags = new Dictionary<string, bool>();
        this.animator = animator;
        this.coroutineRunner = coroutineRunner ?? CoroutineRunner.Instance;
    }

    // 태그 문자열 파싱 및 처리, 완료 시 콜백 호출
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
                Debug.LogWarning($"잘못된 태그: {tag}");
                continue;
            }

            string tagType = parts[0].Trim();
            string tagValue = parts.Length > 1 ? parts[1].Trim() : "";

            switch (tagType)
            {
                case "WAIT":
                    if (!float.TryParse(tagValue, out float waitTime) || waitTime <= 0)
                    {
                        Debug.LogWarning($"잘못된 WAIT 값: {tagValue}");
                        waitTime = 1f;
                    }
                    coroutineRunner.StartCoroutine(WaitCoroutine(waitTime, onComplete));
                    return;

                case "FLAG":
                    if (!string.IsNullOrEmpty(tagValue))
                        SetFlag(tagValue, true);
                    else
                        Debug.LogWarning("FLAG 값 누락");
                    break;

                case "CHECK_FLAG":
                    if (!string.IsNullOrEmpty(tagValue) && !CheckFlag(tagValue))
                    {
                        Debug.Log($"CHECK_FLAG 실패: {tagValue}");
                        onComplete?.Invoke();
                        return;
                    }
                    break;

                case "ANIM":
                    if (animator != null && !string.IsNullOrEmpty(tagValue))
                        animator.SetTrigger(tagValue);
                    else
                        Debug.LogWarning($"ANIM 실패: Animator 또는 값 없음 ({tagValue})");
                    break;

                default:
                    Debug.LogWarning($"지원하지 않는 태그: {tagType}");
                    break;
            }
        }

        onComplete?.Invoke();
    }

    // 대기 시간 후 콜백 호출
    private IEnumerator WaitCoroutine(float seconds, Action onComplete)
    {
        yield return new WaitForSeconds(seconds);
        onComplete?.Invoke();
    }

    // 플래그 설정
    private void SetFlag(string flagName, bool value)
    {
        flags[flagName] = value;
        Debug.Log($"플래그 설정: {flagName} = {value}");
    }

    // 플래그 상태 확인
    private bool CheckFlag(string flagName)
    {
        return flags.TryGetValue(flagName, out bool value) && value;
    }
}

// 코루틴 실행용 싱글톤
public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner instance;

    // 싱글톤 인스턴스, 없으면 생성
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