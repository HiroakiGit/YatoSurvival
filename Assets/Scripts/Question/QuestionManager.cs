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
    [Range(0, 1)] public float probabilityGetBuff = 0.2f;
    [Range(0, 1)] public float probabilityGetDeBuff = 0.6f;
    private Question currentQuestion;

    [Header("UI")]
    public GameObject StartingQuestionCanvas;
    public GameObject QuestionCanvas;
    public GameObject QuestionUI;
    public GameObject[] HardQuestionUIs;
    public GameObject AnswerUI;
    public Text questionAndAnswerText;
    public Text questionAndAnswerContentText;
    public Image MaruAndBatsuImage;
    public Sprite[] MaruAndBatsuSprites;

    [Header("Audio")]
    public AudioClip[] HardQuestionBGMSoundClips;
    public AudioClip questionSoundClip;
    public AudioClip collectSoundClip;
    public AudioClip inCollectSoundClip;

    private List<AudioClip> schoolChimeClipList = new List<AudioClip>();
    private float[] chimeFrequencies = new float[] { 350f, 440f, 392f, 261f, 350f, 392f, 440f, 350f }; 
    private float noteDuration = 0.5f;

    private void Start()
    {
        StartingQuestionCanvas.SetActive(false);
        QuestionCanvas.SetActive(false);
        InitalizeUI();

        for (int i = 0; i < chimeFrequencies.Length; i++) 
        {
            AudioClip clip = SoundClipCreator.Instance.CreateClip(chimeFrequencies[i], chimeFrequencies[i], noteDuration, false);
            schoolChimeClipList.Add(clip);
        }
    }

    public IEnumerator StartingQuestion()
    {
        StartingQuestionCanvas.SetActive(true);

        for (int i = 0; i < schoolChimeClipList.Count; i++)
        {
            SEAudio.Instance.PlayOneShot(schoolChimeClipList[i], 0.1f);

            //�����̎�
            if(schoolChimeClipList.Count / 2 - 1 == i)
            {
                yield return new WaitForSecondsRealtime(0.7f);
            }
            else
            {
                yield return new WaitForSecondsRealtime(0.65f);
            }
        }
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

        //����Փx
        if (currentQuestion.isHard)
        {
            BGMAudio.Instance.BGMAudioSource.loop = false;
            BGMAudio.Instance.PlayBGM(HardQuestionBGMSoundClips[0], false);
            StartCoroutine(BGMAudio.Instance.CheckingIsPlaying(() => {
                BGMAudio.Instance.PlayBGM(HardQuestionBGMSoundClips[1], true);
            }));

            for (int i = 0; i < HardQuestionUIs.Length; i++)
            {
                HardQuestionUIs[i].SetActive(true);
            }
        }

        SEAudio.Instance.PlayOneShot(questionSoundClip, 0.2f);
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
        }

        //���������� => ������\�� => �I��
        //�@�@�@�Ȃ� => �I��
        if (currentQuestion.answerText != string.Empty)
        {
            yield return new WaitForSecondsRealtime(1.5f);
            InitalizeUI();
            questionAndAnswerText.text = "����";
            questionAndAnswerContentText.text = currentQuestion.answerText;
        }

        yield return new WaitForSecondsRealtime(1.5f);
        currentQuestion = null;
        InitalizeUI();
        QuestionCanvas.SetActive(false);

        //�o�t�t�^
        if (isCorrect) 
        {
            float r = Random.Range(0f, 1f);

            if (r <= probabilityGetBuff)
            {
                _BuffAndDeBuffManager.StartBuffProcess();
            }
        }
        //�f�o�t�t�^
        else
        {
            float r = Random.Range(0f, 1f);

            if (r <= probabilityGetDeBuff)
            {
                _BuffAndDeBuffManager.StartDeBuffProcess();
            }
            else
            {
                LogManager.Instance.AddLogs("�����N����Ȃ�����...");
                LogManager.Instance.Log(2f, null);
            }
        }

        GameManager.Instance.isProcessing = false;
        GameManager.Instance.ContinueGame();
    }

    private void InitalizeUI()
    {
        StartingQuestionCanvas.SetActive(false);
        QuestionUI.SetActive(false);
        for (int i = 0; i < HardQuestionUIs.Length; i++)
        {
            //��肠��Ƃ�
            if (currentQuestion != null) 
            {
                //��Փx���̂Ƃ�
                if (currentQuestion.isHard)
                {
                    if (i == 0)
                    {
                        //BG�͎c��
                        HardQuestionUIs[i].SetActive(true);
                    }
                    else
                    {
                        HardQuestionUIs[i].SetActive(false);
                    }
                }
                //���ʂ̂Ƃ�
                else
                {
                    HardQuestionUIs[i].SetActive(false);
                }

            }
            //�Ȃ��Ƃ�
            else
            {
                HardQuestionUIs[i].SetActive(false);
            }
        }
        AnswerUI.SetActive(false);
        questionAndAnswerText.text = string.Empty;
        questionAndAnswerContentText.text = string.Empty;
        MaruAndBatsuImage.sprite = null;
    }
}
