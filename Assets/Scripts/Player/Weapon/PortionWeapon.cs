using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionWeapon : Weapon
{
    public GameObject portionPrefab;
    private Transform origin;

    public override void Initialize(Transform playerT)
    {
        origin = playerT.gameObject.GetComponent<Player>().PlayerHaveObjectsParent;
    }

    public void Fire(Vector2 targetPosition, Transform origin, float speed, float damage)
    {
        GameObject portion = Instantiate(portionPrefab, origin.position, Quaternion.identity, this.origin);
        Rigidbody2D rb = portion.GetComponent<Rigidbody2D>();
        Portion portionScript = portion.GetComponent<Portion>();
        portionScript.damage = damage;
        portionScript.targetPosition = targetPosition;
        portionScript.Parent = transform;
        rb.velocity = (targetPosition - (Vector2)origin.position).normalized * speed;
    }
}
