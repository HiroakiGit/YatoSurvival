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
    public BuffAndDeBuffManager _BuffAndDeBuffManager;
    public RankingManager _RankingManager;
    public LoadingScene _LoadingScene;

    public bool isAutoLogin;
    private bool isGameStarted = false;
    private bool isGameFinished = false;

    [Header("UI")]
    public GameState CurrentState;
    public GameObject ReallyUI;
    public Text ReallyText;
    public AudioClip pushedButtonSoundClip;
    public bool isProcessing = false;

    [Header("GameStart")]
    public GameObject ExplainAndGameStartingCanvas;
    public Text startingText;
    public Text countDownText;

    [Header("GamePause")]
    public GameObject GamePauseCanvas;
    public AudioClip pauseBGMSoundClip;

    [Header("GameOver")]
    public GameObject GameOverCanvas;
    public Text scoreText;
    public List<GameObject> gameOverNonActiveCanvasList = new List<GameObject>();
    public AudioClip gameOverSoundClip;

    [Header("GameSetting")]
    public GameObject BGMMuteImageObj;
    public GameObject SEMuteImageObj;
    public Slider BGMVolumeSlider;
    public Slider SEVolumeSlider;

    //GameManager����e�v���O�����ɏ��������肢���� => �e�v���O��������I���񍐂��󂯎��

    void Awake()
    {
        // �V���O���g���p�^�[���̎���
        if (Instance == null)
        {
            Instance = this;
        }
    }

    //������
    private void Start()
    {
        Time.timeScale = 1;

        CurrentState = GameState.MainState;

        GameOverCanvas.SetActive(false);
        GamePauseCanvas.SetActive(false);
        BGMMuteImageObj.SetActive(false);
        SEMuteImageObj.SetActive(false);

        ReallyUI.SetActive(false);

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
        StartCoroutine(ExplainAndStartingGame());
    }

    //�����ƃJ�E���g�_�E��
    public IEnumerator ExplainAndStartingGame()
    {
        ExplainAndGameStartingCanvas.SetActive(true);
        _BuffAndDeBuffManager.BuffStateCanvas.SetActive(false);

        for (int i = 1; i >= 0; i--)
        {
            countDownText.text = i.ToString();
            
            if(i <= 0)
            {
                startingText.text = "�X�^�[�g�I";
                countDownText.text = string.Empty;
            }

            yield return new WaitForSeconds(1);
        }

        ExplainAndGameStartingCanvas.SetActive(false);
        _BuffAndDeBuffManager.BuffStateCanvas.SetActive(true);

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
        //�o�t�f�o�t�̊ȈՕ\��������
        for (int i = 0; i < _BuffAndDeBuffManager.weaponBuffList.Count; i++)
        {
            _BuffAndDeBuffManager.weaponBuffList[i].Initalize(_BuffAndDeBuffManager._PlayerAttack.WeaponCount(_BuffAndDeBuffManager.weaponBuffList[i].WeaponType));
        }
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
        if (isProcessing) return;

        switch (CurrentState)
        {
            case GameState.MainState:

                if (isGameFinished) return;
                PauseGame(true);
                SEAudio.Instance.PlayOneShot(pushedButtonSoundClip, 0.1f);
                break;

            case GameState.PauseState:

                if (isGameFinished) return;
                ContinueGame();
                SEAudio.Instance.PlayOneShot(pushedButtonSoundClip, 0.1f);
                break;

            case GameState.SubMenuState:

                CloseSubMenu();
                SEAudio.Instance.PlayOneShot(pushedButtonSoundClip, 0.1f);
                break;
        }
    }

    public void PauseGame(bool canvasActive)
    {
        GamePauseCanvas.SetActive(canvasActive);
        CurrentState = GameState.PauseState;

        //TimeScale�ɉe������Ȃ��X�N���v�g�ŁA��L���ɂ������v���O�������L���ɂ���
        _Player._PlayerAttackIndicator.enabled = false;
        _Player._PlayerAnimation.enabled = false;

        if(canvasActive) BGMAudio.Instance.PlayBGM(pauseBGMSoundClip);

        Time.timeScale = 0;
    }

    private void CloseSubMenu()
    {
        _RankingManager.OnClickBackButton();
        ReallyUI.SetActive(false);
        CurrentState = GameState.PauseState;
    }

    public void ContinueGame()
    {
        GamePauseCanvas.SetActive(false);
        CurrentState = GameState.MainState;

        //TimeScale�ɉe������Ȃ��X�N���v�g�ŁA��L���ɂ��Ă����v���O������L���ɂ���
        _Player._PlayerAttackIndicator.enabled = true;
        _Player._PlayerAnimation.enabled = true;

        BGMAudio.Instance.PlayBGM(null);

        Time.timeScale = 1;
    }

    public async void GameOver()
    {
        isGameFinished = true;
        CurrentState = GameState.PauseState;

        BGMAudio.Instance.Stop();
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

    public void OnClickResumeButton()
    {
        HandleEscapePress();
    }

    public void OnClickRetryButton()
    {
        ReallyText.text = "�{���Ƀ��g���C���܂����H";
        ReallyUI.SetActive(true);
        CurrentState = GameState.SubMenuState;
    }
    
    public void OnClickBackTitleButton()
    {
        ReallyText.text = "�{���Ƀ^�C�g���ɖ߂�܂����H";
        ReallyUI.SetActive(true);
        CurrentState = GameState.SubMenuState;
    }
    
    public void OnClickCancelButton()
    {
        HandleEscapePress();
    }

    public void OnClickNextSceneButton()
    {
        if (ReallyText.text.Contains("���g���C"))
        {
            _LoadingScene.LoadNextScene("GameScene");
        }
        else if (ReallyText.text.Contains("�^�C�g��"))
        {
            _LoadingScene.LoadNextScene("LobbyScene");
        }
    }

    bool isMuteBGM = false;
    public void OnClickMuteBGMVolume()
    {
        isMuteBGM = !isMuteBGM;
        if (isMuteBGM) 
        {
            BGMAudio.Instance.BGMAudioSource.mute = true;
            BGMMuteImageObj.SetActive(true);
        }
        else
        {
            BGMAudio.Instance.BGMAudioSource.mute = false;
            BGMMuteImageObj.SetActive(false);
        }
    }
    
    bool isMuteSE = false;
    public void OnClickMuteSEVolume()
    {
        isMuteSE = !isMuteSE;
        if (isMuteSE)
        {
            SEAudio.Instance.SEAudioSource.mute = true;
            SEMuteImageObj.SetActive(true);
        }
        else
        {
            SEAudio.Instance.SEAudioSource.mute = false;
            SEMuteImageObj.SetActive(false);
        }
    }

    public void SetBGMVolume(float value)
    {
        BGMAudio.Instance.BGMAudioSource.volume = 0.08f * value * 2;

        if (value <= 0)
        {
            isMuteBGM = true;
            BGMAudio.Instance.BGMAudioSource.mute = true;
            BGMMuteImageObj.SetActive(true);
        }
        else
        {
            isMuteBGM = false;
            BGMAudio.Instance.BGMAudioSource.mute = false;
            BGMMuteImageObj.SetActive(false);
        }
    } 
    
    public void SetSEVolume(float value)
    {
        SEAudio.Instance.SEAudioSource.volume = 0.3f * value * 2;

        if (value <= 0)
        {
            isMuteSE = true;
            SEAudio.Instance.SEAudioSource.mute = true;
            SEMuteImageObj.SetActive(true);
        }
        else
        {
            isMuteSE = false;
            SEAudio.Instance.SEAudioSource.mute = false;
            SEMuteImageObj.SetActive(false);
        }
    }

    public bool IsGameStarted() { return isGameStarted; }

    public bool IsGameFinished() { return isGameFinished; }
}
