using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public Player _player;

    [Header("Stats")]
    public EnemyStats weakEnemyStats;
    public EnemyStats mediumEnemyStats;
    public EnemyStats strongEnemyStats;
    public EnemyStats slimeEnemyStats;

    [Header("Content")]
    public EnemyStats stats;
    public float health = 3;
    public float attackRange = 0.5f;
    public float attackDamage = 10;
    public float attackInterval = 5f;

    [Header("UI")]
    public Sprite spriteNormal;
    public Sprite spriteWalk;

    [Header("Audio")]
    public AudioClip takeDamageSoundClip;

    [HideInInspector]public ObjectPool experiencePool;

    public void InitializeEnemyType(EnemyType type, float damageRATIO)
    {
        enemyType = type;

        switch (enemyType)
        {
            case EnemyType.Weak:
                stats = weakEnemyStats;
                break;
            case EnemyType.Medium:
                stats = mediumEnemyStats;
                break;
            case EnemyType.Strong:
                stats = strongEnemyStats;
                break;
            case EnemyType.Slime:
                stats = slimeEnemyStats;
                break;
        }

        if (stats != null)
        {
            health = stats.hp;
            attackRange = stats.attackRange;
            attackDamage = stats.attackDamage * damageRATIO;
            attackInterval = stats.attackInterval;
            spriteNormal = stats.spriteNormal;
            spriteWalk = stats.spriteWalk;
        }
    }

    void Start()
    {
        experiencePool = GameObject.FindObjectOfType<ObjectPool>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }

        SEAudio.Instance.PlayOneShot(takeDamageSoundClip, 0.15f);
    }

    void Die()
    {
        DropExperience();
        Destroy(gameObject); // 敵を破壊
    }

    private void DropExperience()
    {
        GameObject experience = experiencePool.GetObject();
        experience.transform.position = transform.position;
        experience.GetComponent<Experience>().Initialize();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーとの衝突を無視する
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // 敵との衝突を無視する
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
        else if (collision.gameObject.CompareTag("Map"))
        {
            // マップコライダーとの衝突を無視する
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
}
