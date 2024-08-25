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

        //���N�G�X�g
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

        //���O�C��
        PlayFabClientAPI.LoginWithPlayFab(LoginRequest, OnLoginSuccess, OnError);
    }
    //======================================

    bool hide = true;
    //�p�X���[�h�m�F�{�^���������Ƃ�
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

    //�o�^�{�^�����������Ƃ�
    public void OnClickRegisterButton()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }

        if (OrganizedString(IFUserName.text).Length < 3 || OrganizedString(IFUserName.text).Length > 10) { ShowMessage("���[�U�[�l�[����3~10�����ɂ��Ă�������"); return; }
        if (OrganizedString(IFPassWord.text).Length < 6 || OrganizedString(IFPassWord.text).Length > 15) { ShowMessage("�p�X���[�h��6~15�����ɂ��Ă�������"); return; }

        _player.SUserName = OrganizedString(IFUserName.text);
        _player.SPassWord = OrganizedString(IFPassWord.text);

        //���N�G�X�g
        var RegisterRequest = new RegisterPlayFabUserRequest()
        {
            TitleId = "455AB",
            Username = _player.SUserName,
            Password = _player.SPassWord,
            RequireBothUsernameAndEmail = false
        };

        //�o�^
        PlayFabClientAPI.RegisterPlayFabUser(RegisterRequest, OnRegisterSuccess, OnError);
    }

    //�o�^����������
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        ShowMessage("�A�J�E���g��o�^���܂����I",false);
        //�������O�C��
        OnClickLoginButton();
    }

    //���O�C���{�^�����������Ƃ�
    public void OnClickLoginButton()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }

        if (OrganizedString(IFUserName.text).Length < 3 || OrganizedString(IFUserName.text).Length > 10) { ShowMessage("���[�U�[�l�[����3~10�����ɂ��Ă�������"); return; }
        if (OrganizedString(IFPassWord.text).Length < 6 || OrganizedString(IFPassWord.text).Length > 15) { ShowMessage("�p�X���[�h��6~15�����ɂ��Ă�������"); return; }

        _player.SUserName = OrganizedString(IFUserName.text);
        _player.SPassWord = OrganizedString(IFPassWord.text);

        //���N�G�X�g
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

        //���O�C��
        PlayFabClientAPI.LoginWithPlayFab(LoginRequest, OnLoginSuccess, OnError);
    }

    //�f�B�X�v���C�l�[����ݒ�
    private IEnumerator SetDisplayName()
    {
        yield return new WaitForSeconds(1);

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = _player.SUserName
        }, result =>
        {
            _player.SDisplayName = result.DisplayName;
            Debug.Log($"�j�b�N�l�[����:{_player.SDisplayName}");

            //�v���Z�X�I��
            EndLogin();

        }, error => OnError(error)) ;
    }

    //���O�C������������
    private void OnLoginSuccess(LoginResult result)
    {
        ShowMessage("���O�C���������܂����I", false);
        //DisplayName��ݒ�
        StartCoroutine(SetDisplayName());
    }

    //���O�C���I��
    private void EndLogin()
    {
        LoginCanvas.SetActive(false);
        //���O�C���I�����̏������Ăяo��
        GameManager.Instance.OnLoginEnd();
    }

    //�G���[===================================================================================
    private void OnError(PlayFabError error)
    {
        _player.SUserName = null;
        _player.SPassWord = null;
        Debug.Log(error.GenerateErrorReport());

        if (error.Error == PlayFabErrorCode.UsernameNotAvailable)
        {
            ShowMessage("���̃��[�U�[�l�[���͊��Ɏg���Ă��܂�");
        }
        if (error.Error == PlayFabErrorCode.AccountNotFound)
        {
            ShowMessage("�A�J�E���g��������܂���");
        }
        if (error.Error == PlayFabErrorCode.InvalidUsernameOrPassword)
        {
            ShowMessage("���[�U�[�l�[���܂��̓p�X���[�h���Ԉ���Ă��܂�");
        }
        if(error.Error == PlayFabErrorCode.APIClientRequestRateLimitExceeded)
        {
            ShowMessage("���΂炭���Ԃ������Ă�������");
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
