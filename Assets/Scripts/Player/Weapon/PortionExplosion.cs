using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionExplosion : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource playerAudioSource;
    public AudioClip explosionSoundClip;

    private void Start()
    {
        playerAudioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        playerAudioSource.PlayOneShot(explosionSoundClip, 0.07f);
    }

    public void OnAnimationEnd()
    {
        //アニメーション終了時このオブジェを消す
        Destroy(this.transform.parent.gameObject);
    }
}
