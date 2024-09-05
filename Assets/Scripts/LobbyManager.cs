using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public LoadingScene _LoadingScene;
    [Header("UI")]
    public GameObject RankingButtonObj;
    public GameObject ReallyUI;
    public Text reallyText;

    private void Awake()
    {
        RankingButtonObj.SetActive(!PlaySetting.Instance.isPlayFabLogin);
        ReallyUI.SetActive(false);
    }

    //プレイボタンを押したとき
    public async void OnClickNextSceneButton()
    {
        await SectionDataManager.Instance.LoadSectionData();

        if (SectionDataManager.Instance.IsEndCurrentSection())
        {
            ReallyUI.SetActive(true);
            reallyText.text = $"{SectionDataManager.Instance.GetCurrentSectionName()}は終了しました。\nゲームのプレイは可能ですが、ランキングは更新されません。\nそれでもプレイしますか？";
        }
        else
        {
            _LoadingScene.LoadNextScene("GameScene");
        }
    }

    //プレイキャンセルボタンを押したとき
    public void OnClickCancelButton()
    {
        ReallyUI.SetActive(false);
    }
}
