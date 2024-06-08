using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public GameManager _gameManager;
    [SerializeField] GameObject LoginCanvas;
    [SerializeField] InputField IFUserName;
    [SerializeField] InputField IFPassWord;
    public string SUserName;
    public string SPassWord;
    public string SDisplayName;

    public void StartLogin()
    {
        LoginCanvas.SetActive(true);
    }

    //===============================
    public void AutoLogin()
    {
        SUserName = "testman";
        SPassWord = "testman";

        //リクエスト
        var LoginRequest = new LoginWithPlayFabRequest()
        {
            TitleId = "455AB",
            Username = SUserName,
            Password = SPassWord,

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
        SUserName = IFUserName.text;
        SPassWord = IFPassWord.text;

        //リクエスト
        var RegisterRequest = new RegisterPlayFabUserRequest()
        {
            TitleId = "455AB",
            Username = SUserName,
            Password = SPassWord,
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
        SUserName = IFUserName.text;
        SPassWord = IFPassWord.text;

        //リクエスト
        var LoginRequest = new LoginWithPlayFabRequest()
        {
            TitleId = "455AB",
            Username = SUserName,
            Password = SPassWord,

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
            DisplayName = SUserName
        }, result =>
        {
            SDisplayName = result.DisplayName;
            Debug.Log($"ニックネームは:{SDisplayName}");
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
        _gameManager.OnLoginEnd();
    }

    //エラー
    private void OnError(PlayFabError error)
    {
        SUserName = null;
        SPassWord = null;
        Debug.Log(error);
    }
}
