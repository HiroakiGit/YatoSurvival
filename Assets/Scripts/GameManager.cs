using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public LoginManager _loginManager;
    public MapEdgeEnemySpawner _enemyGenerater;
    public Player _Player;

    public bool isAutoLogin;
    private bool isGameStarted = false;

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
        if(isAutoLogin)_loginManager.AutoLogin();
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
    }

    public bool IsGameStarted()
    {
        return isGameStarted;
    }
}
