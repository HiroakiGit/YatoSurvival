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
    public EnemyStats higeSlimeEnemyStats;

    [Header("Content")]
    public EnemyStats stats;
    public Sprite[] sprites;
    public float scaleChange;
    public float health = 3;
    public float attackRange = 0.5f;
    public float attackDamage = 10;
    public float attackInterval = 5f;
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
            case EnemyType.HigeSlime:
                stats = higeSlimeEnemyStats;
                break;
        }

        if (stats != null)
        {
            sprites = stats.sprites;
            scaleChange = stats.scaleChange;
            health = stats.hp;
            attackRange = stats.attackRange;
            attackDamage = stats.attackDamage * damageRATIO;
            attackInterval = stats.attackInterval;
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

        SEAudio.Instance.PlayOneShot(takeDamageSoundClip, 0.1f);
    }

    void Die()
    {
        switch (enemyType)
        {
            case EnemyType.MimoriSlime:
                DropExperience(5);
                break;
            case EnemyType.OkaSlime:
                DropExperience(7);
                break;
            case EnemyType.HigeSlime:
                DropExperience(10);
                break;
            case EnemyType.Strong:
                DropExperience(1);
                break;
            case EnemyType.Medium:
                DropExperience(1);
                break;
            case EnemyType.Weak:
                DropExperience(1);
                break;
        }
        Destroy(gameObject); // 敵を破壊
    }

    private void DropExperience(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject experience = experiencePool.GetObject();
            // ランダムな範囲を設定
            float radius = 0.1f;
            // ランダムな位置を取得
            Vector2 randomOffset = Random.insideUnitCircle * radius;
            // 現在の位置にランダムなオフセットを追加
            experience.transform.position = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);
            experience.GetComponent<Experience>().Initialize();
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("DeadZone"))
        {
            health = 0;
            Destroy(gameObject); // 敵を破壊
        }
    }
}
