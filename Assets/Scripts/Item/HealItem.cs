using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : Item
{
    public float healAmount = 2;

    void Update()
    {
        if (attracted && playerTransform != null)
        {
            Attracting();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>()._PlayerHealth.Heal(healAmount);
            Destroy(this.gameObject);
        }
    }
}
