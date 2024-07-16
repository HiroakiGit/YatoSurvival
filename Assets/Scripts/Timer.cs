using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int aliveTime;
    [SerializeField] Text minText;
    [SerializeField] Text secText;

    public void TimeCountStart()
    {
        StartCoroutine(TimeCounter());
    }

    //Timer
    private IEnumerator TimeCounter()
    {
        while (GameManager.Instance.IsGameFinished() == false)
        {
            yield return new WaitForSeconds(1);

            aliveTime++;
            minText.text = ((int)(aliveTime / 60)).ToString("d2");
            secText.text = ((int)aliveTime % 60).ToString("d2");
        }
    }
    public float GenerateRandomTime(float minTime, float maxTime)
    {
        return Random.Range(minTime, maxTime);
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
                    Value = aliveTime
                }
            }
        }, result =>
        {
            Debug.Log($"スコア {aliveTime} 送信完了！");
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
