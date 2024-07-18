using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StrengtheningAndAddWeapon : MonoBehaviour
{
    public PlayerAttack _PlayerAttack;
    public GameObject StrengtheningAndAddWeaponAndProblemCanvas;
    public List<StrengtheningAndAddWeaponDetails> strengtheningAndAddWeaponDetails = new List<StrengtheningAndAddWeaponDetails>();
    public List<StrengtheningAndAddWeaponDetails> canSelectStrengtheningDetails = new List<StrengtheningAndAddWeaponDetails>();
    public int addWeaponLevel = 5;
    public int showCount;
    public Image[] weaponTypeImages;
    public Image[] infoImages;
    public Sprite[] infoSprites;
    public Text[] texts;
    public ButtonSpriteChanger[] buttonSpriteChangers;

    public List<StrengtheningAndAddWeaponDetails> selectedStrengtheningAndAddWeaponDetails = new List<StrengtheningAndAddWeaponDetails>();

    private void Start()
    {
        StrengtheningAndAddWeaponAndProblemCanvas.SetActive(false);
    }

    //既に存在する武器の強化ができるListを作成
    public void AddCanSelectStrengtheningDetails(WeaponType type)
    {
        for (int j = 0; j < strengtheningAndAddWeaponDetails.Count; j++)
        {
            if (strengtheningAndAddWeaponDetails[j]._WeaponType == type)
            {
                // リストに要素が無ければ追加
                if (!canSelectStrengtheningDetails.Contains(strengtheningAndAddWeaponDetails[j]))
                {
                    canSelectStrengtheningDetails.Add(strengtheningAndAddWeaponDetails[j]);
                }
            }
        } 
    }

    public void SelectAndShowStrengtheningAndAddWeaponDetails(int currentLevel)
    {
        if (currentLevel % addWeaponLevel == 0)
        {
            StrengtheningAndAddWeaponAndProblemCanvas.SetActive(true);
            GameManager.Instance.PauseGame();

            canSelectStrengtheningDetails = canSelectStrengtheningDetails.OrderBy(x => Guid.NewGuid()).ToList();
            strengtheningAndAddWeaponDetails = strengtheningAndAddWeaponDetails.OrderBy(x => Guid.NewGuid()).ToList();

            for (int i = 0; i < showCount; i++)
            {
                int r = UnityEngine.Random.Range(0, 3);

                if (i >= canSelectStrengtheningDetails.Count)
                {
                    AddDetail(strengtheningAndAddWeaponDetails, i, 0);
                    continue;
                }

                switch (r)
                {
                    case 0:
                        AddDetail(strengtheningAndAddWeaponDetails, i, 0);
                        break;
                    case 1:
                        AddDetail(canSelectStrengtheningDetails, i, 1);
                        break;
                    case 2:
                        AddDetail(canSelectStrengtheningDetails, i, 2);
                        break;
                }
            }

            for (int i = 0;i < buttonSpriteChangers.Length; i++)
            {
                buttonSpriteChangers[i].Reset();
            }

            //表示
            for (int n = 0; n < showCount; n++)
            {
                weaponTypeImages[n].sprite = selectedStrengtheningAndAddWeaponDetails[n]._Sprite;

                if (selectedStrengtheningAndAddWeaponDetails[n].state == 0)
                {
                    texts[n].text = "追加";
                    infoImages[n].sprite = infoSprites[0];
                }
                else if (selectedStrengtheningAndAddWeaponDetails[n].state == 1)
                {
                    texts[n].text = "レート上昇";
                    infoImages[n].sprite = infoSprites[1];
                }
                else 
                {
                    texts[n].text = "強化";
                    infoImages[n].sprite = infoSprites[2];
                }
            }
        }
    }

    private void AddDetail(List<StrengtheningAndAddWeaponDetails> sourceList, int index, int state)
    {
        StrengtheningAndAddWeaponDetails a = new StrengtheningAndAddWeaponDetails
        {
            state = state,
            _WeaponType = sourceList[index]._WeaponType,
            _Sprite = sourceList[index]._Sprite,
            decreaseAttackIntervalRatio = sourceList[index].decreaseAttackIntervalRatio
        };
        selectedStrengtheningAndAddWeaponDetails.Add(a);
    }

    public void OnClickStrengtheningAndAddWeaponButton(int k)
    {
        var clickedData = selectedStrengtheningAndAddWeaponDetails[k];

        switch (clickedData.state)
        {
            case 0:
                _PlayerAttack.AddWeapon(clickedData._WeaponType);
                break;
            case 1:
                _PlayerAttack.DecreaseAttackInterval(clickedData._WeaponType, clickedData.decreaseAttackIntervalRatio);
                break;
            case 2:
                break;
        }

        StrengtheningAndAddWeaponAndProblemCanvas.SetActive(false);
        selectedStrengtheningAndAddWeaponDetails.Clear();
        GameManager.Instance.ContinueGame();
    }
}
