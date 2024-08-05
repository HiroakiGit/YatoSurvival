using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSquareWeapon : Weapon
{
    public GameObject setSquarePrefab;
    public float maxDistance = 5f;

    public void Fire(Vector2 direction, Transform origin, float speed, float damage)
    {
        StartCoroutine(ThrowSetSquare(direction, origin, speed, damage));
    }

    private IEnumerator ThrowSetSquare(Vector2 direction, Transform origin, float speed, float damage)
    {
        GameObject setsquare = Instantiate(setSquarePrefab, origin.position, Quaternion.identity, transform);
        setsquare.transform.GetChild(0).GetComponent<SetSquaer>().damage = damage;
        Rigidbody2D rb = setsquare.GetComponent<Rigidbody2D>();
        Vector2 startPosition = origin.position;

        // Move towards the target
        while (Vector2.Distance(startPosition, setsquare.transform.position) < maxDistance)
        {
            rb.velocity = direction.normalized * speed;
            yield return null;
        }

        // Move back to the origin
        while (Vector2.Distance(origin.position, setsquare.transform.position) > 0.1f)
        {
            Vector2 returnDirection = (origin.position - setsquare.transform.position).normalized;
            rb.velocity = returnDirection * speed;
            yield return null;
        }

        // Stop the boomerang and destroy it
        rb.velocity = Vector2.zero;
        Destroy(setsquare.gameObject);
    }

    public override void Initialize(Transform playerTransform)
    {
        
    }
}
