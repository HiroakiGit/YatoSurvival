using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public Player _player;
    public Sprite[] spritesNormal;
    public Sprite[] spritesWalk;
    [HideInInspector] public Sprite spriteNormal;
    [HideInInspector] public Sprite spriteWalk;
    public int health = 3;

    public ObjectPool experiencePool;

    [Header("Attack")]
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public float attackInterval = 5f;

    public void InitializeEnemyType(EnemyType type)
    {
        enemyType = type;

        if(enemyType == EnemyType.Weak)
        {
            spriteNormal = spritesNormal[0];
            spriteWalk = spritesWalk[0];
            health = 1;
            attackDamage = 20;
        }
        else if (enemyType == EnemyType.Medium)
        {
            spriteNormal = spritesNormal[1];
            spriteWalk = spritesWalk[1];
            health = 1;
            attackDamage = 20;
        }
        else if (enemyType == EnemyType.Strong)
        {
            spriteNormal = spritesNormal[2];
            spriteWalk = spritesWalk[2];
            health = 1;
            attackDamage = 20;
        }
    }

    void Start()
    {
        experiencePool = GameObject.FindObjectOfType<ObjectPool>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
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
