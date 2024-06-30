using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : Item
{
    public int healAmount = 2;

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
            other.GetComponent<Player>()._playerHealth.Heal(healAmount);
            Destroy(this.gameObject);
        }
    }
}
