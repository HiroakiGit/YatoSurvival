using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadingScene : MonoBehaviour
{
    [SerializeField] private GameObject _loadingCanvas;
    [SerializeField] private Slider _slider;
    public bool isloadingScene;

    private void Start()
    {
        _loadingCanvas.SetActive(false);
        isloadingScene = false;
    }

    public void OnClickQuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }

    public void LoadNextScene(string SceneName)
    {
        _loadingCanvas.SetActive(true);
        StartCoroutine(LoadScene(SceneName));
    }

    IEnumerator LoadScene(string SceneName)
    {
        isloadingScene = true;
        yield return new WaitForSecondsRealtime(1);
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneName);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            _slider.value = async.progress;
            if (async.progress >= 0.9f)
            {
                yield return new WaitForSecondsRealtime(1);
                _slider.value = 1;
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
