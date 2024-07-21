using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;
    [SerializeField] GameObject LogCanvas;
    [SerializeField] Text LogText;
    public List<string> logs = new List<string>();


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
        logs.Add(log);
    }

    public void Log(float sec, System.Action onComplete)
    {
        StartCoroutine(Show(sec, onComplete));
    }

    public IEnumerator Show(float sec, System.Action onComplete) 
    {
        foreach (string log in logs)
        {
            LogText.text = log;
            LogCanvas.SetActive(true);
            yield return new WaitForSecondsRealtime(sec);
            LogText.text = string.Empty;
            LogCanvas.SetActive(false);
        }
        logs.Clear();

        onComplete?.Invoke();
    }
}
