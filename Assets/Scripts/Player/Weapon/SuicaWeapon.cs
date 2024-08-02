using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicaWeapon : Weapon
{
    public GameObject suicaPrefab;

    public override void Initialize(Transform playerT)
    {

    }

    public void Fire(Vector2 direction, Transform origin, float speed,float damage)
    {
        GameObject suica = Instantiate(suicaPrefab, origin.position, Quaternion.identity, transform);
        suica.GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;
        suica.GetComponent<Suica>().damage = damage;
    }
}
