using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class RankingManager : MonoBehaviour
{
    public Timer _Timer;
    public GameObject RankingCanvas;
    public List<Rank> rankingList = new List<Rank>();
    public List<GameObject> rankingObjList = new List<GameObject>();

    [Header("UI")]
    public GameObject RankPrefab;
    public Transform rankingListParent;

    private void Start()
    {
        RankingCanvas.SetActive(false);
    }

    public void SubmitScore()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "HighScore",
                    Value = _Timer.aliveTime
                }
            }
        }, result =>
        {
            Debug.Log($"スコア {_Timer.aliveTime} 送信完了！");
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public async void OnClickGetLeaderboardButton()
    {
        await GetLeaderboard();
        ShowLeaderboard();
    }

    private async Task GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScore"
        };

        var taskCompletionSource = new TaskCompletionSource<bool>();

        PlayFabClientAPI.GetLeaderboard(request, result =>
        {
            for (int i = 0; i <  result.Leaderboard.Count; i++)
            {
                Rank r = new Rank((result.Leaderboard[i].Position + 1).ToString(), result.Leaderboard[i].DisplayName, result.Leaderboard[i].StatValue);

                rankingList.Add(r);
            }
            taskCompletionSource.SetResult(true);
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
            taskCompletionSource.SetResult(false);
        });

        await taskCompletionSource.Task;
    }

    private void ShowLeaderboard()
    {
        for (int i = 0; i < rankingList.Count; i++)
        {
            GameObject obj = Instantiate(RankPrefab, rankingListParent);
            rankingObjList.Add(obj);

            RankUI ui = obj.GetComponent<RankUI>();
            ui.positionText.text = rankingList[i].position.ToString();
            ui.playerNameText.text = rankingList[i].playerName;
            ui.scoreText.text = ((int)rankingList[i].time / 60).ToString("d2")+ ":" + ((int)rankingList[i].time % 60).ToString("d2");
            obj = null;
            ui = null;
        }

        RankingCanvas.SetActive(true);
    }

    public void OnClickBackButton()
    {
        RankingCanvas.SetActive(false);
        Initalize();
    }

    private void Initalize()
    {
        rankingList.Clear();
        for (int i = 0; i < rankingObjList.Count; i++)
        {
            Destroy(rankingObjList[i].gameObject);
        }

        rankingObjList.Clear();
    }
}
