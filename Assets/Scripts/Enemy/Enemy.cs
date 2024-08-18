using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public Player _player;
    public EnemyMoveAndAttackAndAnime _EnemyMoveAndAttackAndAnime;

    [Header("Stats")]
    public EnemyStats weakEnemyStats;
    public EnemyStats mediumEnemyStats;
    public EnemyStats strongEnemyStats;
    public EnemyStats mimoriSlimeEnemyStats;
    public EnemyStats okaSlimeEnemyStats;

    [Header("Content")]
    public EnemyStats stats;
    public float health = 3;
    public float attackRange = 0.5f;
    public float attackDamage = 10;
    public float attackInterval = 5f;
    public Sprite[] sprites;
    public bool isWave = false;

    [Header("Audio")]
    public AudioClip takeDamageSoundClip;

    [HideInInspector]public ObjectPool experiencePool;

    public void InitializeEnemyType(EnemyType type, float damageRATIO, bool iswave = false)
    {
        enemyType = type;
        isWave = iswave;

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
            case EnemyType.MimoriSlime:
                stats = mimoriSlimeEnemyStats;
                break;
            case EnemyType.OkaSlime:
                stats = okaSlimeEnemyStats;
                break;
        }

        if (stats != null)
        {
            health = stats.hp;
            attackRange = stats.attackRange;
            attackDamage = stats.attackDamage * damageRATIO;
            attackInterval = stats.attackInterval;
            sprites = stats.sprites;
        }
    }

    void Start()
    {
        experiencePool = GameObject.FindObjectOfType<ObjectPool>();
        _EnemyMoveAndAttackAndAnime.ProcessStart();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }

        if (!GameManager.Instance.IsGameFinished())
        {
            SEAudio.Instance.PlayOneShot(takeDamageSoundClip, 0.08f);
        }
    }

    void Die()
    {
        DropExperience();
        Destroy(gameObject); // “G‚ð”j‰ó
    }

    private void DropExperience()
    {
        GameObject experience = experiencePool.GetObject();
        experience.transform.position = transform.position;
        experience.GetComponent<Experience>().Initialize();
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("DeadZone"))
        {
            health = 0;
            Destroy(gameObject); // “G‚ð”j‰ó
        }
    }
}
