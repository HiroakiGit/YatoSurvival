﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MachineChangeAdaptor _MachineChangeAdaptor;
    public float normalSpeed;
    public float currentSpeed;
    [Header("Audio")]
    public AudioClip[] walkSoundClip;
    public float stepInterval = 1f;
    private float stepTimer = 0f;

    private Rigidbody2D rb;
    public Vector2 moveInput;
    private Vector2 moveVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        //開始まで待つ
        if (!GameManager.Instance.IsGameStarted()) return;
        //ゲームが終わっていたら何もしない
        if(GameManager.Instance.IsGameFinished()) return;

#if UNITY_STANDALONE
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * currentSpeed;
#endif
#if UNITY_ANDROID || UNITY_IOS
        // ジョイスティックの入力を取得
        float horizontal = _MachineChangeAdaptor.inputMove.Horizontal; // ジョイスティックの水平入力
        float vertical = _MachineChangeAdaptor.inputMove.Vertical;     // ジョイスティックの垂直入力
        // ジョイスティックの入力をベクトルとして保持
        moveInput = new Vector2(horizontal, vertical);
        moveVelocity = moveInput.normalized * currentSpeed * _MachineChangeAdaptor.moveSpeed;
#endif

        //動いているとき
        if (rb.velocity.magnitude > 0.1f)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= (stepInterval * (normalSpeed / currentSpeed)))
            {
                int r = Random.Range(0, walkSoundClip.Length);
                SEAudio.Instance.PlayOneShot(walkSoundClip[r], 1f);
                stepTimer = 0f; // タイマーをリセット
            }
        }
        else
        {
            // プレイヤーが停止した場合、タイマーをリセット
            stepTimer = 0f;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveVelocity;
    }

    public void ChangeSpeed(float speedRATIO)
    {
        if(speedRATIO == 1)
        {
            currentSpeed = normalSpeed * speedRATIO;
        }
        else
        {
            currentSpeed = currentSpeed * speedRATIO;

            if((currentSpeed + 0.3f) < normalSpeed)
            {
                currentSpeed = normalSpeed - 0.3f;
            }
            else if(normalSpeed < (currentSpeed - 0.3f))
            {
                currentSpeed = normalSpeed + 0.3f;
            }
        }
    }
}
