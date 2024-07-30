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

    //������
    private void Start()
    {
        CurrentState = GameState.MainState;

        GameOverCanvas.SetActive(false);
        GamePauseCanvas.SetActive(false);

        // �X���C�_�[�̏����l���I�[�f�B�I�\�[�X�̉��ʂɐݒ�
        BGMVolumeSlider.value = 0.5f;
        BGMVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        
        SEVolumeSlider.value = 0.5f;
        SEVolumeSlider.onValueChanged.AddListener(SetSEVolume);

        //���O�C���J�n
        if (isAutoLogin) _loginManager.AutoLogin();
        else _loginManager.StartLogin();
    }

    //���O�C���I����
    public void OnLoginEnd()
    {
        //TODO: �J�n�܂�Delay�����
        StartGame();
    }

    //�Q�[���J�n
    public void StartGame()
    {
        isGameStarted = true;
        //�������퐶��
        _Player._PlayerAttack.GenerateInitialWeapon();
        //���ԑ���J�n
        _Timer.TimeCountStart();
    }

    private void Update()
    {
        //ESC�L�[���������Ƃ�
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

        //TimeScale�ɉe������Ȃ��X�N���v�g�ŁA��L���ɂ������v���O�������L���ɂ���
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

        //TimeScale�ɉe������Ȃ��X�N���v�g�ŁA��L���ɂ��Ă����v���O������L���ɂ���
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

        //�v���C���[�̎q�I�u�W�F�N�g���\��
        foreach (Transform obj in _Player.playerTransform)
        {
            obj.gameObject.SetActive(false);
        }

        //�Q�[���I�[�o�[���ɏ�������Canvas���\��
        for (int i = 0; i < gameOverNonActiveCanvasList.Count; i++)
        {
            gameOverNonActiveCanvasList[i].gameObject.SetActive(false);
        }

        //�X�R�A(��������)�𑗐M
        _RankingManager.SubmitScore();
        scoreText.text = $"��������[��:�b]\r\n{_Timer.minText.text}:{_Timer.secText.text}";

        //�v���C���[�����ʃA�j���[�V�����̏�����҂�
        await _Player._PlayerAnimation.PlayerDead();

        //�Q�[���I�[�o�[��BGM�Đ�
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
