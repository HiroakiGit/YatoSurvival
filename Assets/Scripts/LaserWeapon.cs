using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float laserDuration = 0.2f;

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

        lineRenderer.SetPosition(0, origin.position);

        //敵に当たったところまでレーザー可視化
        //if (hit.collider != null)
        //{
        //lineRenderer.SetPosition(1, hit.point);
        //}
        //else
        //{
        lineRenderer.SetPosition(1, (Vector2)origin.position + direction * 10f); // 任意の距離
        //}

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
                    enemy.TakeDamage(1); // レーザービームのダメージを与える
                }
            }
        }
    }
}
