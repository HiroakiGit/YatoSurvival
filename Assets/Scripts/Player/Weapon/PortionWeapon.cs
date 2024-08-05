using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionWeapon : Weapon
{
    public GameObject portionPrefab;

    public override void Initialize(Transform player)
    {
        
    }

    public void Fire(Vector2 targetPosition, Transform origin, float speed, float damage)
    {
        GameObject portion = Instantiate(portionPrefab, origin.position, Quaternion.identity, transform);
        Rigidbody2D rb = portion.GetComponent<Rigidbody2D>();
        Portion portionScript = portion.GetComponent<Portion>();
        portionScript.damage = damage;
        portionScript.targetPosition = targetPosition;
        portionScript.playerTransform = origin;
        rb.velocity = (targetPosition - (Vector2)origin.position).normalized * speed;
    }
}
