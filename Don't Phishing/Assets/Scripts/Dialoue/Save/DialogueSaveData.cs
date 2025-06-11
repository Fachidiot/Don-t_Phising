using System;

/// <summary>
/// 대화 저장 데이터 구조 클래스
/// 현재 대화 이벤트 이름, 진행 중인 ID, 저장 시각을 포함
/// </summary>
[Serializable]
public class DialogueSaveData
{
    public string dialogueEventName;  // ScriptableObject 이름
    public int currentId;             // 현재 진행 중인 대사 ID
    public string savedTime;          // 저장된 시각 (표시용)

    public DialogueSaveData(string eventName, int id)
    {
        dialogueEventName = eventName;
        currentId = id;
        savedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
