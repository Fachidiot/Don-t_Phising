using System.Collections;
using UnityEngine;
using TMPro; // UI 텍스트 표시용

public class Player : MonoBehaviour
{
    [SerializeField]
    private DialogueEvent m_DialogueEvent; // 대화 데이터를 담고 있는 ScriptableObject

    [SerializeField]
    private TMP_Text playerTextUI; // 실제 화면에 출력할 텍스트 UI (TextMeshPro 사용)

    private int currentId = 9000; // 시작할 대사 ID
    private bool isTyping = false; // 현재 타이핑 중인지 여부
    private bool isReadyForNext = false; // 다음 대사로 넘어갈 준비가 되었는지 여부

    [SerializeField]
    private bool introEnd = false;      //처음 인트로 유무

    private Coroutine typeCoroutine; // 현재 실행 중인 코루틴 참조
    private string currentFullText = ""; // 현재 출력할 전체 텍스트
    private string currentDisplayedText = ""; // 현재까지 출력된 텍스트

    

    // 매 프레임마다 키 입력 체크
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                // 타이핑 중일 때 E를 누르면 즉시 전체 텍스트를 출력
                isTyping = false;
                StopCoroutine(typeCoroutine);
                ShowFullTextImmediately();
            }
            else if (isReadyForNext)
            {
                // 텍스트가 다 출력된 후 E를 누르면 다음 대사로 진행
                ProceedNext();
            }
        }
    }

    // 게임 시작 시 인트로 확인 유무 후 텍스트 출력
    private void Start()
    {
        if(introEnd = true) ProceedNext();
    }

    // 현재 ID의 대사를 불러와 타이핑 시작
    private void ProceedNext()
    {
        isReadyForNext = false;

        // ID에 해당하는 대사 찾기
        var dialogue = m_DialogueEvent.lines.Find(d => d.id == currentId);
        if (dialogue == null)
        {
            Debug.LogWarning($"ID {currentId}에 해당하는 대사가 없습니다.");
            return;
        }

        // 텍스트 초기화
        currentFullText = dialogue.text;
        currentDisplayedText = "";

        // 코루틴으로 타이핑 효과 시작
        typeCoroutine = StartCoroutine(TypeTextCoroutine(dialogue.text, dialogue.nextId));
    }

    // 타이핑 효과 구현 코루틴
    private IEnumerator TypeTextCoroutine(string text, int nextId)
    {
        isTyping = true;

        // 한 글자씩 출력
        for (int i = 0; i < text.Length; i++)
        {
            if (!isTyping)
                yield break; // 중간에 종료되면 코루틴 종료

            currentDisplayedText += text[i];
            playerTextUI.text = currentDisplayedText; // UI에 텍스트 반영

            yield return new WaitForSeconds(0.03f); // 타이핑 속도 조절
        }

        isTyping = false;
        isReadyForNext = true;
        currentId = nextId; // 다음 대사 ID 설정
    }

    // 텍스트 전체 즉시 표시
    private void ShowFullTextImmediately()
    {
        currentDisplayedText = currentFullText;
        playerTextUI.text = currentDisplayedText; // UI에 텍스트 반영
        isReadyForNext = true;
    }

    public void introEvent()
    {
        Debug.Log("인트로 텍스트 ");
        introEnd = true;
    }
}
