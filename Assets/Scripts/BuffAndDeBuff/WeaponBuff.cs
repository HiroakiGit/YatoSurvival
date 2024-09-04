using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WeaponBuff
{
    public string WeaponName;
    [Multiline(2)]public string WeaponExplain;
    public WeaponType WeaponType;
    public Sprite Sprite;
    public int state;
    public float increaseRate;
    public float increaseDamage;

    public Image showWeaponTypeImage;
    public Image[] showStateImage;
    public Text[] showStateCountText;

    [HideInInspector]public int increaseRateCount;
    [HideInInspector]public int increaseDamageCount;

    public void BuffStateCountUp(int rate = 0, int damage = 0)
    {
        increaseRateCount = increaseRateCount + rate;
        increaseDamageCount = increaseDamageCount + damage;
    }
    
    //画面左上の簡易表示
    public void ShowBuffState(int weaponCount)
    {
        if (weaponCount <= 0) return;

        Color c = new Color(1, 1, 1, 1);
        showWeaponTypeImage.color = c;
        for (int i = 0; i < showStateImage.Length; i++)
        {
            showStateImage[i].color = c;
        }

        showWeaponTypeImage.sprite = Sprite;
        showStateCountText[0].text = $"×{weaponCount}";
        showStateCountText[1].text = $":{increaseRateCount}";
        showStateCountText[2].text = $":{increaseDamageCount}";
    }

    public void Initalize(int weaponCount)
    {
        //初期化
        Color c = new Color(0, 0, 0, 0.3f);
        showWeaponTypeImage.color = c;
        for (int i = 0; i < showStateImage.Length; i++)
        {
            showStateImage[i].color = c;
        }

        for (int i = 0; i < showStateCountText.Length; i++)
        {
            showStateCountText[i].text = String.Empty;
        }

        ShowBuffState(weaponCount);
    }
}
