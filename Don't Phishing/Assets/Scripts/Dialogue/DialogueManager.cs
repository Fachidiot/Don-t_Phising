using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;            // ��ȭ ��ü �г�
    [SerializeField] private GameObject[] choices;                // ������ ��ư��
    [Header("Animation")]
    [SerializeField] private Animator dialogueAnimator;           // ��ȭâ ���� �ݱ� �ִϸ��̼�
    //[SerializeField] private GameObject dialogueDelayAni;         // ... �ִϸ��̼� ������Ʈ

    private TextMeshProUGUI[] choicesText;                        // ������ ��ư �ؽ�Ʈ
    private Story currentStory;                                   // Ink ���丮
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

        // ������ �ؽ�Ʈ �ʱ�ȭ
        choicesText = new TextMeshProUGUI[choices.Length];
        for (int i = 0; i < choices.Length; i++)
        {
            choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    /// <summary>
    /// Ink JSON ���� �޾Ƽ� ��ȭ ��� ����
    /// </summary>
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    /// <summary>
    /// ���� ���� �Ѿ��
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
    /// ��� �� �� ��� (NPC/�÷��̾�/�˸� ����, Ÿ�� ȿ�� ����)
    /// </summary>
    private IEnumerator DisplayLine(string line)
    {
        HideChoices();

        // Ink �±� �м�
        bool isNpcLine = false;
        bool isPlayerLine = false;
        bool isAlarm = false;

        foreach (string tag in currentStory.currentTags)
        {
            if (tag == "npc") isNpcLine = true;
            if (tag == "player") isPlayerLine = true;
            if (tag.Contains("alarm")) isAlarm = true;
        }

        // �˸� ȿ�� ó��
        if (isAlarm)
        {
            SMSManager.Instance.SaveMessage(line, false); // �˸� ó�� (���߿� ȿ�� �߰� ����)
            yield return new WaitForSeconds(0.5f);
        }

        // �÷��̾� �޽���: �ٷ� ���
        if (isPlayerLine)
        {
            SMSManager.Instance.SaveMessage(line, true);
            yield return new WaitForSeconds(0.5f);
        }

        // NPC �޽���: �ִϸ��̼� + õõ�� ���
        if (isNpcLine)
        {
            string[] lines = line.Split('\n');

            foreach (string singleLine in lines)
            {
                //dialogueDelayAni.SetActive(true);
                yield return new WaitForSeconds(0.8f);
                // dialogueDelayAni.SetActive(false);

               // SMSManager.Instance.SaveMessage("", true);
                yield return StartCoroutine(TypeText(singleLine));
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(0.5f);
        }

        DisplayChoices();
        canContinueToNextLine = true;
    }

    /// <summary>
    /// Ÿ���� ȿ��: �� ���ھ� ���
    /// </summary>
    private IEnumerator TypeText(string fullText)
    {
        string temp = "";

        // �ϴ� NPC �޽��� ����
        SMSManager.Instance.SaveMessage("", false);

        foreach (char c in fullText)
        {
            temp += c;
            SMSManager.Instance.UpdateLastNPCMessage(temp);
            yield return new WaitForSeconds(0.03f); // Ÿ�� ȿ�� �ӵ�
        }
    }

    /// <summary>
    /// �÷��̾� ���� �ݿ�
    /// </summary>
    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            StartCoroutine(WaitAndContinue());
        }
    }

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
    /// WAIT �±� ����
    /// </summary>
    public void ContinueAfterWait()
    {
        DialogueStateManager.Instance.ClearState();
        ContinueStory();
    }
}
