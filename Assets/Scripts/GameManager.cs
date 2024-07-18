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

    public bool isAutoLogin;
    private bool isGameStarted = false;
    private bool isGameFinished = false;

    //GameManagerから各プログラムに処理をお願いする => 各プログラムから終了報告を受け取る

    void Awake()
    {
        // シングルトンパターンの実装
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

    //一番最初
    private void Start()
    {
        //ログイン開始
        if (isAutoLogin) _loginManager.AutoLogin();
        else _loginManager.StartLogin();
    }

    //ログイン終了時
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
        PauseGame();
        isGameFinished = true;
        _Timer.SubmitScore();
    }

    public bool IsGameStarted() { return isGameStarted; }

    public bool IsGameFinished() { return isGameFinished; }
}
