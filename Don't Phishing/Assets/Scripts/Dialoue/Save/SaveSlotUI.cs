using UnityEngine;
using TMPro;
using UnityEngine.UI;

// 세이브 슬롯 UI 하나를 제어하는 스크립트
// 저장된 데이터 표시 및 클릭 이벤트 처리

public class SaveSlotUI : MonoBehaviour
{
    [Header("UI 구성 요소")]
    [SerializeField] private TMP_Text slotTitleText;    // 이벤트 이름
    [SerializeField] private TMP_Text timeText;         // 저장 시각
    [SerializeField] private Button slotButton;         // 클릭 버튼

    private int slotIndex;  // 이 슬롯의 고유 번호

    public enum SlotMode { Save, Load }
    private SlotMode mode;


    // 슬롯 초기화 함수
    public void Initialize(int index, SlotMode mode, System.Action<int> onClickAction)
    {
        this.slotIndex = index;
        this.mode = mode;

        DialogueSaveData data = SMSManager.Instance.LoadDialogueSlot(index);

        if (data != null)
        {
            slotTitleText.text = data.dialogueEventName;
            timeText.text = data.savedTime;
        }
        else
        {
            slotTitleText.text = "빈 슬롯";
            timeText.text = "";
        }

        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(() => onClickAction?.Invoke(slotIndex));
    }
}
