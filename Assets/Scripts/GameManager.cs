using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public LoginManager _loginManager;
    public MapEdgeEnemySpawner _enemyGenerater;
    public Player _Player;
    public Timer _Timer;
    public PlayerAttackIndicator _PlayerAttackIndicator;
    public PlayerAnimation _PlayerAnimation;
    public StrengtheningManager _StrengtheningManager;
    public QuestionManager _QuestionManager;

    public bool isAutoLogin;
    private bool isGameStarted = false;
    private bool isGameFinished = false;

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
        _StrengtheningManager.StrengtheningCanvas.SetActive(false);
        _QuestionManager.QuestionCanvas.SetActive(false);
        PauseGame();
        isGameFinished = true;
        _Timer.SubmitScore();
    }

    public bool IsGameStarted() { return isGameStarted; }

    public bool IsGameFinished() { return isGameFinished; }
}
