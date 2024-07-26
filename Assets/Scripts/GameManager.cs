using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public LoginManager _loginManager;
    public MapEdgeEnemySpawner _enemyGenerater;
    public Player _Player;
    public Timer _Timer;
    public RankingManager _RankingManager;

    public bool isAutoLogin;
    private bool isGameStarted = false;
    private bool isGameFinished = false;

    [Header("GameOver")]
    public GameObject GameOverCanvas;
    public Text scoreText;
    public List<GameObject> gameOverNonActiveCanvasList = new List<GameObject>();
    public AudioClip gameOverSoundClip;

    //GameManager����e�v���O�����ɏ��������肢���� => �e�v���O��������I���񍐂��󂯎��

    void Awake()
    {
        // �V���O���g���p�^�[���̎���
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //��ԍŏ�
    private void Start()
    {
        GameOverCanvas.SetActive(false);

        //���O�C���J�n
        if (isAutoLogin) _loginManager.AutoLogin();
        else _loginManager.StartLogin();
    }

    //���O�C���I����
    public void OnLoginEnd()
    {
        StartGame();
    }

    public void StartGame()
    {
        isGameStarted = true;
        _Player._PlayerAttack.GenerateInitialWeapon();
        _Timer.TimeCountStart();
    }

    public void PauseGame()
    {
        _Player._PlayerAttackIndicator.enabled = false;
        _Player._PlayerAnimation.enabled = false;
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        _Player._PlayerAttackIndicator.enabled = true;
        _Player._PlayerAnimation.enabled = true;
        Time.timeScale = 1;
    }

    public async void EndGame()
    {
        isGameFinished = true;
        BGMAndSEAudio.Instance.PauseBGM();
        PlayerAudio.Instance.PauseBGM();
        Time.timeScale = 0;

        foreach (Transform obj in _Player.playerTransform)
        {
            obj.gameObject.SetActive(false);
        }

        for (int i = 0; i < gameOverNonActiveCanvasList.Count; i++)
        {
            gameOverNonActiveCanvasList[i].gameObject.SetActive(false);
        }

        _RankingManager.SubmitScore();

        await _Player._PlayerAnimation.PlayerDead();

        BGMAndSEAudio.Instance.PlayBGM(gameOverSoundClip);
        BGMAndSEAudio.Instance.BGMAndSEAudioSource.loop = false;

        GameOverCanvas.SetActive(true);
        scoreText.text = $"��������[��:�b]\n{_Timer.minText.text}:{_Timer.secText.text}";

    }

    public bool IsGameStarted() { return isGameStarted; }

    public bool IsGameFinished() { return isGameFinished; }
}
