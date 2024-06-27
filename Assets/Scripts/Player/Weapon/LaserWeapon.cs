using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : Weapon
{
    public Transform playerTransform;
    public LineRenderer lineRenderer;
    public float laserLength;
    public float laserDuration = 0.2f;
    public int damage = 1; //与えるダメージ

    public override void Initialize(Transform playerT)
    {
        playerTransform = playerT;
    }

    public void Fire(Vector2 direction, Transform origin)
    {
        StartCoroutine(FireLaser(direction, origin));
    }

    private IEnumerator FireLaser(Vector2 direction, Transform origin)
    {
        // Raycastの始点を自分の位置から少し離した位置に設定する
        Vector2 raycastOrigin = (Vector2)origin.position + direction * 2f; // 例えば、方向に0.1単位進んだ位置

        RaycastHit2D[] hits = Physics2D.RaycastAll(raycastOrigin, direction);
        // Raycastを可視化
        Debug.DrawRay(raycastOrigin, direction * 10f, Color.red);

        lineRenderer.SetPosition(1, (Vector2)origin.position + direction.normalized * laserLength); // 任意の距離

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
        //視点は自分に固定
        lineRenderer.SetPosition(0, playerTransform.position);
    }
}
