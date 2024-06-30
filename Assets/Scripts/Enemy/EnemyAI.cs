using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Player _player;
    public float speed = 5f; // �ړ����x
    public float minDistance = 1f; // �v���C���[�Ƃ̍ŏ�����
    private SpriteRenderer spriteRenderer; // �X�v���C�g�����_���[���L���b�V��

    [Header("Attack")]
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public float attackInterval = 5f;
    private float lastAttackTime;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // �X�v���C�g�����_���[���擾
    }

    void Update()
    {
        //����
        // �v���C���[�Ƃ̋������v�Z
        float distance = Vector2.Distance(transform.position, _player.playerTransform.position);

        // �v���C���[�ւ̕����x�N�g���𐳋K��
        Vector2 direction = (_player.playerTransform.position - transform.position).normalized;

        // �v���C���[�Ƃ̋������ŏ��������傫���ꍇ�ɂ݈̂ړ�
        if (distance > minDistance)
        {
            // �v���C���[�Ɍ������Ĉړ�
            transform.position = Vector2.MoveTowards(transform.position, _player.playerTransform.position, speed * Time.deltaTime);
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

        //�U��
        if (_player.playerTransform == null || _player == null)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, _player.playerTransform.position);
        if (distanceToPlayer <= attackRange)
        {
            if (Time.time - lastAttackTime >= attackInterval)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
    }
    void AttackPlayer()
    {
        // �_���[�W���v���C���[�ɗ^����
        _player._playerHealth.TakeDamage(attackDamage);
        Debug.Log("Attacked Player for " + attackDamage + " damage.");
    }
}
