using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Enemy _Enemy;

    private SpriteRenderer spriteRenderer;
    private bool walk = false;
    private float lastAttackTime;
    NavMeshAgent agent;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        StartCoroutine(MoveAnimation());
    }

    IEnumerator MoveAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            walk = !walk;

            if (walk)
            {
                spriteRenderer.sprite = _Enemy.spriteWalk;
            }
            else
            {
                spriteRenderer.sprite = _Enemy.spriteNormal;
            }
        }
    }

    void Update()
    {
        // �v���C���[�ւ̕����x�N�g���𐳋K��
        Vector2 direction = (_Enemy._player.playerTransform.position - transform.position).normalized;

        // �v���C���[�Ɍ������Ĉړ�
        agent.SetDestination(_Enemy._player.playerTransform.position);
        

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
        if (_Enemy._player.playerTransform == null || _Enemy._player == null)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, _Enemy._player.playerTransform.position);
        if (distanceToPlayer <= _Enemy.attackRange)
        {
            if (Time.time - lastAttackTime >= _Enemy.attackInterval)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
    }
    void AttackPlayer()
    {
        // �_���[�W���v���C���[�ɗ^����
        _Enemy._player._PlayerHealth.TakeDamage(_Enemy.attackDamage);
        //Debug.Log("Attacked Player for " + _Enemy.attackDamage + " damage.");
    }
}
