using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSquaer : MonoBehaviour
{
    public float rotationSpeed;
    public float damage = 1; //—^‚¦‚éƒ_ƒ[ƒW

    private void LateUpdate()
    {
        //‰ñ“]
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
            Destroy(gameObject); // OŠp’è‹K‚ğ”j‰ó
        }
    }
}
