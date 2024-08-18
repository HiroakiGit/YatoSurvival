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

        //�X���C���̏����ݒ�
        if(_Enemy.enemyType == EnemyType.MimoriSlime || _Enemy.enemyType == EnemyType.OkaSlime)
        {
            agent.enabled = true;
            circleCollider.radius = 4.8f;
            agent.radius = 2.5f;
            StartCoroutine(SlimeMoveAndAnime());
        }
        //���ʂ̓G�̏����ݒ�
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

        //�U��
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

            //Color(�ʏ���_���[�W��������)
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
            // �v���C���[�ւ̕����x�N�g���𐳋K��
            Vector2 direction = (_Enemy._player.playerTransform.position - transform.position).normalized;

            // �v���C���[�Ɍ������Ĉړ�
            Vector3 playerPosition = _Enemy._player.playerTransform.position;

            if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(playerPosition);
                agent.speed = _Enemy.stats.moveSpeed;
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
            // �v���C���[�Ɍ������Ĉړ�
            Vector3 playerPosition = _Enemy._player.playerTransform.position;

            if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(playerPosition);
                // �ړ����x��ݒ肵�āA�҂��҂��ړ���\��
                agent.speed = _Enemy.stats.moveSpeed; // �X���C���̈ړ����x�i�K�X�����j
                agent.isStopped = false;
            }

            // �X�P�[���ƃX�v���C�g��؂�ւ��ďk�ށE�L�т铮���\��
            spriteIndex = (spriteIndex + 1) % _Enemy.sprites.Length;
            spriteRenderer.sprite = _Enemy.sprites[spriteIndex];

            // �X�P�[����ύX���A�v���C���[�Ɍ������Ă҂��ƈړ�
            if (spriteIndex == 1)
            {
                // �k��
                transform.localScale = originalScale * 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            else if (spriteIndex == 2)
            {
                // �L�т�
                transform.localScale = originalScale * 0.1f;

                // ��莞�ԑҋ@�i�҂��ƒ��˂铮����\���j
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                // �ʏ�̃T�C�Y�ɖ߂�
                transform.localScale = originalScale * 0.1f;

                // NavMeshAgent�̈ړ����~������i���̏�ɗ��܂�j
                agent.isStopped = true;

                // ��莞�ԑҋ@
                yield return new WaitForSeconds(_Enemy.stats.attackInterval);
            }
        }
    }
    //======================================================================================

    void AttackPlayer()
    {
        // �_���[�W���v���C���[�ɗ^����
        _Enemy._player._PlayerHealth.TakeDamage(_Enemy.attackDamage);
    }
}
