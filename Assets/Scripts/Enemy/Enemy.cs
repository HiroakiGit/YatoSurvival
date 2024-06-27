using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public ObjectPool experiencePool;
    public int health = 3;
    

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
        else if (collision.gameObject.CompareTag("Map"))
        {
            // マップコライダーとの衝突を無視する
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
}
