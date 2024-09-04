using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;
    [SerializeField] GameObject LogCanvas;
    [SerializeField] Text LogText;
    public List<string> logList = new List<string>();


    void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        LogCanvas.SetActive(false);
    }

    public void AddLogs(string log)
    {
        logList.Add(log);
    }

    public void Log(float sec, System.Action onComplete)
    {
        StartCoroutine(Show(sec, onComplete));
    }

    public IEnumerator Show(float sec, System.Action onComplete) 
    {
        foreach (string log in logList.ToArray())
        {
            LogText.text = log;
            LogCanvas.SetActive(true);
            yield return new WaitForSecondsRealtime(sec);
            LogText.text = string.Empty;
            LogCanvas.SetActive(false);
        }
        logList.Clear();

        onComplete?.Invoke();
    }
}
