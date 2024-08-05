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
    public Transform playerTransform;
    public float explosionDistanceThreshold = 0.1f; // –Ú•WˆÊ’u‚É‚Ç‚ê‚­‚ç‚¢‹ß‚Ã‚¢‚½‚ç”š”­‚·‚é‚©
    public float damage;

    private void Update()
    {
        if (Vector2.Distance(transform.position, targetPosition) <= explosionDistanceThreshold)
        {
            // ”š”­‚ðŠJŽn
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        // ”š”­ƒGƒtƒFƒNƒg‚ð¶¬
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity, playerTransform);
        }

        // ”š”­‚Ì”ÍˆÍ“à‚Ì“G‚ð’T‚·
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {
                // “G‚ðŒã‘Þ‚³‚¹‚é
                Vector2 explosionDirection = (nearbyObject.transform.position - transform.position).normalized;
                Vector2 retreatPosition = (Vector2)nearbyObject.transform.position + explosionDirection * explosionForce;
                StartCoroutine(MoveEnemy(nearbyObject.transform, retreatPosition));
                //“G‚Éƒ_ƒ[ƒW‚ð—^‚¦‚é
                Enemy enemy = nearbyObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }

        // ”š’e‚ð”j‰ó
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
        // ƒMƒYƒ‚‚Å”š”­”ÍˆÍ‚ð•`‰æ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
