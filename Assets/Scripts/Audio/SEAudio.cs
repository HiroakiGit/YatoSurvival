using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SEAudio : MonoBehaviour
{
    public static SEAudio Instance;
    public float startVolume = 0.3f;
    public AudioSource SEAudioSource;
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
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SEAudioSource.volume = startVolume;
    }

    public void PlayOneShot(AudioClip clip, float volume, bool isMust = false)
    {
        if (isMust)
        {
            SEAudioSource.PlayOneShot(clip, volume);
        }
        else 
        {
            if (!GameManager.Instance.IsGameFinished())
            {
                SEAudioSource.PlayOneShot(clip, volume);
            }
        }
    }
}
