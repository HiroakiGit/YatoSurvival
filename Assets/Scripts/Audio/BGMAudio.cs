using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMAudio : MonoBehaviour
{
    public static BGMAudio Instance;
    public float startVolume = 0.05f;
    public AudioClip LobbyBGM;
    public AudioClip GameBGM;
    public AudioSource BGMAudioSource;

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

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (SceneManager.GetActiveScene().name.Contains("Lobby"))
        {
            PlayBGM(null, true);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BGMAudioSource.volume = startVolume;
        if (SceneManager.GetActiveScene().name.Contains("Lobby"))
        {
            PlayBGM(null, true);
        }
    }

    public void PlayBGM(AudioClip clip, bool loop)
    {
        BGMAudioSource.loop = loop;

        if (clip != null) 
        {
            BGMAudioSource.clip = clip;
            BGMAudioSource.Play();
            return;
        }
        else
        {
            if (SceneManager.GetActiveScene().name.Contains("Lobby"))
            {
                BGMAudioSource.clip = LobbyBGM;
                BGMAudioSource.Play();
            }
            else if (SceneManager.GetActiveScene().name.Contains("Game"))
            {
                BGMAudioSource.clip = GameBGM;
                BGMAudioSource.Play();
            }
        }
    }

    public delegate void functionType();
    public IEnumerator CheckingIsPlaying(functionType callback)
    {
        bool canprocess = true;
        while (canprocess)
        {
            yield return new WaitForSecondsRealtime(0.01f);

            if (!BGMAudioSource.isPlaying)
            {
                if (!GameManager.Instance.IsGameFinished())
                {
                    callback();
                    break;
                }
                else
                {
                    Debug.Log("むり");
                    canprocess = false;
                }
            }
        }
    }

    public void Stop()
    {
        BGMAudioSource.Stop();
    }

    public void PlayOneShot(AudioClip clip, float volume)
    {
        BGMAudioSource.PlayOneShot(clip, volume);
    }
}
