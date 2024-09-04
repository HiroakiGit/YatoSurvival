using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suica : MonoBehaviour
{
    public float damage = 1; //与えるダメージ

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // 弾を破壊
        }
        else if (other.CompareTag("Map") || other.CompareTag("Obstacle"))
        {
            Destroy(gameObject); // 弾を破壊
        }
    }
}
