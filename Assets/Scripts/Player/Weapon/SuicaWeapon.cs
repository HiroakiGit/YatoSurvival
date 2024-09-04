using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicaWeapon : Weapon
{
    public GameObject suicaPrefab;
    private Transform origin;

    public override void Initialize(Transform playerT)
    {
        origin = playerT.gameObject.GetComponent<Player>().PlayerHaveObjectsParent;
    }

    public void Fire(Vector2 direction, Transform origin, float speed,float damage)
    {
        GameObject suica = Instantiate(suicaPrefab, origin.position, Quaternion.identity, this.origin);
        suica.GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;
        suica.GetComponent<Suica>().damage = damage;
    }
}
