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
        // �����������������ɐݒ�
        lastDirection = Vector2.down;
        // �v���C���[�̃s�{�b�g�𑫌��ɐݒ肷�邽�߂̃I�t�Z�b�g���v�Z
        pivotOffset = new Vector3(0, -spriteRenderer.bounds.extents.y, 0);
    }

    void Update()
    {
        if (GameManager.Instance.IsGameFinished()) return;

        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // �v���C���[�������Ă��邩�𔻒f
        isMoving = direction != Vector2.zero;

        if (isMoving)
        {
            // �Ō�ɓ������������L��
            lastDirection = direction;

            // �A�j���[�V�����t���[�����X�V
            frameTimer += Time.deltaTime;
            if (frameTimer >= frameRate)
            {
                currentFrame = (currentFrame + 1) % 2; // 0��1�����݂ɐݒ�
                frameTimer = 0f;
            }
        }
        else
        {
            // �Î~���Ă���Ƃ��̓t���[�������Z�b�g
            currentFrame = 0;
            frameTimer = 0f;
        }

        // �����ɉ����ăX�v���C�g��ݒ�
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
            // �Ō�ɓ����������̃X�v���C�g��\��
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

        Vector3 startPosition = transform.position - pivotOffset; // �s�{�b�g���l�����Ĉʒu�𒲐�
        Vector3 endPosition = startPosition + fallOffset;

        await Task.Delay(1500);

        float t = 0;
        bool isSEPlayed = false;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime * rotateSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            transform.position = Vector3.Lerp(startPosition, endPosition, t) + pivotOffset; // �s�{�b�g���l�����Ĉʒu�𒲐�

            if(!isSEPlayed && t > 0.7f)
            {
                SEAudio.Instance.PlayOneShot(playerDeadSoundClip, 0.6f);
                isSEPlayed = true;
            }
            await Task.Yield();
        }

        transform.rotation = endRotation; // �ŏI�I�ɂ҂�����ƖڕW�p�x�ɂ���
        transform.position = endPosition + pivotOffset; // �ŏI�I�Ȉʒu�𒲐�

        await Task.Delay(1000);
    }
}
