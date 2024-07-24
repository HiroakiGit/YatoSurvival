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
    public PlayerAttackIndicator _PlayerAttackIndicator;
    public PlayerAnimation _PlayerAnimation;
    public RankingManager _RankingManager;

    public bool isAutoLogin;
    private bool isGameStarted = false;
    private bool isGameFinished = false;

    [Header("GameOverUI")]
    public GameObject GameOverCanvas;
    public Text scoreText;
    public List<GameObject> gameOverNonActiveCanvasList = new List<GameObject>();

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
        _PlayerAttackIndicator.enabled = false;
        _PlayerAnimation.enabled = false;
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        _PlayerAttackIndicator.enabled = true;
        _PlayerAnimation.enabled = true;
        Time.timeScale = 1;
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        _PlayerAttackIndicator.enabled = false;
        StartCoroutine(_PlayerAnimation.PlayerDead());

        GameOverCanvas.SetActive(true);
        scoreText.text = $"��������[��:�b]\n{_Timer.minText.text}:{_Timer.secText.text}";

        for(int i = 0; i < gameOverNonActiveCanvasList.Count; i++)
        {
            gameOverNonActiveCanvasList[i].gameObject.SetActive(false);
        }

        isGameFinished = true;
        _RankingManager.SubmitScore();
    }

    public bool IsGameStarted() { return isGameStarted; }

    public bool IsGameFinished() { return isGameFinished; }
}
