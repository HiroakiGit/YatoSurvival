using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSquaer : MonoBehaviour
{
    public float rotationSpeed;
    public float damage = 1; //与えるダメージ

    private void LateUpdate()
    {
        //回転
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        else if (other.CompareTag("Map") || other.CompareTag("Obstacle"))
        {
            Destroy(gameObject); // 三角定規を破壊
        }
    }
}
