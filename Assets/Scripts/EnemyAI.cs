using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // �v���C���[��Transform
    public float speed = 5f; // �ړ����x
    public float minDistance = 1f; // �v���C���[�Ƃ̍ŏ�����
    private SpriteRenderer spriteRenderer; // �X�v���C�g�����_���[���L���b�V��

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // �X�v���C�g�����_���[���擾
    }

    void Update()
    {
        // �v���C���[�Ƃ̋������v�Z
        float distance = Vector2.Distance(transform.position, player.position);

        // �v���C���[�ւ̕����x�N�g���𐳋K��
        Vector2 direction = (player.position - transform.position).normalized;

        // �v���C���[�Ƃ̋������ŏ��������傫���ꍇ�ɂ݈̂ړ�
        if (distance > minDistance)
        {
            // �v���C���[�Ɍ������Ĉړ�
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }


        // �G�̌������v���C���[�Ɍ�����i�E�����ƍ������̂݁j
        if (direction.x > 0)
        {
            spriteRenderer.flipX = false; // �E����
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true; // ������
        }
    }
}
