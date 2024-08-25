using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System.Threading.Tasks;

public class LoginManager : MonoBehaviour
{
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
    //======================================

    bool hide = true;
    //パスワード確認ボタン押したとき
    public void OnClickCheckPassWordButton()
    {
        hide = !hide;
        CheckMarkObj.SetActive(!hide);
        if(hide) IFPassWord.contentType = InputField.ContentType.Password;
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
    public void OnClickRegisterButton()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }

        if (OrganizedString(IFUserName.text).Length < 3 || OrganizedString(IFUserName.text).Length > 10) { ShowMessage("ユーザーネームは3~10文字にしてください"); return; }
        if (OrganizedString(IFPassWord.text).Length < 6 || OrganizedString(IFPassWord.text).Length > 15) { ShowMessage("パスワードは6~15文字にしてください"); return; }

        _player.SUserName = OrganizedString(IFUserName.text);
        _player.SPassWord = OrganizedString(IFPassWord.text);

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
        ShowMessage("アカウントを登録しました！",false);
        //自動ログイン
        OnClickLoginButton();
    }

    //ログインボタンを押したとき
    public void OnClickLoginButton()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }

        if (OrganizedString(IFUserName.text).Length < 3 || OrganizedString(IFUserName.text).Length > 10) { ShowMessage("ユーザーネームは3~10文字にしてください"); return; }
        if (OrganizedString(IFPassWord.text).Length < 6 || OrganizedString(IFPassWord.text).Length > 15) { ShowMessage("パスワードは6~15文字にしてください"); return; }

        _player.SUserName = OrganizedString(IFUserName.text);
        _player.SPassWord = OrganizedString(IFPassWord.text);

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
    private IEnumerator SetDisplayName()
    {
        yield return new WaitForSeconds(1);

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = _player.SUserName
        }, result =>
        {
            _player.SDisplayName = result.DisplayName;
            Debug.Log($"ニックネームは:{_player.SDisplayName}");

            //プロセス終了
            EndLogin();

        }, error => OnError(error)) ;
    }

    //ログイン完了した時
    private void OnLoginSuccess(LoginResult result)
    {
        ShowMessage("ログイン成功しました！", false);
        //DisplayNameを設定
        StartCoroutine(SetDisplayName());
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
        Debug.Log(error.GenerateErrorReport());

        if (error.Error == PlayFabErrorCode.UsernameNotAvailable)
        {
            ShowMessage("このユーザーネームは既に使われています");
        }
        if (error.Error == PlayFabErrorCode.AccountNotFound)
        {
            ShowMessage("アカウントが見つかりません");
        }
        if (error.Error == PlayFabErrorCode.InvalidUsernameOrPassword)
        {
            ShowMessage("ユーザーネームまたはパスワードが間違っています");
        }
        if(error.Error == PlayFabErrorCode.APIClientRequestRateLimitExceeded)
        {
            ShowMessage("しばらく時間をおいてください");
        }
    }

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

    //=======================================================================================

    private string OrganizedString(string str)
    {
        return str.Replace("\r", "").Replace("\n", "").Trim();
    }
}
