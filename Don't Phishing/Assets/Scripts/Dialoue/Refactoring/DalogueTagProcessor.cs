using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 대사 내 태그를 파싱하고 순차적으로 처리하는 태그 처리기
/// WAIT, FLAG, CHECK_FLAG, ANIM, Image 등을 지원
/// </summary>
public class DialogueTagProcessor
{
    private Dictionary<string, bool> flags = new();
    private Animator animator;
    private MonoBehaviour coroutineHost;

    /// <summary>
    /// 태그 처리기 생성자
    /// </summary>
    /// <param name="animator">애니메이션 태그용 Animator</param>
    /// <param name="host">코루틴 실행용 MonoBehaviour</param>
    public DialogueTagProcessor(Animator animator = null, MonoBehaviour host = null)
    {
        this.animator = animator;
        this.coroutineHost = host;
    }

    /// <summary>
    /// 태그 문자열 전체 처리 시작
    /// </summary>
    public void Process(string tagString, Action onComplete)
    {
        if (string.IsNullOrWhiteSpace(tagString))
        {
            onComplete?.Invoke();
            return;
        }

        var tags = tagString.Split(',');
        ProcessTagsSequentially(tags, 0, onComplete);
    }

    /// <summary>
    /// 태그들을 순차적으로 처리 (WAIT 등 고려)
    /// </summary>
    private void ProcessTagsSequentially(string[] tags, int index, Action onComplete)
    {
        if (index >= tags.Length)
        {
            onComplete?.Invoke();
            return;
        }

        var parts = tags[index].Split(':');
        string tagType = parts[0].Trim();
        string value = parts.Length > 1 ? parts[1].Trim() : "";

        switch (tagType)
        {
            case "WAIT":
                if (float.TryParse(value, out float waitTime))
                    coroutineHost.StartCoroutine(WaitAndNext(waitTime, () => ProcessTagsSequentially(tags, index + 1, onComplete)));
                else
                    ProcessTagsSequentially(tags, index + 1, onComplete);
                break;

            case "FLAG":
                SetFlag(value, true);
                ProcessTagsSequentially(tags, index + 1, onComplete);
                break;

            case "CHECK_FLAG":
                if (!CheckFlag(value))
                    onComplete?.Invoke(); // 조건 실패 시 중단
                else
                    ProcessTagsSequentially(tags, index + 1, onComplete);
                break;

            case "ANIM":
                animator?.SetTrigger(value);
                ProcessTagsSequentially(tags, index + 1, onComplete);
                break;

            case "Image":
                SMSManager.Instance?.SaveMessage(value, false); // 이미지 메시지 전송
                ProcessTagsSequentially(tags, index + 1, onComplete);
                break;

            default:
                Debug.LogWarning($"[TagProcessor] 지원하지 않는 태그: {tagType}");
                ProcessTagsSequentially(tags, index + 1, onComplete);
                break;
        }
    }

    /// <summary>
    /// 대기 시간 후 다음 태그로 이동하는 코루틴
    /// </summary>
    private IEnumerator WaitAndNext(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback?.Invoke();
    }

    /// <summary>
    /// 내부 플래그 값 설정
    /// </summary>
    private void SetFlag(string name, bool value) => flags[name] = value;

    /// <summary>
    /// 내부 플래그 값 확인
    /// </summary>
    private bool CheckFlag(string name) => flags.TryGetValue(name, out bool v) && v;
}
