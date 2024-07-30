using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StrengtheningManager : MonoBehaviour
{
    public PlayerAttack _PlayerAttack;
    public QuestionManager _QuestionManager;
    public List<StrengtheningDetails> strengtheningDetailsList = new List<StrengtheningDetails>();
    private List<StrengtheningDetails> canSelectStrengtheningDetailsList = new List<StrengtheningDetails>();
    public int strengtheningLevel = 5;
    public int showCount;

    private List<StrengtheningDetails> selectedStrengtheningDetailsList = new List<StrengtheningDetails>();

    [Header("UI")]
    public GameObject StrengtheningCanvas;
    public Image[] weaponTypeImages;
    public Image[] infoImages;
    public Sprite[] infoSprites;
    public Text[] texts;

    private void Start()
    {
        StrengtheningCanvas.SetActive(false);
    }

    //既に存在する武器の強化ができるListを作成
    public void AddCanSelectStrengtheningDetails(WeaponType type)
    {
        for (int j = 0; j < strengtheningDetailsList.Count; j++)
        {
            if (strengtheningDetailsList[j].WeaponType == type)
            {
                // リストに要素が無ければ追加
                if (!canSelectStrengtheningDetailsList.Contains(strengtheningDetailsList[j]))
                {
                    canSelectStrengtheningDetailsList.Add(strengtheningDetailsList[j]);
                }
            }
        } 
    }

    //レベルアップしたとき
    public void StartStrengthening(int currentLevel)
    {
        //一定レベル間隔で呼び出す
        if (currentLevel % strengtheningLevel == 0)
        {
            StrengtheningCanvas.SetActive(true);
            GameManager.Instance.PauseGame();

            SelectStrengtheningDetails();
            ShowStrengtheningDetails();
        }
    }

    private void SelectStrengtheningDetails()
    {
        //それぞれのリストをランダムに並び替え(重複回避)
        canSelectStrengtheningDetailsList = canSelectStrengtheningDetailsList.OrderBy(x => Guid.NewGuid()).ToList();
        strengtheningDetailsList = strengtheningDetailsList.OrderBy(x => Guid.NewGuid()).ToList();

        //表示回数だけ表示内容を決定する
        for (int i = 0; i < showCount; i++)
        {
            int r = UnityEngine.Random.Range(0, 10);

            //表示回数のほうが今持ってる武器数より多いときは追加または強化またはレート上昇
            if (i >= canSelectStrengtheningDetailsList.Count)
            {
                int n = 0;

                //武器数が最大値に到達してるか確認
                if (_PlayerAttack.IsFullWeapon(strengtheningDetailsList[i].WeaponType))
                {
                    Debug.Log($"Full! : {strengtheningDetailsList[i].WeaponType}");
                    //レート上昇または強化に限る
                    n = UnityEngine.Random.Range(1, 3);
                }

                //追加または強化またはレート上昇
                AddDetail(strengtheningDetailsList, i, n);
                continue;
            }


            //追加、レート上昇、強化をランダムで決める
            if (r > 7)  //8,9
            {
                int n = 0;

                //武器数が最大値に到達してるか確認
                if (_PlayerAttack.IsFullWeapon(strengtheningDetailsList[i].WeaponType))
                {
                    Debug.Log($"Full! : {strengtheningDetailsList[i].WeaponType}");
                    //レート上昇または強化に限る
                    n = UnityEngine.Random.Range(1, 3);
                }

                //追加または強化またはレート上昇
                AddDetail(strengtheningDetailsList, i, n);
            }
            else if (r > 3) //4,5,6,7
            {
                //レート上昇
                AddDetail(canSelectStrengtheningDetailsList, i, 1);
            }
            else //0,1,2,3
            {
                //強化
                AddDetail(canSelectStrengtheningDetailsList, i, 2);
            }
        }
    }

    //selectedStrengtheningDetailsListに選ばれた要素を追加
    private void AddDetail(List<StrengtheningDetails> sourceList, int index, int state)
    {
        StrengtheningDetails a = new StrengtheningDetails
        {
            state = state,
            WeaponType = sourceList[index].WeaponType,
            Sprite = sourceList[index].Sprite,
            decreaseAttackIntervalRatio = sourceList[index].decreaseAttackIntervalRatio
        };
        selectedStrengtheningDetailsList.Add(a);
    }

    private void ShowStrengtheningDetails()
    {
        //表示
        for (int n = 0; n < showCount; n++)
        {
            weaponTypeImages[n].sprite = selectedStrengtheningDetailsList[n].Sprite;

            if (selectedStrengtheningDetailsList[n].state == 0)
            {
                texts[n].text = "追加";
                if (!_PlayerAttack.IsExistWeapon(selectedStrengtheningDetailsList[n].WeaponType))
                {
                    //New Weapon
                    infoImages[n].sprite = infoSprites[0];
                }
                else
                {
                    infoImages[n].sprite = infoSprites[1];
                }
            }
            else if (selectedStrengtheningDetailsList[n].state == 1)
            {
                texts[n].text = "レート上昇";
                infoImages[n].sprite = infoSprites[2];
            }
            else
            {
                texts[n].text = "強化";
                infoImages[n].sprite = infoSprites[3];
            }
        }
    }

    //ボタンを押したとき
    public void OnClickStrengtheningButton(int k)
    {
        StrengtheningCanvas.SetActive(false);

        var clickedData = selectedStrengtheningDetailsList[k];

        switch (clickedData.state)
        {
            case 0:
                //追加
                _PlayerAttack.AddWeapon(clickedData.WeaponType);
                LogManager.Instance.AddLogs($"{clickedData.WeaponType}を入手した！");
                break;
            case 1:
                //レート上昇
                _PlayerAttack.DecreaseAttackInterval(clickedData.WeaponType, clickedData.decreaseAttackIntervalRatio);
                LogManager.Instance.AddLogs($"{clickedData.WeaponType}のレートが上昇した！");
                break;
            case 2:
                //攻撃力増加
                LogManager.Instance.AddLogs($"{clickedData.WeaponType}の攻撃力が増加した！");
                break;
        }

        selectedStrengtheningDetailsList.Clear();

        LogManager.Instance.AddLogs($"まもなく問題が来る...");
        LogManager.Instance.AddLogs($"正解したらいいことあるかも！\n不正解だったら...");
        //FadeOut
        FadeUI.Instance.StartFadeOut(6.8f);

        //Log
        LogManager.Instance.Log(2, () =>
        {
            _QuestionManager.StartQuestion();
        }); 
    }
}
