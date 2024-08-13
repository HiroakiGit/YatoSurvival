using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeBuff
{
    [HideInInspector]public GameObject stateUI;
    public string Name;
    public DeBuffType DeBuffType;
    public Sprite Sprite;
    public float duration;
}
