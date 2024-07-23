using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public void OnClickGetLeaderboardButton()
    {
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = "HighScore"
        }, result =>
        {
            foreach (var item in result.Leaderboard)
            {
                Debug.Log($"{item.Position + 1}��:{item.DisplayName} " + $"�X�R�A {item.StatValue}");
            }
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
