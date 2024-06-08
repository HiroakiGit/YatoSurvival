using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LoginManager _loginManager;
    public EnemyGenerater _enemyGenerater;
    public GameObject player;

    //GameManager����e�v���O�����ɏ��������肢���� => �e�v���O��������I���񍐂��󂯎��

    //��ԍŏ�
    private void Start()
    {
        //���O�C���J�n
        //_loginManager.AutoLogin();
        _loginManager.StartLogin();
    }

    //���O�C���I����
    public void OnLoginEnd()
    {
        _enemyGenerater.Generate();
    }
}
