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
    [SerializeField] Text ErrorText;
    [SerializeField] Button[] Buttons;
    private Coroutine currentErrorCoroutine;

    public void StartLogin()
    {
        LoginCanvas.SetActive(true);
        IFUserName.text = string.Empty;
        IFPassWord.text = string.Empty;
        ErrorText.text = string.Empty;
    }

    //===============================
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
    //===============================

    //�o�^�{�^�����������Ƃ�
    public void OnClickRegisterButton()
    {
        if (OrganizedString(IFUserName.text).Length < 3 || OrganizedString(IFUserName.text).Length > 10) { ShowMessage("���[�U�[�l�[����3~10�����ɂ��Ă�������", Color.red); return; }
        if (OrganizedString(IFPassWord.text).Length < 6 || OrganizedString(IFPassWord.text).Length > 15) { ShowMessage("�p�X���[�h��6~15�����ɂ��Ă�������", Color.red); return; }

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
        ShowMessage("�A�J�E���g��o�^���܂����I",Color.green);
        //�������O�C��
        OnClickLoginButton();
    }

    //���O�C���{�^�����������Ƃ�
    public void OnClickLoginButton()
    {
        if (OrganizedString(IFUserName.text).Length < 3 || OrganizedString(IFUserName.text).Length > 10) { ShowMessage("���[�U�[�l�[����3~10�����ɂ��Ă�������", Color.red); return; }
        if (OrganizedString(IFPassWord.text).Length < 6 || OrganizedString(IFPassWord.text).Length > 15) { ShowMessage("�p�X���[�h��6~15�����ɂ��Ă�������", Color.red); return; }

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
        ShowMessage("���O�C���������܂����I", Color.green);
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
            ShowMessage("���̃��[�U�[�l�[���͊��Ɏg���Ă��܂�", Color.red);
        }
        if (error.Error == PlayFabErrorCode.AccountNotFound)
        {
            ShowMessage("�A�J�E���g��������܂���", Color.red);
        }
        if (error.Error == PlayFabErrorCode.InvalidUsernameOrPassword)
        {
            ShowMessage("���[�U�[�l�[���܂��̓p�X���[�h���Ԉ���Ă��܂�", Color.red);
        }
        if(error.Error == PlayFabErrorCode.APIClientRequestRateLimitExceeded)
        {
            ShowMessage("���΂炭���Ԃ������Ă�������", Color.red);
        }
    }

    private void ShowMessage(string message, Color color)
    {
        if (currentErrorCoroutine != null)
        {
            StopCoroutine(currentErrorCoroutine);
        }
        currentErrorCoroutine = StartCoroutine(MessageUIProcess(message, color));
    }

    private IEnumerator MessageUIProcess(string m, Color c)
    {
        for (int i = 0; i < Buttons.Length; i++) 
        {
            Buttons[i].interactable = false;
        }
        ErrorText.color = c;
        ErrorText.text = m;

        yield return new WaitForSeconds(2);

        ErrorText.text = string.Empty;
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = true;
        }
    }

    //=======================================================================================

    private string OrganizedString(string str)
    {
        return str.Replace("\r", "").Replace("\n", "").Trim();
    }
}
