using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GameObject TimerCanvas;
    public int aliveTime;
    public Text minText;
    public Text secText;

    private void Start()
    {
        TimerCanvas.SetActive(false);
    }

    public void TimeCountStart()
    {
        TimerCanvas.SetActive(true);
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
}
