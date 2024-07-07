using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicaWeapon : Weapon
{
    public GameObject suicaPrefab;
    public float suicaSpeed;

    public override void Initialize(Transform playerT)
    {

    }

    public void Fire(Vector2 direction, Transform origin)
    {
        GameObject suica = Instantiate(suicaPrefab, origin.position, Quaternion.identity);
        suica.GetComponent<Rigidbody2D>().velocity = direction.normalized * suicaSpeed;
    }
}
