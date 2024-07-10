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
        // プレイヤーへの方向ベクトルを正規化
        Vector2 direction = (_Enemy._player.playerTransform.position - transform.position).normalized;

        // プレイヤーに向かって移動
        agent.SetDestination(_Enemy._player.playerTransform.position);
        

        // 敵の向きをプレイヤーに向ける（右向きと左向きのみ）
        if (direction.x > 0)
        {
            spriteRenderer.flipX = false; // 右向き
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true; // 左向き
        }

        //攻撃
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
        // ダメージをプレイヤーに与える
        _Enemy._player._PlayerHealth.TakeDamage(_Enemy.attackDamage);
        //Debug.Log("Attacked Player for " + _Enemy.attackDamage + " damage.");
    }
}
