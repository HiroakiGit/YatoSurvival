using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SEAudio : MonoBehaviour
{
    public static SEAudio Instance;
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
        //SetParent();
    }

    //public void OnLevelWasLoaded()
    //{
        //SetParent();
    //}

    private void SetParent()
    {
        if (SceneManager.GetActiveScene().name.Contains("Game"))
        {
            transform.parent = GameObject.Find("Player").transform;
        }
    }

    public void PlayOneShot(AudioClip clip, float volume)
    {
        SEAudioSource.PlayOneShot(clip, volume);
    }
}
