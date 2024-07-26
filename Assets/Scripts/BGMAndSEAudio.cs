using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMAndSEAudio : MonoBehaviour
{
    public static BGMAndSEAudio Instance;
    public AudioClip LobbyBGM;
    public AudioClip GameBGM;
    public AudioSource BGMAndSEAudioSource;

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
        PlayBGM(null);
    }

    public void OnLevelWasLoaded()
    {
        PlayBGM(null);
    }

    public void PlayBGM(AudioClip clip)
    {
        BGMAndSEAudioSource.loop = true;

        if (clip != null) 
        {
            BGMAndSEAudioSource.clip = clip;
            BGMAndSEAudioSource.Play();
            return;
        }

        if (SceneManager.GetActiveScene().name.Contains("Lobby"))
        {
            BGMAndSEAudioSource.clip = LobbyBGM;
            BGMAndSEAudioSource.Play();
        }
        else if (SceneManager.GetActiveScene().name.Contains("Game"))
        {
            BGMAndSEAudioSource.clip = GameBGM;
            BGMAndSEAudioSource.Play();
        }
    }

    public void PauseBGM()
    {
        BGMAndSEAudioSource.Pause();
    }

    public void PlayOneShot(AudioClip clip, float volume)
    {
        BGMAndSEAudioSource.PlayOneShot(clip, volume);
    }
}
