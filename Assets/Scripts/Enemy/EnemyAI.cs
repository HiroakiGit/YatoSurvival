using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Player _player;
    public float speed = 5f; // 移動速度
    public float minDistance = 1f; // プレイヤーとの最小距離
    private SpriteRenderer spriteRenderer; // スプライトレンダラーをキャッシュ

    [Header("Attack")]
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public float attackInterval = 5f;
    private float lastAttackTime;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // スプライトレンダラーを取得
    }

    void Update()
    {
        //動き
        // プレイヤーとの距離を計算
        float distance = Vector2.Distance(transform.position, _player.playerTransform.position);

        // プレイヤーへの方向ベクトルを正規化
        Vector2 direction = (_player.playerTransform.position - transform.position).normalized;

        // プレイヤーとの距離が最小距離より大きい場合にのみ移動
        if (distance > minDistance)
        {
            // プレイヤーに向かって移動
            transform.position = Vector2.MoveTowards(transform.position, _player.playerTransform.position, speed * Time.deltaTime);
        }

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
        // ダメージをプレイヤーに与える
        _player._playerHealth.TakeDamage(attackDamage);
        Debug.Log("Attacked Player for " + attackDamage + " damage.");
    }
}
