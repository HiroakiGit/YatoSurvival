﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEditor.Experimental.GraphView;
using System.Threading.Tasks;
using Unity.VisualScripting;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LoginManager : MonoBehaviour
{
    public UserDataManager _UserDataManager;
    public Player _player;
    [SerializeField] GameObject LoginCanvas;
    [SerializeField] GameObject SetUserNameAndPassWordCanvas;
    [SerializeField] GameObject SetDisplayNameCanvas;
    [SerializeField] InputField IFUserName;
    [SerializeField] InputField IFPassWord;
    [SerializeField] InputField IFDisplayName;
    [SerializeField] GameObject CheckMarkObj;
    [SerializeField] Text ErrorText;
    [SerializeField] Button[] Buttons;
    private Coroutine currentErrorCoroutine;

    public void StartLogin()
    {
        LoginCanvas.SetActive(true);
        SetUserNameAndPassWordCanvas.SetActive(true);
        SetDisplayNameCanvas.SetActive(false);
        CheckMarkObj.SetActive(false);
        IFUserName.text = string.Empty;
        IFPassWord.text = string.Empty;
        IFDisplayName.text = string.Empty;
        ErrorText.text = string.Empty;
    }

    //AutoLogin===============================
    public void AutoLogin()
    {
        IFUserName.text = "testman";
        IFPassWord.text = "testman";
        OnClickLoginButton();
    }
    //======================================

    bool hide = true;
    //パスワード確認ボタン押したとき
    public void OnClickCheckPassWordButton()
    {
        hide = !hide;
        CheckMarkObj.SetActive(!hide);
        if (hide) IFPassWord.contentType = InputField.ContentType.Password;
        else IFPassWord.contentType = InputField.ContentType.Standard;
        StartCoroutine(ReloadInputField());
    }

    private IEnumerator ReloadInputField()
    {
        IFPassWord.ActivateInputField();
        yield return null;
        IFPassWord.MoveTextEnd(true);
    }

    //登録ボタンを押したとき
    public async void OnClickRegisterButton()
    {
        //ボタン無効化
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }

        //文字数が適切か
        if (OrganizedString(IFUserName.text).Length < 3 || OrganizedString(IFUserName.text).Length > 10) { ShowMessage("ユーザー名は3~10文字にしてください"); return; }
        if (OrganizedString(IFPassWord.text).Length < 6 || OrganizedString(IFPassWord.text).Length > 15) { ShowMessage("パスワードは6~15文字にしてください"); return; }

        //Load
        if(!PlaySetting.Instance.isPlayFabLogin) await _UserDataManager.LoadUserData();

        //UserName,Passwordを設定
        _player.SUserName = OrganizedString(IFUserName.text);
        _player.SPassWord = OrganizedString(IFPassWord.text);

        //PlayFabLoginかどうか
        if(PlaySetting.Instance.isPlayFabLogin)
        {
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
        else
        {
            //すでにその名前のプレイヤーがいる => Error
            if (await _UserDataManager.IsExistUserName(_player.SUserName))
            {
                ShowMessage($"登録失敗: ユーザー名が既に存在します");
            }
            //まだその名前のプレイヤーがいない => 登録成功
            else
            {
                await _UserDataManager.RegisterUser(_player.SUserName, _player.SPassWord);
                //登録完了
                OnRegisterSuccess(null);
            }
        }
    }

    //登録完了した時
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        ShowMessage($"登録成功: {_player.SUserName}", false);

        if (PlaySetting.Instance.isPlayFabLogin)
        {
            //自動ログイン
            OnClickLoginButton();
        }
        else
        {
            OnLoginSuccess(null);
        }
    }

    //ログインボタンを押したとき
    public async void OnClickLoginButton()
    {
        //ボタン無効化
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }
        
        //文字数が適切か
        if (OrganizedString(IFUserName.text).Length < 3 || OrganizedString(IFUserName.text).Length > 10) { ShowMessage("ユーザー名は3~10文字にしてください"); return; }
        if (OrganizedString(IFPassWord.text).Length < 6 || OrganizedString(IFPassWord.text).Length > 15) { ShowMessage("パスワードは6~15文字にしてください"); return; }

        //Load
        if (!PlaySetting.Instance.isPlayFabLogin) await _UserDataManager.LoadUserData();

        //UserName,Passwordを設定
        _player.SUserName = OrganizedString(IFUserName.text);
        _player.SPassWord = OrganizedString(IFPassWord.text);

        //PlayFabLoginかどうか
        if (PlaySetting.Instance.isPlayFabLogin)
        {
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
        else
        {
            //すでにその名前のプレイヤーがいる => ログイン成功
            if (await _UserDataManager.IsExistUserName(_player.SUserName))
            {
                if (await _UserDataManager.IsCorrectPassWord(_player.SUserName, _player.SPassWord))
                {
                    OnLoginSuccess(null);
                }
                else
                {
                    ShowMessage($"ログイン失敗: パスワードが間違っています");
                }
            }
            //その名前のプレイヤーがいない => ログイン失敗
            else
            {
                ShowMessage($"ログイン失敗: ユーザーが見つかりません");
            }
        }
    }

    //ログイン完了した時
    private void OnLoginSuccess(LoginResult result)
    {
        ShowMessage($"ログイン成功: {_player.SUserName}", false);
        StartCoroutine(StartSetDisplayName());
    }

    //DisplayNameを設定する処理を開始
    private IEnumerator StartSetDisplayName()
    {
        yield return new WaitForSeconds(2);

        //DisplayName設定
        //PlayFabLoginかどうか
        if (PlaySetting.Instance.isPlayFabLogin)
        {
            SetUserNameAndPassWordCanvas.SetActive(false);
            SetDisplayNameCanvas.SetActive(true);
        }
        else
        {
            SetDisplayName();
        }
    }

    //DisplayName決定ボタン押したとき
    public void OnClickSetDisplayNameButton()
    {
        //ボタン無効化
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }

        SetDisplayName();
    }

    //キャンセルボタン押したとき
    public void OnClickBackToSetNamePassButton()
    {
        //ボタン無効化
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }

        SetDisplayNameCanvas.SetActive(false);
        SetUserNameAndPassWordCanvas.SetActive(true);
    }

    //DisplayName設定
    private void SetDisplayName()
    {
        //PlayFabLoginかどうか
        if (PlaySetting.Instance.isPlayFabLogin)
        {
            //文字数が適切か
            if (OrganizedString(IFDisplayName.text).Length < 3 || OrganizedString(IFDisplayName.text).Length > 10) { ShowMessage("ニックネームは3~10文字にしてください"); return; }

            var DisplayNameRequest = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = IFDisplayName.text
            };

            //PlayFabのDisplayNameリクエスト
            PlayFabClientAPI.UpdateUserTitleDisplayName(DisplayNameRequest, OnSetDisplayNameEnd, OnError);
        }
        else
        {
            _player.SDisplayName = _player.SUserName;
            OnSetDisplayNameEnd(null);
        }
    }

    //DisplayNameの設定が完了した時
    private void OnSetDisplayNameEnd(UpdateUserTitleDisplayNameResult result)
    {
        if(result != null) _player.SDisplayName = result.DisplayName;
        StartCoroutine(EndSetDisplayNameEnd());
    }

    //DisplayName終了
    private IEnumerator EndSetDisplayNameEnd()
    {
        ShowMessage($"ニックネーム: {_player.SDisplayName}", false);
        yield return new WaitForSeconds(2);
        EndLogin();
    }

    //ログイン終了
    private void EndLogin()
    {
        LoginCanvas.SetActive(false);
        //ログイン終了時の処理を呼び出す
        GameManager.Instance.OnLoginEnd();
    }

    //エラー===================================================================================
    private void OnError(PlayFabError error)
    {
        _player.SUserName = null;
        _player.SPassWord = null;
        _player.SDisplayName = null;
        Debug.Log(error.GenerateErrorReport());

        if (error.Error == PlayFabErrorCode.UsernameNotAvailable)
        {
            ShowMessage("ユーザー名が既に存在します");
        }
        else if (error.Error == PlayFabErrorCode.AccountNotFound)
        {
            ShowMessage("ユーザーが見つかりません");
        }
        else if (error.Error == PlayFabErrorCode.InvalidUsernameOrPassword)
        {
            ShowMessage("ユーザーネームまたはパスワードが間違っています");
        }
        else if (error.Error == PlayFabErrorCode.InvalidParams)
        {
            ShowMessage("日本語や特定の記号は使用できません");
        }
        else if (error.Error == PlayFabErrorCode.NameNotAvailable)
        {
            ShowMessage("ニックネームが既に存在します");
        }
        else if (error.Error == PlayFabErrorCode.APIClientRequestRateLimitExceeded)
        {
            ShowMessage("しばらく時間をおいてください");
        }
        else
        {
            ShowMessage(string.Empty);
        }
    }

    //メッセージ================================================================================
    private void ShowMessage(string message, bool isError = true)
    {
        if (currentErrorCoroutine != null)
        {
            StopCoroutine(currentErrorCoroutine);
        }
        currentErrorCoroutine = StartCoroutine(MessageUIProcess(message, isError));
    }

    private IEnumerator MessageUIProcess(string m, bool isError = true)
    {
        if(isError) ErrorText.color = Color.red;
        else ErrorText.color = Color.green;

        ErrorText.text = m;

        yield return new WaitForSeconds(2);

        ErrorText.text = string.Empty;


        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = true;
        }
    }
    //======================================================================================

    private string OrganizedString(string str)
    {
        return str.Replace("\r", "").Replace("\n", "").Trim();
    }
}
