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

            //Color(�ʏ���_���[�W��������)
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
            // �v���C���[�Ɍ������Ĉړ�
            Vector3 playerPosition = _Enemy._player.playerTransform.position;
            agent.SetDestination(playerPosition);

            // �ړ����x��ݒ肵�āA�҂��҂��ړ���\��
            agent.speed = _Enemy.stats.moveSpeed; // �X���C���̈ړ����x�i�K�X�����j
            agent.isStopped = false;

            // �X�P�[���ƃX�v���C�g��؂�ւ��ďk�ށE�L�т铮���\��
            spriteIndex = (spriteIndex + 1) % slimeSprites.Length;
            spriteRenderer.sprite = slimeSprites[spriteIndex];

            // �U��
            float distanceToPlayer = Vector2.Distance(transform.position, _Enemy._player.playerTransform.position);
            if (distanceToPlayer <= _Enemy.attackRange)
            {
                if (Time.time - lastAttackTime >= _Enemy.attackInterval)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
            }

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

    void Update()
    {
        //TODO
        if (_Enemy.enemyType == EnemyType.Slime) return;

        // �v���C���[�ւ̕����x�N�g���𐳋K��
        Vector2 direction = (_Enemy._player.playerTransform.position - transform.position).normalized;

        // �v���C���[�Ɍ������Ĉړ�
        Vector3 playerPosition = _Enemy._player.playerTransform.position;
        agent.SetDestination(playerPosition);
        agent.speed = _Enemy.stats.moveSpeed;

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
    }
}
