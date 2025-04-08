using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    // �̱��� ����
    public static DialogueManager Instance { get; private set; }

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;         // ��ü ��ȭ �г� 
    [SerializeField] private GameObject[] choices;             // ������ ��ư��
    [SerializeField] private Transform messageContainer;       // �޽��� �������� ���� ��ġ (Vertical Layout Group �� ����)

    //[Header("Prefabs")]
    //[SerializeField] private GameObject npcMessagePrefab;      // NPC �޽��� ������
    //[SerializeField] private GameObject playerMessagePrefab;   // �÷��̾� �޽��� ������

    [Header("Animation")]
    [SerializeField] private Animator dialogueAnimator;        // ��ȭâ �ִϸ��̼�

    private TextMeshProUGUI[] choicesText;                     // ������ �ؽ�Ʈ ĳ��
    private Story currentStory;                                // ���� Ink ���丮
    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;
    public bool dialogueIsPlaying { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        dialoguePanel.SetActive(false);

        // ������ ��ư �ؽ�Ʈ ĳ��
        choicesText = new TextMeshProUGUI[choices.Length];
        for (int i = 0; i < choices.Length; i++)
        {
            choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    /// <summary>
    /// Ink JSON �޾Ƽ� ��ȭ ����
    /// </summary>
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        //dialogueAnimator.Play("DialogueBox_Open");

        ContinueStory();
    }

    /// <summary>
    /// ���� �ٷ� ���� (or ����)
    /// </summary>
    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string nextLine = currentStory.Continue();

            if (displayLineCoroutine != null)
                StopCoroutine(displayLineCoroutine);

            displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    /// <summary>
    /// �� �پ� ��� (Ÿ�� ȿ�� ����, �ٷ� ����)
    /// </summary>
    private IEnumerator DisplayLine(string line)
    {
        HideChoices();

        // NPC �޽��� ������ ����
        SpawnNPCMessage(line);

        yield return new WaitForSeconds(0.4f);

        DisplayChoices(); // ���� ������ ���
        canContinueToNextLine = true;
    }

    /// <summary>
    /// �÷��̾� ���� �ݿ�
    /// </summary>
    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            string selectedText = currentStory.currentChoices[choiceIndex].text;

            // �÷��̾� ��ǳ�� ����
            SpawnPlayerMessage(selectedText);

            // ���� �ݿ�
            currentStory.ChooseChoiceIndex(choiceIndex);
            StartCoroutine(WaitAndContinue());
        }
    }

    /// <summary>
    /// 1������ �� Continue ȣ��
    /// </summary>
    private IEnumerator WaitAndContinue()
    {
        yield return null;
        ContinueStory();
    }

    /// <summary>
    /// ������ �����
    /// </summary>
    private void HideChoices()
    {
        foreach (var choice in choices)
            choice.SetActive(false);
    }

    /// <summary>
    /// ������ ǥ��
    /// </summary>
    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        for (int i = 0; i < currentChoices.Count; i++)
        {
            choices[i].SetActive(true);
            choicesText[i].text = currentChoices[i].text;
        }

        for (int i = currentChoices.Count; i < choices.Length; i++)
        {
            choices[i].SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    /// <summary>
    /// ù ��° ������ �⺻ ��Ŀ��
    /// </summary>
    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0]);
    }

    /// <summary>
    /// ��ȭ ����
    /// </summary>
    private IEnumerator ExitDialogueMode()
    {
        dialogueAnimator.Play("DialogueBox_Close");
        yield return new WaitForSeconds(0.3f);

        dialogueIsPlaying = false;
        HideChoices();
        dialoguePanel.SetActive(false);
    }

    /// <summary>
    /// NPC �޽��� ����
    /// </summary>
    private void SpawnNPCMessage(string message)
    {
        //GameObject go = Instantiate(npcMessagePrefab, messageContainer);
        //go.GetComponentInChildren<TextMeshProUGUI>().text = message;

        SMSManager.Instance.SaveMessage(message, false);
    }

    /// <summary>
    /// �÷��̾� �޽��� ����
    /// </summary>
    private void SpawnPlayerMessage(string message)
    {
        //GameObject go = Instantiate(playerMessagePrefab, messageContainer);
        //go.GetComponentInChildren<TextMeshProUGUI>().text = message;

        SMSManager.Instance.SaveMessage(message, true);
    }
}
