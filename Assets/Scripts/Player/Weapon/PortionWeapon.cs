using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionWeapon : Weapon
{
    public GameObject portionPrefab;
    public float throwSpeed = 10f;

    public override void Initialize(Transform player)
    {
        
    }

    public void Fire(Vector2 targetPosition, Transform origin)
    {
        GameObject portion = Instantiate(portionPrefab, origin.position, Quaternion.identity);
        Rigidbody2D rb = portion.GetComponent<Rigidbody2D>();
        Portion portionScript = portion.GetComponent<Portion>();
        portionScript.targetPosition = targetPosition;
        rb.velocity = (targetPosition - (Vector2)origin.position).normalized * throwSpeed;
    }
}
