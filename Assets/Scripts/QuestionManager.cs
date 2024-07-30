using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public List<Question> normalQuestions;
    public List<Question> hardQuestions;
    [Range(0, 1)] public float hardQuestionChance = 0.2f;
    private Question currentQuestion;

    [Header("UI")]
    public GameObject QuestionCanvas;
    public Text questionContentText;
    private System.Action<bool> onAnswered;

    [Header("Audio")]
    public AudioClip collectSoundClip;
    public AudioClip inCollectSoundClip;

    private void Start()
    {
        QuestionCanvas.SetActive(false);
    }

    public void StartQuestion()
    {
        GameManager.Instance.PauseGame();
        Question question = GetRandomQuestion();
        ShowQuestion(question, OnQuestionAnswered);
    }

    private Question GetRandomQuestion()
    {
        List<Question> selectedList = Random.value < hardQuestionChance ? hardQuestions : normalQuestions;
        int index = Random.Range(0, selectedList.Count);
        return selectedList[index];
    }

    private void ShowQuestion(Question question, System.Action<bool> onAnsweredCallback)
    {
        currentQuestion = question;
        onAnswered = onAnsweredCallback;
        questionContentText.text = question.questionText;
        QuestionCanvas.SetActive(true);
    }

    public void OnClickYesButton()
    {
        onAnswered.Invoke(currentQuestion.isCorrect);
        QuestionCanvas.SetActive(false);
    }

    public void OnClickNoButton()
    {
        onAnswered.Invoke(!currentQuestion.isCorrect);
        QuestionCanvas.SetActive(false);
    }

    private void OnQuestionAnswered(bool isCorrect)
    {
        if (isCorrect)
        {
            //Ž©•ª‚ð‹­‰»
            SEAudio.Instance.PlayOneShot(collectSoundClip, 0.2f);
        }
        else
        {
            //“G‚ðˆê’èŽžŠÔ‹­‰»
            SEAudio.Instance.PlayOneShot(inCollectSoundClip, 0.2f);
        }

        GameManager.Instance.ContinueGame();
    }
}
