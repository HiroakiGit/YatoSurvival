using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed;

    public void Fire(Vector2 direction, Transform origin)
    {
        GameObject projectile = Instantiate(projectilePrefab, origin.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = direction.normalized * projectileSpeed;
    }
}
