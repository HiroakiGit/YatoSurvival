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

        //���N�G�X�g
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

        //���O�C��
        PlayFabClientAPI.LoginWithPlayFab(LoginRequest, OnLoginSuccess, OnError);
    }
    //===============================

    //�o�^�{�^�����������Ƃ�
    public void OnClickRegisterButton()
    {
        SUserName = IFUserName.text;
        SPassWord = IFPassWord.text;

        //���N�G�X�g
        var RegisterRequest = new RegisterPlayFabUserRequest()
        {
            TitleId = "455AB",
            Username = SUserName,
            Password = SPassWord,
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
        SUserName = IFUserName.text;
        SPassWord = IFPassWord.text;

        //���N�G�X�g
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

        //���O�C��
        PlayFabClientAPI.LoginWithPlayFab(LoginRequest, OnLoginSuccess, OnError);
    }

    //�f�B�X�v���C�l�[����ݒ�
    private void SetDisplayName()
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = SUserName
        }, result =>
        {
            SDisplayName = result.DisplayName;
            Debug.Log($"�j�b�N�l�[����:{SDisplayName}");
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
        _gameManager.OnLoginEnd();
    }

    //�G���[
    private void OnError(PlayFabError error)
    {
        SUserName = null;
        SPassWord = null;
        Debug.Log(error);
    }
}
