using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff
{
    [HideInInspector] public GameObject stateUI;
    public string Name;
    public BuffType BuffType;
    public Sprite Sprite;
    public float initalDuration;
    public float duration;
}
