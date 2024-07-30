using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    MainState,
    PauseState,
    SubMenuState
}

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

    [Header("UI")]
    public GameState CurrentState;
    public AudioClip pushedButtonSoundClip;
    public bool isProcessing = false;

    [Header("GamePause")]
    public GameObject GamePauseCanvas;

    [Header("GameOver")]
    public GameObject GameOverCanvas;
    public Text scoreText;
    public List<GameObject> gameOverNonActiveCanvasList = new List<GameObject>();
    public AudioClip gameOverSoundClip;

    [Header("GameSetting")]
    public Slider BGMVolumeSlider;
    public Slider SEVolumeSlider;

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

    //初期化
    private void Start()
    {
        CurrentState = GameState.MainState;

        GameOverCanvas.SetActive(false);
        GamePauseCanvas.SetActive(false);

        // スライダーの初期値をオーディオソースの音量に設定
        BGMVolumeSlider.value = 0.5f;
        BGMVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        
        SEVolumeSlider.value = 0.5f;
        SEVolumeSlider.onValueChanged.AddListener(SetSEVolume);

        //ログイン開始
        if (isAutoLogin) _loginManager.AutoLogin();
        else _loginManager.StartLogin();
    }

    //ログイン終了時
    public void OnLoginEnd()
    {
        //TODO: 開始までDelayいれる
        StartGame();
    }

    //ゲーム開始
    public void StartGame()
    {
        isGameStarted = true;
        //初期武器生成
        _Player._PlayerAttack.GenerateInitialWeapon();
        //時間測定開始
        _Timer.TimeCountStart();
    }

    private void Update()
    {
        //ESCキーを押したとき
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscapePress();
        }
    }

    private void HandleEscapePress()
    {
        switch (CurrentState)
        {
            case GameState.MainState:

                if (isGameFinished) return;
                PauseGame();
                SEAudio.Instance.PlayOneShot(pushedButtonSoundClip, 0.1f);
                break;

            case GameState.PauseState:

                if (isGameFinished) return;
                ContinueGame();
                SEAudio.Instance.PlayOneShot(pushedButtonSoundClip, 0.1f);
                break;

            case GameState.SubMenuState:

                if(isProcessing) return;
                CloseSubMenu();
                SEAudio.Instance.PlayOneShot(pushedButtonSoundClip, 0.1f);
                break;
        }
    }

    public void PauseGame()
    {
        GamePauseCanvas.SetActive(true);
        CurrentState = GameState.PauseState;

        //TimeScaleに影響されないスクリプトで、非有効にしたいプログラムを非有効にする
        _Player._PlayerAttackIndicator.enabled = false;
        _Player._PlayerAnimation.enabled = false;

        Time.timeScale = 0;
    }

    private void CloseSubMenu()
    {
        _RankingManager.OnClickBackButton();
        CurrentState = GameState.PauseState;
    }

    public void ContinueGame()
    {
        GamePauseCanvas.SetActive(false);
        CurrentState = GameState.MainState;

        //TimeScaleに影響されないスクリプトで、非有効にしていたプログラムを有効にする
        _Player._PlayerAttackIndicator.enabled = true;
        _Player._PlayerAnimation.enabled = true;

        Time.timeScale = 1;
    }

    public async void EndGame()
    {
        isGameFinished = true;
        CurrentState = GameState.MainState;

        BGMAudio.Instance.PauseBGM();
        Time.timeScale = 0;

        //プレイヤーの子オブジェクトを非表示
        foreach (Transform obj in _Player.playerTransform)
        {
            obj.gameObject.SetActive(false);
        }

        //ゲームオーバー時に消したいCanvasを非表示
        for (int i = 0; i < gameOverNonActiveCanvasList.Count; i++)
        {
            gameOverNonActiveCanvasList[i].gameObject.SetActive(false);
        }

        //スコア(生存時間)を送信
        _RankingManager.SubmitScore();
        scoreText.text = $"生存時間[分:秒]\r\n{_Timer.minText.text}:{_Timer.secText.text}";

        //プレイヤーが死ぬアニメーションの処理を待つ
        await _Player._PlayerAnimation.PlayerDead();

        //ゲームオーバーのBGM再生
        BGMAudio.Instance.PlayBGM(gameOverSoundClip);
        BGMAudio.Instance.BGMAudioSource.loop = false;

        GameOverCanvas.SetActive(true);
    }

    public void SetBGMVolume(float value)
    {
        BGMAudio.Instance.BGMAudioSource.volume = 0.08f * value * 2;
    } 
    
    public void SetSEVolume(float value)
    {
        SEAudio.Instance.SEAudioSource.volume = 0.3f * value * 2;
    }

    public bool IsGameStarted() { return isGameStarted; }

    public bool IsGameFinished() { return isGameFinished; }
}
