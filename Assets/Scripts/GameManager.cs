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

    private bool isGameStarted = false;
    private bool isGameFinished = false;

    [Header("TestPlay")]
    public bool isAutoLogin;
    public bool isExplain;
    public bool isCountDown;

    [Header("===UI===")]
    public GameState CurrentState;
    public GameObject ReallyUI;
    public Text ReallyText;
    public AudioClip pushedButtonSoundClip;
    public bool isProcessing = false;

    [Header("Explain")]
    public GameObject ExplainCanvas;
    public GameObject[] ExplainContents;
    public Image[] stateImages;
    private int state;

    [Header("GameStart")]
    public GameObject GameStartingCanvas;
    public Text startingText;
    public Text countDownText;
    public int startCount = 3;
    private AudioClip[] startSoundClips;

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
        Application.targetFrameRate = 60;

        CurrentState = GameState.MainState;

        ExplainCanvas.SetActive(false);
        foreach(GameObject obj in ExplainContents)
        {
            obj.SetActive(false);
        }
        state = 0;
        ChangeState(0,false);

        GamePauseCanvas.SetActive(false);
        GameStartingCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
        BGMMuteImageObj.SetActive(false);
        SEMuteImageObj.SetActive(false);

        ReallyUI.SetActive(false);

        // �X���C�_�[�̏����l���I�[�f�B�I�\�[�X�̉��ʂɐݒ�
        BGMVolumeSlider.value = 0.5f;
        BGMVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        
        SEVolumeSlider.value = 0.5f;
        SEVolumeSlider.onValueChanged.AddListener(SetSEVolume);

        // �N���b�v����x�����������ĕۑ�
        startSoundClips = new AudioClip[]
        {
            SoundClipCreator.Instance.CreateClip(440f, 440f, 0.15f, false),
            SoundClipCreator.Instance.CreateClip(440f, 440f, 0.15f, false),
            SoundClipCreator.Instance.CreateClip(440f, 440f, 0.15f, false),
            SoundClipCreator.Instance.CreateClip(880f, 880f, 0.5f, false),
        };

        //���O�C���J�n
        if (isAutoLogin) _loginManager.AutoLogin();
        else _loginManager.StartLogin();
    }

    //���O�C���I����
    public void OnLoginEnd()
    {
        if (isExplain)
        {
            StartExplain();
        }
        else
        {
            StartCoroutine(StartingGame());
        }
    }

    //�����J�n
    private void StartExplain()
    {
        ExplainCanvas.SetActive(true);
    }

    //���ւ̃{�^��������
    public void OnClickNextExplainButton(int nextstate)
    {
        ChangeState(nextstate, true);
    }
    
    public void OnClickChangeExplainButton(int state)
    {
        ChangeState(state, false);
    }

    private void ChangeState(int state, bool isNext)
    {
        if(isNext) this.state = this.state + state;
        else this.state = state;
       
        if (this.state < 0) this.state = ExplainContents.Length - 1;
        if(this.state > ExplainContents.Length - 1) this.state = 0;

        for(int i = 0; i < ExplainContents.Length; i++)
        {
            if(this.state == i)
            {
                ExplainContents[i].SetActive(true);
                stateImages[i].color = Color.cyan;
            }
            else
            {
                ExplainContents[i].SetActive(false);
                stateImages[i].color = Color.white;
            }
        }
    }

    //�����I���{�^����������
    public void OnClickExplainDoneButton()
    {
        ExplainCanvas.SetActive(false);
        StartCoroutine(StartingGame());
    }

    //�J�E���g�_�E���J�n
    public IEnumerator StartingGame()
    {
        yield return new WaitForSeconds(1);

        GameStartingCanvas.SetActive(true);
        _BuffAndDeBuffManager.BuffAndDeBuffStateCanvas.SetActive(false);

        //�J�E���g�_�E��
        if (isCountDown)
        {
            for (int i = startCount; i >= 0; i--)
            {
                countDownText.text = i.ToString();

                if (i <= 0)
                {
                    startingText.text = "�X�^�[�g�I";
                    countDownText.text = string.Empty;
                }

                SEAudio.Instance.PlayOneShot(startSoundClips[startCount - i], 0.1f);

                yield return new WaitForSeconds(1);
            }
        }

        GameStartingCanvas.SetActive(false);
        _BuffAndDeBuffManager.BuffAndDeBuffStateCanvas.SetActive(true);

        StartGame();
    }

    //�Q�[���J�n
    public void StartGame()
    {
        isGameStarted = true;
        //BGM
        BGMAudio.Instance.PlayBGM(null, true);
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
        if (isProcessing || !IsGameStarted()) return;

        switch (CurrentState)
        {
            case GameState.MainState:

                if (isGameFinished) return;
                PauseGame(true, false);
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

    public void PauseGame(bool canvasActive, bool continuePlayMainBGM)
    {
        GamePauseCanvas.SetActive(canvasActive);
        CurrentState = GameState.PauseState;

        //TimeScale�ɉe������Ȃ��X�N���v�g�ŁA��L���ɂ������v���O�������L���ɂ���
        _Player._PlayerAttackIndicator.enabled = false;
        _Player._PlayerAnimation.enabled = false;

        if (canvasActive) BGMAudio.Instance.PlayBGM(pauseBGMSoundClip, true);
        if(!canvasActive && !continuePlayMainBGM) BGMAudio.Instance.Stop();

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

        BGMAudio.Instance.PlayBGM(null, true);

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
        BGMAudio.Instance.PlayBGM(gameOverSoundClip, false);

        GameOverCanvas.SetActive(true);
    }

    public void OnClickResumeButton()
    {
        HandleEscapePress();
    }

    public void OnClickRetryButton()
    {
        if(!IsGameFinished()) ReallyText.text = "�{���Ƀ��g���C���܂����H\n�����݂̐i���͂Ȃ��Ȃ�܂�";
        else ReallyText.text = "�{���Ƀ��g���C���܂����H";
        ReallyUI.SetActive(true);
        CurrentState = GameState.SubMenuState;
    }
    
    public void OnClickBackTitleButton()
    {
        if (!IsGameFinished()) ReallyText.text = "�{���Ƀ^�C�g���ɖ߂�܂����H\n�����݂̐i���͂Ȃ��Ȃ�܂�";
        else ReallyText.text = "�{���Ƀ^�C�g���ɖ߂�܂����H";
        ReallyUI.SetActive(true);
        CurrentState = GameState.SubMenuState;
    }
    
    public void OnClickCancelButton()
    {
        HandleEscapePress();
        ReallyUI.SetActive(false);
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
        BGMAudio.Instance.BGMAudioSource.volume = 0.05f * value * 2;

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
