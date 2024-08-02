using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMAudio : MonoBehaviour
{
    public static BGMAudio Instance;
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
        PlayBGM(null);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGM(null);
    }

    public void PlayBGM(AudioClip clip)
    {
        BGMAudioSource.loop = true;

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

    public void PauseBGM()
    {
        BGMAudioSource.Pause();
    }
    
    public void UnPauseBGM()
    {
        BGMAudioSource.UnPause();
    }

    public void PlayOneShot(AudioClip clip, float volume)
    {
        BGMAudioSource.PlayOneShot(clip, volume);
    }
}
