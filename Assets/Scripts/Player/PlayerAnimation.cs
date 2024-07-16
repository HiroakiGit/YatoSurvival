using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Sprite idleUp;
    public Sprite[] walkUp;
    public Sprite idleDown;
    public Sprite[] walkDown;
    public Sprite idleLeft;
    public Sprite[] walkLeft;
    public Sprite idleRight;
    public Sprite[] walkRight;

    public float frameRate = 0.1f; // アニメーションのフレームレート

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float frameTimer;
    private Vector2 lastDirection;
    private bool isMoving;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastDirection = Vector2.down; // 初期向きを下向きに設定
    }

    void Update()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // プレイヤーが動いているかを判断
        isMoving = direction != Vector2.zero;

        if (isMoving)
        {
            // 最後に動いた方向を記憶
            lastDirection = direction;

            // アニメーションフレームを更新
            frameTimer += Time.deltaTime;
            if (frameTimer >= frameRate)
            {
                currentFrame = (currentFrame + 1) % 2; // 0と1を交互に設定
                frameTimer = 0f;
            }
        }
        else
        {
            // 静止しているときはフレームをリセット
            currentFrame = 0;
            frameTimer = 0f;
        }

        // 向きに応じてスプライトを設定
        UpdateSprite(direction);
    }

    void UpdateSprite(Vector2 direction)
    {
        if (direction.y > 0)
        {
            spriteRenderer.sprite = isMoving ? walkUp[currentFrame] : idleUp;
        }
        else if (direction.y < 0)
        {
            spriteRenderer.sprite = isMoving ? walkDown[currentFrame] : idleDown;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.sprite = isMoving ? walkLeft[currentFrame] : idleLeft;
        }
        else if (direction.x > 0)
        {
            spriteRenderer.sprite = isMoving ? walkRight[currentFrame] : idleRight;
        }
        else
        {
            // 最後に動いた方向のスプライトを表示
            if (lastDirection.y > 0)
            {
                spriteRenderer.sprite = isMoving ? walkUp[currentFrame] : idleUp;
            }
            else if (lastDirection.y < 0)
            {
                spriteRenderer.sprite = isMoving ? walkDown[currentFrame] : idleDown;
            }
            else if (lastDirection.x < 0)
            {
                spriteRenderer.sprite = isMoving ? walkLeft[currentFrame] : idleLeft;
            }
            else if (lastDirection.x > 0)
            {
                spriteRenderer.sprite = isMoving ? walkRight[currentFrame] : idleRight;
            }
        }
    }
}
