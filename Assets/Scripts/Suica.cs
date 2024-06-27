using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suica : MonoBehaviour
{
    public int damage = 1; //—^‚¦‚éƒ_ƒ[ƒW

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // ’e‚ğ”j‰ó
        }
        else if (other.CompareTag("Map"))
        {
            Destroy(gameObject); // ’e‚ğ”j‰ó
        }
    }
}
