using UnityEngine;

/// <summary>
/// 저장 슬롯 UI 6개를 자동으로 생성하고 제어하는 매니저
/// 저장 모드 또는 불러오기 모드 전환 가능
/// </summary>
public class SaveSlotManager : MonoBehaviour
{
    [Header("슬롯 프리팹 및 배치 위치")]
    [SerializeField] private GameObject slotPrefab;           // SaveSlotUI 프리팹
    [SerializeField] private Transform slotParent;            // 슬롯 배치할 부모 오브젝트

    [Header("슬롯 수")]
    [SerializeField] private int slotCount = 6;

    [Header("모드 설정")]
    [SerializeField] private SaveSlotUI.SlotMode mode = SaveSlotUI.SlotMode.Save;

    [Header("현재 대사 정보")]
    [SerializeField] private string currentEventName;
    [SerializeField] private int currentDialogueId;

    private void Start()
    {
        GenerateSlots();
    }


    // 슬롯들을 생성하고 초기화합니다.
    private void GenerateSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            SaveSlotUI slotUI = slotObj.GetComponent<SaveSlotUI>();

            slotUI.Initialize(i, mode, OnSlotClicked);
        }
    }


    // 슬롯 클릭 시 동작: 저장 또는 불러오기

    private void OnSlotClicked(int slotIndex)
    {
        if (mode == SaveSlotUI.SlotMode.Save)
        {
            if (string.IsNullOrEmpty(currentEventName))
            {
                Debug.LogWarning("[SaveSlotManager] 저장 실패 - 이벤트 이름 없음");
                return;
            }

            SMSManager.Instance.SaveDialogueSlot(currentEventName, currentDialogueId, slotIndex);
        }
        else if (mode == SaveSlotUI.SlotMode.Load)
        {
            DialogueSaveData data = SMSManager.Instance.LoadDialogueSlot(slotIndex);
            if (data != null)
            {
                // DialogueEvent 불러와서 DialogueController에 전달
                DialogueEvent loadedEvent = Resources.Load<DialogueEvent>($"DialogueEvents/{data.dialogueEventName}");
                if (loadedEvent != null)
                {
                    DialogueController controller = FindObjectOfType<DialogueController>();
                    controller.StartDialogue(loadedEvent);
                    controller.ProceedNext(data.currentId);
                }
                else
                {
                    Debug.LogWarning($"[SaveSlotManager] 이벤트 파일 찾을 수 없음: {data.dialogueEventName}");
                }
            }
        }
    }


    // 외부에서 현재 대사 정보를 세팅하는 함수
    public void SetCurrentDialogue(string eventName, int dialogueId)
    {
        currentEventName = eventName;
        currentDialogueId = dialogueId;
    }


    // 슬롯 모드 변경 (Save <-> Load)
    public void SetMode(SaveSlotUI.SlotMode newMode)
    {
        mode = newMode;
    }
}
