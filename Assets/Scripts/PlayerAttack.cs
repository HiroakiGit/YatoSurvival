using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public ProjectileWeapon projectileWeapon;
    public LaserWeapon laserWeapon;

    public float fireRate;
    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.time > nextFireTime)
        {
            FireWeapons();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireWeapons()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;

        // �e�𔭎�
        projectileWeapon.Fire(direction, transform);

        // ���[�U�[�r�[���𔭎�
        laserWeapon.Fire(direction, transform);
    }
}
