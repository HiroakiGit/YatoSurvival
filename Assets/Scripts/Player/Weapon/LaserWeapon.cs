using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : Weapon
{
    public Vector2[] directions = { Vector2.right, Vector2.left, Vector2.up, Vector2.down};

    public Transform playerTransform;
    public LineRenderer lineRenderer;
    public Vector2 direction;
    public float laserLength;
    public float laserDuration = 0.2f;
    public int damage = 1; //与えるダメージ

    public override void Initialize(Transform playerT)
    {
        playerTransform = playerT;
        lineRenderer.enabled = false;
    }

    public void Fire()
    {
        StartCoroutine(FireLaser());
    }

    private IEnumerator FireLaser()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll((Vector2)playerTransform.position, direction.normalized * laserLength);
        // Raycastを可視化
        Debug.DrawRay((Vector2)playerTransform.position, direction.normalized * laserLength, Color.red);

        lineRenderer.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        lineRenderer.enabled = false;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (playerTransform == null) return;
        //始点は自分に固定
        lineRenderer.SetPosition(0, playerTransform.position);
        lineRenderer.SetPosition(1, (Vector2)playerTransform.position + direction.normalized * laserLength); // 任意の距離
    }
}
