using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public BuffAndDeBuffManager _BuffAndDeBuffManager;
    public List<Question> normalQuestions;
    public List<Question> hardQuestions;
    [Range(0, 1)] public float hardQuestionChance = 0.2f;
    [Range(0, 1)] public float probabilityGetDeBuff = 0.6f;
    private Question currentQuestion;

    [Header("UI")]
    public GameObject QuestionCanvas;
    public GameObject QuestionUI;
    public GameObject AnswerUI;
    public Text questionAndAnswerText;
    public Text questionAndAnswerContentText;
    public Image MaruAndBatsuImage;
    public Sprite[] MaruAndBatsuSprites;

    [Header("Audio")]
    public AudioClip collectSoundClip;
    public AudioClip inCollectSoundClip;

    private void Start()
    {
        QuestionCanvas.SetActive(false);
        InitalizeUI();
    }

    public void StartQuestion()
    {
        InitalizeUI();

        Time.timeScale = 0;
        GameManager.Instance.isProcessing = true;

        Question question = GetRandomQuestion();
        ShowQuestion(question);
    }

    private Question GetRandomQuestion()
    {
        List<Question> selectedList = Random.value < hardQuestionChance ? hardQuestions : normalQuestions;
        int index = Random.Range(0, selectedList.Count);
        return selectedList[index];
    }

    private void ShowQuestion(Question question)
    {
        currentQuestion = question;

        questionAndAnswerText.text = "���";
        questionAndAnswerContentText.text = question.questionText;

        QuestionCanvas.SetActive(true);
        QuestionUI.SetActive(true);
    }

    public void OnClickYesButton()
    {
        StartCoroutine(OnQuestionAnswered(currentQuestion.isCorrect));
    }

    public void OnClickNoButton()
    {
        StartCoroutine(OnQuestionAnswered(!currentQuestion.isCorrect));
    }

    private IEnumerator OnQuestionAnswered(bool isCorrect)
    {
        InitalizeUI();
        AnswerUI.SetActive(true);

        if (isCorrect)
        {
            //�}��
            MaruAndBatsuImage.sprite = MaruAndBatsuSprites[0];
            SEAudio.Instance.PlayOneShot(collectSoundClip, 0.2f);
        }
        else
        {
            //�o�c
            MaruAndBatsuImage.sprite = MaruAndBatsuSprites[1];
            SEAudio.Instance.PlayOneShot(inCollectSoundClip, 0.2f);

            //���������� => ������\�� => �I��
            //�@�@�@�Ȃ� => �I��
            if (currentQuestion.answerText != string.Empty)
            {
                yield return new WaitForSecondsRealtime(1.5f);
                InitalizeUI();
                questionAndAnswerText.text = "����";
                questionAndAnswerContentText.text = currentQuestion.answerText;
            }
        }

        yield return new WaitForSecondsRealtime(1.5f);
        InitalizeUI();
        QuestionCanvas.SetActive(false);

        //�f�o�t�t�^
        if (!isCorrect) 
        {
            float r = Random.Range(0f, 1f);

            if(r <= probabilityGetDeBuff)
            {
                _BuffAndDeBuffManager.StartDeBuffProcess();
            }
            else
            {
                LogManager.Instance.AddLogs("�����N����Ȃ�����...");
                LogManager.Instance.Log(2f,null);
            }
        }

        GameManager.Instance.isProcessing = false;
        GameManager.Instance.ContinueGame();
    }

    private void InitalizeUI()
    {
        QuestionUI.SetActive(false);
        AnswerUI.SetActive(false);
        questionAndAnswerText.text = string.Empty;
        questionAndAnswerContentText.text = string.Empty;
        MaruAndBatsuImage.sprite = null;
    }
}
