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

    private void Start()
    {
        QuestionCanvas.SetActive(false);
    }

    public void StartQuestion()
    {
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
            //����������
            Debug.Log("Correct!");
        }
        else
        {
            //�G����莞�ԋ���
            Debug.Log("InCorrect");
        }

        GameManager.Instance.ContinueGame();
    }
}
