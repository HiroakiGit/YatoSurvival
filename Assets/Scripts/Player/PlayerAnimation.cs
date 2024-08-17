using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Walk")]
    public Sprite idleUp;
    public Sprite[] walkUp;
    public Sprite idleDown;
    public Sprite[] walkDown;
    public Sprite idleLeft;
    public Sprite[] walkLeft;
    public Sprite idleRight;
    public Sprite[] walkRight;

    public float frameRate = 0.1f;

    [Header("Dead")]
    public float rotateSpeed;
    public float targetAngle;
    public Vector3 fallOffset = new Vector3(0, -0.5f, 0); 
    private Vector3 pivotOffset;
    Quaternion targetRotation;

    [Header("Audio")]
    public AudioClip playerDeadSoundClip;

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float frameTimer;
    private Vector2 lastDirection;
    private bool isMoving;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 初期向きを下向きに設定
        lastDirection = Vector2.down;
        // プレイヤーのピボットを足元に設定するためのオフセットを計算
        pivotOffset = new Vector3(0, -spriteRenderer.bounds.extents.y, 0);
    }

    void Update()
    {
        if (GameManager.Instance.IsGameFinished()) return;

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

    public async Task PlayerDead()
    {
        spriteRenderer.sprite = idleLeft;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, targetAngle);

        Vector3 startPosition = transform.position - pivotOffset; // ピボットを考慮して位置を調整
        Vector3 endPosition = startPosition + fallOffset;

        await Task.Delay(1500);

        float t = 0;
        bool isSEPlayed = false;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime * rotateSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            transform.position = Vector3.Lerp(startPosition, endPosition, t) + pivotOffset; // ピボットを考慮して位置を調整

            if(!isSEPlayed && t > 0.7f)
            {
                SEAudio.Instance.PlayOneShot(playerDeadSoundClip, 0.6f);
                isSEPlayed = true;
            }
            await Task.Yield();
        }

        transform.rotation = endRotation; // 最終的にぴったりと目標角度にする
        transform.position = endPosition + pivotOffset; // 最終的な位置を調整

        await Task.Delay(1000);
    }
}
