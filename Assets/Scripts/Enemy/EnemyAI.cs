using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Player _player;
    public Sprite spriteNormal;
    public Sprite spriteWalk;
    private SpriteRenderer spriteRenderer;
    private bool walk = false;

    [Header("Attack")]
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public float attackInterval = 5f;
    private float lastAttackTime;

    NavMeshAgent agent;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            walk = !walk;

            if (walk)
            {
                spriteRenderer.sprite = spriteWalk;
            }
            else
            {
                spriteRenderer.sprite = spriteNormal;
            }
        }
    }

    void Update()
    {
        //����
        // �v���C���[�Ƃ̋������v�Z
        //float distance = Vector2.Distance(transform.position, _player.playerTransform.position);

        // �v���C���[�ւ̕����x�N�g���𐳋K��
        Vector2 direction = (_player.playerTransform.position - transform.position).normalized;

        // �v���C���[�Ɍ������Ĉړ�
        agent.SetDestination(_player.playerTransform.position);
        

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
        _player._PlayerHealth.TakeDamage(attackDamage);
        Debug.Log("Attacked Player for " + attackDamage + " damage.");
    }
}
