using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public Player _player;
    [SerializeField] GameObject LoginCanvas;
    [SerializeField] InputField IFUserName;
    [SerializeField] InputField IFPassWord;

    public void StartLogin()
    {
        LoginCanvas.SetActive(true);
    }

    //===============================
    public void AutoLogin()
    {
        _player.SUserName = "testman";
        _player.SPassWord = "testman";

        //リクエスト
        var LoginRequest = new LoginWithPlayFabRequest()
        {
            TitleId = "455AB",
            Username = _player.SUserName,
            Password = _player.SPassWord,

            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        //ログイン
        PlayFabClientAPI.LoginWithPlayFab(LoginRequest, OnLoginSuccess, OnError);
    }
    //===============================

    //登録ボタンを押したとき
    public void OnClickRegisterButton()
    {
        _player.SUserName = IFUserName.text;
        _player.SPassWord = IFPassWord.text;

        //リクエスト
        var RegisterRequest = new RegisterPlayFabUserRequest()
        {
            TitleId = "455AB",
            Username = _player.SUserName,
            Password = _player.SPassWord,
            RequireBothUsernameAndEmail = false
        };

        //登録
        PlayFabClientAPI.RegisterPlayFabUser(RegisterRequest, OnRegisterSuccess, OnError);
    }

    //登録完了した時
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("PlayFabアカウントを登録しました！");
        //DisplayNameを設定
        SetDisplayName();
        //自動ログイン
        OnClickLoginButton();
    }

    //ログインボタンを押したとき
    public void OnClickLoginButton()
    {
        _player.SUserName = IFUserName.text;
        _player.SPassWord = IFPassWord.text;

        //リクエスト
        var LoginRequest = new LoginWithPlayFabRequest()
        {
            TitleId = "455AB",
            Username = _player.SUserName,
            Password = _player.SPassWord,

            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        //ログイン
        PlayFabClientAPI.LoginWithPlayFab(LoginRequest, OnLoginSuccess, OnError);
    }

    //ディスプレイネームを設定
    private void SetDisplayName()
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = _player.SUserName
        }, result =>
        {
            _player.SDisplayName = result.DisplayName;
            Debug.Log($"ニックネームは:{_player.SDisplayName}");
        }, error => OnError(error)) ;
    }

    //ログイン完了した時
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("PlayFabアカウントでログインしました！");
        //DisplayNameを設定
        SetDisplayName();
        //プロセス終了
        EndLogin();
    }

    //ログイン終了
    private void EndLogin()
    {
        LoginCanvas.SetActive(false);
        //ログイン終了時の処理を呼び出す
        GameManager.Instance.OnLoginEnd();
    }

    //エラー
    private void OnError(PlayFabError error)
    {
        _player.SUserName = null;
        _player.SPassWord = null;
        Debug.Log(error);
    }
}
