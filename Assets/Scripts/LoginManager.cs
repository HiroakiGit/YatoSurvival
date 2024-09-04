using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using PlayFab;
using PlayFab.ClientModels;

public class LoginManager : MonoBehaviour
{
    public UserDataManager _UserDataManager;
    public Player _player;
    [SerializeField] GameObject LoginCanvas;
    [SerializeField] InputField IFUserName;
    [SerializeField] InputField IFPassWord;
    [SerializeField] GameObject CheckMarkObj;
    [SerializeField] Text ErrorText;
    [SerializeField] Button[] Buttons;
    private Coroutine currentErrorCoroutine;

    public void StartLogin()
    {
        LoginCanvas.SetActive(true);
        CheckMarkObj.SetActive(false);
        IFUserName.text = string.Empty;
        IFPassWord.text = string.Empty;
        ErrorText.text = string.Empty;
    }

    //AutoLogin===============================
    public void AutoLogin()
    {
        IFUserName.text = "testman";
        _player.SPassWord = "testman";
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
        await _UserDataManager.LoadUserData();

        //UserName,Passwordを設定
        _player.SUserName = OrganizedString(IFUserName.text);
        _player.SPassWord = OrganizedString(IFPassWord.text);

        //PlayFabLoginかどうか
        if(GameManager.Instance.isPlayFabLogin)
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

        if (GameManager.Instance.isPlayFabLogin)
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
        await _UserDataManager.LoadUserData();

        //UserName,Passwordを設定
        _player.SUserName = OrganizedString(IFUserName.text);
        _player.SPassWord = OrganizedString(IFPassWord.text);

        //PlayFabLoginかどうか
        if (GameManager.Instance.isPlayFabLogin)
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
        StartCoroutine(EndLogin());
    }

    //ログイン終了
    private IEnumerator EndLogin()
    {
        yield return new WaitForSecondsRealtime(2);
        LoginCanvas.SetActive(false);
        //ログイン終了時の処理を呼び出す
        GameManager.Instance.OnLoginEnd();
    }

    //エラー===================================================================================
    private void OnError(PlayFabError error)
    {
        _player.SUserName = null;
        _player.SPassWord = null;
        Debug.Log(error.GenerateErrorReport());

        if (error.Error == PlayFabErrorCode.UsernameNotAvailable)
        {
            ShowMessage("ユーザー名が既に存在します");
        }
        if (error.Error == PlayFabErrorCode.AccountNotFound)
        {
            ShowMessage("ユーザーが見つかりません");
        }
        if (error.Error == PlayFabErrorCode.InvalidUsernameOrPassword)
        {
            ShowMessage("ユーザーネームまたはパスワードが間違っています");
        }
        if (error.Error == PlayFabErrorCode.APIClientRequestRateLimitExceeded)
        {
            ShowMessage("しばらく時間をおいてください");
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

        if (isError)
        {
            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i].interactable = true;
            }
        }
    }
    //======================================================================================

    private string OrganizedString(string str)
    {
        return str.Replace("\r", "").Replace("\n", "").Trim();
    }
}
