using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class RankingManager : MonoBehaviour
{
    public Player _Player;
    public Timer _Timer;
    public List<Rank> rankingList = new List<Rank>();
    public List<GameObject> rankingObjList = new List<GameObject>();

    [Header("UI")]
    public GameObject RankingCanvas;
    public GameObject LoadingObj;
    public GameObject BackButton;
    public GameObject RankPrefab;
    public Transform rankingListParent;
    private bool playerInRanking = false;
    private int myNUM;

    [Header("Audio")]
    public AudioClip showDataSoundClip;

    private void Start()
    {
        Initalize();
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
        GameManager.Instance.isProcessing = true;

        RankingCanvas.SetActive(true);
        GameManager.Instance.CurrentState = GameState.SubMenuState;

        await GetLeaderboard();
        ShowLeaderboard();
        BackButton.SetActive(true);

        GameManager.Instance.isProcessing = false;
    }

    private async Task GetLeaderboard()
    {
        playerInRanking = false;
        myNUM = 0;
        LoadingObj.SetActive(true);
        SEAudio.Instance.PlayOneShot(showDataSoundClip, 0.7f, true);

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
                
                //自分がいるとき
                if(result.Leaderboard[i].DisplayName == _Player.SDisplayName)
                {
                    playerInRanking = true;
                    myNUM = i;
                }
                
                rankingList.Add(r);
            }
            taskCompletionSource.SetResult(true);
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
            taskCompletionSource.SetResult(false);
        });

        await taskCompletionSource.Task;

        await Task.Delay(1000);

        LoadingObj.SetActive(false);
    }

    private void ShowLeaderboard()
    {
        for (int i = 0; i < rankingList.Count; i++)
        {
            GameObject obj = Instantiate(RankPrefab, rankingListParent);
            rankingObjList.Add(obj);

            RankUI ui = obj.GetComponent<RankUI>();
            ui.positionText.text = $"{rankingList[i].position}.";
            ui.playerNameText.text = rankingList[i].playerName;
            ui.scoreText.text = ((int)rankingList[i].time / 60).ToString("d2")+ ":" + ((int)rankingList[i].time % 60).ToString("d2");
            if (playerInRanking) 
            {
                if(i == myNUM)
                {
                    ui.Outline.enabled = true;
                    playerInRanking = false;
                    myNUM = 0;
                }
            }

            obj = null;
            ui = null;
        }
    }

    public void OnClickBackButton()
    {
        Initalize();
        GameManager.Instance.CurrentState = GameState.PauseState;
    }

    private void Initalize()
    {
        RankingCanvas.SetActive(false);
        LoadingObj.SetActive(false);
        BackButton.SetActive(false);

        rankingList.Clear();
        for (int i = 0; i < rankingObjList.Count; i++)
        {
            Destroy(rankingObjList[i].gameObject);
        }

        rankingObjList.Clear();
    }
}
