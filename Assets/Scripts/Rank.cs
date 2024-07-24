using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rank
{
    public string position;
    public string playerName;
    public int time;

    public Rank(string p, string n, int t)
    {
        position = p;
        playerName = n;
        time = t;
    }
}
