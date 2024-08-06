using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    [Header("Audio")]
    public AudioClip[] walkSoundClip;
    public float stepInterval = 1f;
    private float stepTimer = 0f;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //�J�n�܂ő҂�
        if (!GameManager.Instance.IsGameStarted()) return;

        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;

        //�����Ă���Ƃ�
        if(rb.velocity.magnitude > 0.1f)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                int r = Random.Range(0, walkSoundClip.Length);
                SEAudio.Instance.PlayOneShot(walkSoundClip[r], 1f);
                stepTimer = 0f; // �^�C�}�[�����Z�b�g
            }
        }
        else
        {
            // �v���C���[����~�����ꍇ�A�^�C�}�[�����Z�b�g
            stepTimer = 0f;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveVelocity;
    }
}
