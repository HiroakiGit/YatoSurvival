using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class RankingManager : MonoBehaviour
{
    public UserDataManager _UserDataManager;
    public Player _Player;
    public Timer _Timer;
    public List<Rank> rankingList = new List<Rank>();
    public List<GameObject> rankingObjList = new List<GameObject>();

    [Header("UI")]
    public GameObject RankingCanvas;
    public GameObject LoadingObj;
    public GameObject NoDataTextObj;
    public GameObject BackButton;
    public GameObject RankPrefab;
    public Transform rankingListParent;
    private bool playerInRanking = false;
    private int myNUM;

    [Header("Audio")]
    public AudioClip rankingBGM;
    public AudioClip showDataSoundClip;

    private void Start()
    {
        Initalize();
    }

    public async Task SubmitScore()
    {
        await _UserDataManager.UpdateScore(_Player.SUserName ,_Timer.aliveTime);
    }

    //ランキング取得ボタンを押したとき
    public async void OnClickGetLeaderboardButton()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.isProcessing = true;
            GameManager.Instance.CurrentState = GameState.SubMenuState;
        }

        RankingCanvas.SetActive(true);
        await GetLeaderboard();
        ShowLeaderboard();
        BackButton.SetActive(true);
        BGMAudio.Instance.PlayBGM(rankingBGM, true);

        if (GameManager.Instance != null) 
        {
            GameManager.Instance.isProcessing = false;
        }
    }

    private async Task GetLeaderboard()
    {
        await _UserDataManager.LoadUserData();

        playerInRanking = false;
        myNUM = 0;

        LoadingObj.SetActive(true);
        SEAudio.Instance.PlayOneShot(showDataSoundClip, 0.7f, true);

        var taskCompletionSource = new TaskCompletionSource<bool>();

        int maxcount = 0;
        if (_UserDataManager.GetRankingList().Count < 20)
        {
            maxcount = _UserDataManager.GetRankingList().Count;
        }
        else
        {
            maxcount = 20;
        }

        var resultList = _UserDataManager.GetRankingList();
        for (int i = 0; i < maxcount; i++)
        {
            Rank r = new Rank((i + 1).ToString(), resultList[i].username, resultList[i].score);
            
            if(_Player != null)
            {
                //自分がいるとき
                if (resultList[i].username == _Player.SUserName)
                {
                    playerInRanking = true;
                    myNUM = i;
                }
            }

            rankingList.Add(r);
        }

        taskCompletionSource.SetResult(true);
        await taskCompletionSource.Task;
        await Task.Delay(500);
        LoadingObj.SetActive(false);
    }

    private void ShowLeaderboard()
    {
        if(rankingList.Count == 0) NoDataTextObj.SetActive(true);

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
        if (GameManager.Instance != null) 
        {
            GameManager.Instance.PauseGame(!GameManager.Instance.IsGameFinished(), false);
        }
        else
        {
            BGMAudio.Instance.PlayBGM(null, true);
        }
    }

    private void Initalize()
    {
        RankingCanvas.SetActive(false);
        LoadingObj.SetActive(false);
        NoDataTextObj.SetActive(false);
        BackButton.SetActive(false);

        rankingList.Clear();
        for (int i = 0; i < rankingObjList.Count; i++)
        {
            Destroy(rankingObjList[i].gameObject);
        }

        rankingObjList.Clear();
    }
}
