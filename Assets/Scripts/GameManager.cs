using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LoginManager _loginManager;
    public MapEdgeEnemySpawner _enemyGenerater;

    //GameManagerから各プログラムに処理をお願いする => 各プログラムから終了報告を受け取る

    //一番最初
    private void Start()
    {
        //ログイン開始
        _loginManager.AutoLogin();
        //_loginManager.StartLogin();
    }

    //ログイン終了時
    public void OnLoginEnd()
    {
        //_enemyGenerater.Generate();
    }
}
