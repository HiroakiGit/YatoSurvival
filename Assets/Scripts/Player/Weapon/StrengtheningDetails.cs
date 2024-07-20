using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StrengtheningDetails
{
    public WeaponType WeaponType;
    public Sprite Sprite;
    public int state;
    public float decreaseAttackIntervalRatio;
    public float increaseDamage;
}
