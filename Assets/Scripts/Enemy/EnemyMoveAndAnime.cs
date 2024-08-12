using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveAndAnime : MonoBehaviour
{
    public Enemy _Enemy;

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private NavMeshAgent agent;
    private bool walk = false;
    private float lastAttackTime;

    public Sprite[] slimeSprites;
    private int spriteIndex = 0;
    private Vector3 originalScale;

    [Header("Color")]
    public Color normalColor;
    public Color increaseDamageColor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        originalScale = transform.localScale;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        //TODO
        if(_Enemy.enemyType == EnemyType.Slime)
        {
            circleCollider.radius = 4.8f;
            agent.radius = 2.5f;
            StartCoroutine(SlimeMoveAndAttack());
        }
        else 
        {
            StartCoroutine(NormalEnemyMoveAnimation());
        }
    }

    private IEnumerator NormalEnemyMoveAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            walk = !walk;

            //Anime
            if (walk)
            {
                spriteRenderer.sprite = _Enemy.spriteWalk;
            }
            else
            {
                spriteRenderer.sprite = _Enemy.spriteNormal;
            }

            //Color(通常よりダメージが高い時)
            if(_Enemy.attackDamage > _Enemy.stats.attackDamage)
            {
                spriteRenderer.color = increaseDamageColor;
            }
            else
            {
                spriteRenderer.color= normalColor;
            }
        }
    }

    private IEnumerator SlimeMoveAndAttack()
    {
        while (true)
        {
            // プレイヤーに向かって移動
            Vector3 playerPosition = _Enemy._player.playerTransform.position;
            agent.SetDestination(playerPosition);

            // 移動速度を設定して、ぴょんぴょん移動を表現
            agent.speed = _Enemy.stats.moveSpeed; // スライムの移動速度（適宜調整）
            agent.isStopped = false;

            // スケールとスプライトを切り替えて縮む・伸びる動作を表現
            spriteIndex = (spriteIndex + 1) % slimeSprites.Length;
            spriteRenderer.sprite = slimeSprites[spriteIndex];

            // 攻撃
            float distanceToPlayer = Vector2.Distance(transform.position, _Enemy._player.playerTransform.position);
            if (distanceToPlayer <= _Enemy.attackRange)
            {
                if (Time.time - lastAttackTime >= _Enemy.attackInterval)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
            }

            // スケールを変更し、プレイヤーに向かってぴょんと移動
            if (spriteIndex == 1)
            {
                // 縮む
                transform.localScale = originalScale * 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            else if (spriteIndex == 2)
            {
                // 伸びる
                transform.localScale = originalScale * 0.1f;

                // 一定時間待機（ぴょんと跳ねる動きを表現）
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                // 通常のサイズに戻す
                transform.localScale = originalScale * 0.1f;

                // NavMeshAgentの移動を停止させる（その場に留まる）
                agent.isStopped = true;

                // 一定時間待機
                yield return new WaitForSeconds(_Enemy.stats.attackInterval);
            }
        }
    }

    void Update()
    {
        //TODO
        if (_Enemy.enemyType == EnemyType.Slime) return;

        // プレイヤーへの方向ベクトルを正規化
        Vector2 direction = (_Enemy._player.playerTransform.position - transform.position).normalized;

        // プレイヤーに向かって移動
        Vector3 playerPosition = _Enemy._player.playerTransform.position;
        agent.SetDestination(playerPosition);
        agent.speed = _Enemy.stats.moveSpeed;

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
    }
}
