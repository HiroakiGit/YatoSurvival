using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySetting : MonoBehaviour
{
    public static PlaySetting Instance;

    [Header("PlaySetting")]
    public bool isPlayFabLogin = true;
    public bool isAutoLogin = false;
    public bool isExplain = true;
    public bool isCountDown = true;

    void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
