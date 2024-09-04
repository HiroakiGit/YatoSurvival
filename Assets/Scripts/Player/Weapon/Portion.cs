using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portion : MonoBehaviour
{
    public GameObject explosionEffect;
    public float explosionRadius = 1f;
    public float explosionForce = 500f;
    public float enemyMoveSpeed = 100f;
    public Vector2 targetPosition;
    public Transform Parent;
    public float explosionDistanceThreshold = 0.1f; // 目標位置にどれくらい近づいたら爆発するか
    public float damage;

    private void Update()
    {
        if (Vector2.Distance(transform.position, targetPosition) <= explosionDistanceThreshold)
        {
            // 爆発を開始
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        // 爆発エフェクトを生成
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity, Parent);
        }

        // 爆発の範囲内の敵を探す
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {
                // 敵を後退させる
                Vector2 explosionDirection = (nearbyObject.transform.position - transform.position).normalized;
                Vector2 retreatPosition = (Vector2)nearbyObject.transform.position + explosionDirection * explosionForce;
                StartCoroutine(MoveEnemy(nearbyObject.transform, retreatPosition));
                //敵にダメージを与える
                Enemy enemy = nearbyObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }

        // 爆弾を破壊
        Destroy(gameObject);
        yield return null;
    }

    private IEnumerator MoveEnemy(Transform enemy, Vector2 targetPosition)
    {
        while (Vector2.Distance(enemy.position, targetPosition) > 0.1f)
        {
            enemy.position = Vector2.MoveTowards(enemy.position, targetPosition, enemyMoveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void OnDrawGizmosSelected()
    {
        // ギズモで爆発範囲を描画
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
