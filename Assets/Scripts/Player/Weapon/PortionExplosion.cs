using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionExplosion : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip explosionSoundClip;

    private void Start()
    {
        SEAudio.Instance.PlayOneShot(explosionSoundClip, 0.07f);
    }

    public void OnAnimationEnd()
    {
        //�A�j���[�V�����I�������̃I�u�W�F������
        Destroy(this.transform.parent.gameObject);
    }
}
