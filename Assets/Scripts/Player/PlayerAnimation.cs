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

    public float frameRate = 0.1f; // �A�j���[�V�����̃t���[�����[�g

    [Header("Dead")]
    public float rotateSpeed;
    public float targetAngle;
    Quaternion targetRotation;

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float frameTimer;
    private Vector2 lastDirection;
    private bool isMoving;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastDirection = Vector2.down; // �����������������ɐݒ�
    }

    void Update()
    {
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

    public IEnumerator PlayerDead()
    {
        spriteRenderer.sprite = idleLeft;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, targetAngle);

        float t = 0;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime * rotateSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        transform.rotation = endRotation; // �ŏI�I�ɂ҂�����ƖڕW�p�x�ɂ���
    }
}
