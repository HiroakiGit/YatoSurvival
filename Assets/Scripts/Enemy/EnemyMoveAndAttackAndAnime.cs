using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class EnemyMoveAndAttackAndAnime : MonoBehaviour
{
    public Enemy _Enemy;

    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private Vector3 originalScale;

    private bool walk = false;
    private float lastAttackTime;

    [Header("Slime")]
    private int spriteIndex = 0;

    [Header("Color")]
    public Color increaseDamageColor;

    public void ProcessStart()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        originalScale = transform.localScale;

        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = _Enemy.stats.attackRange;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        //スライムの初期設定
        if(_Enemy.enemyType == EnemyType.MimoriSlime || _Enemy.enemyType == EnemyType.OkaSlime)
        {
            agent.enabled = true;
            circleCollider.radius = 4.8f;
            agent.radius = 2.5f;
            StartCoroutine(SlimeMoveAndAnime());
        }
        //普通の敵の初期設定
        else 
        {
            StartCoroutine(NormalEnemyAnime());

            if (!_Enemy.isWave)
            {
                agent.enabled = true;
                StartCoroutine(NormalEnemyMove());
            }
            else
            {
                agent.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (_Enemy._player == null) return;

        //攻撃
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

    //Normal(Weak,Medium,Strong)============================================================
    private IEnumerator NormalEnemyAnime()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            walk = !walk;

            //Anime
            if (walk)
            {
                spriteRenderer.sprite = _Enemy.sprites[0];
            }
            else
            {
                spriteRenderer.sprite = _Enemy.sprites[1];
            }

            //Color(通常よりダメージが高い時)
            if(_Enemy.attackDamage > _Enemy.stats.attackDamage)
            {
                spriteRenderer.color = increaseDamageColor;
            }
            else
            {
                spriteRenderer.color= _Enemy.stats.normalColor;
            }
        }
    }

    private IEnumerator NormalEnemyMove()
    {
        while (true)
        {
            // プレイヤーへの方向ベクトルを正規化
            Vector2 direction = (_Enemy._player.playerTransform.position - transform.position).normalized;

            // プレイヤーに向かって移動
            Vector3 playerPosition = _Enemy._player.playerTransform.position;

            if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(playerPosition);
                agent.speed = _Enemy.stats.moveSpeed;
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

            yield return new WaitForSeconds(0.1f);
        }
    }

    //Wave
    public IEnumerator NormalEnemyWaveMove(Vector3 direction, float moveSpeed)
    {
        Vector3 moveDirection;
        moveDirection = direction.normalized;

        while (_Enemy.health > 0)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }
    //======================================================================================


    //Slime=================================================================================
    private IEnumerator SlimeMoveAndAnime()
    {
        while (true)
        {
            // プレイヤーに向かって移動
            Vector3 playerPosition = _Enemy._player.playerTransform.position;

            if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(playerPosition);
                // 移動速度を設定して、ぴょんぴょん移動を表現
                agent.speed = _Enemy.stats.moveSpeed; // スライムの移動速度（適宜調整）
                agent.isStopped = false;
            }

            // スケールとスプライトを切り替えて縮む・伸びる動作を表現
            spriteIndex = (spriteIndex + 1) % _Enemy.sprites.Length;
            spriteRenderer.sprite = _Enemy.sprites[spriteIndex];

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
    //======================================================================================

    void AttackPlayer()
    {
        // ダメージをプレイヤーに与える
        _Enemy._player._PlayerHealth.TakeDamage(_Enemy.attackDamage);
    }
}
