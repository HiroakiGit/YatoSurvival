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
        _player.SUserName = IFUserName.text;
        _player.SPassWord = IFPassWord.text;

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
        Debug.Log("PlayFab�A�J�E���g��o�^���܂����I");
        //DisplayName��ݒ�
        SetDisplayName();
        //�������O�C��
        OnClickLoginButton();
    }

    //���O�C���{�^�����������Ƃ�
    public void OnClickLoginButton()
    {
        _player.SUserName = IFUserName.text;
        _player.SPassWord = IFPassWord.text;

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
    private void SetDisplayName()
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = _player.SUserName
        }, result =>
        {
            _player.SDisplayName = result.DisplayName;
            Debug.Log($"�j�b�N�l�[����:{_player.SDisplayName}");
        }, error => OnError(error)) ;
    }

    //���O�C������������
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("PlayFab�A�J�E���g�Ń��O�C�����܂����I");
        //DisplayName��ݒ�
        SetDisplayName();
        //�v���Z�X�I��
        EndLogin();
    }

    //���O�C���I��
    private void EndLogin()
    {
        LoginCanvas.SetActive(false);
        //���O�C���I�����̏������Ăяo��
        GameManager.Instance.OnLoginEnd();
    }

    //�G���[
    private void OnError(PlayFabError error)
    {
        _player.SUserName = null;
        _player.SPassWord = null;
        Debug.Log(error);
    }
}
