using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSquareWeapon : Weapon
{
    public GameObject setSquarePrefab;
    public float throwSpeed = 10f;
    public float maxDistance = 5f;

    public void Fire(Vector2 direction, Transform origin)
    {
        StartCoroutine(ThrowSetSquare(direction, origin));
    }

    private IEnumerator ThrowSetSquare(Vector2 direction, Transform origin)
    {
        GameObject setsquare = Instantiate(setSquarePrefab, origin.position, Quaternion.identity);
        Rigidbody2D rb = setsquare.GetComponent<Rigidbody2D>();
        Vector2 startPosition = origin.position;

        // Move towards the target
        while (Vector2.Distance(startPosition, setsquare.transform.position) < maxDistance)
        {
            rb.velocity = direction.normalized * throwSpeed;
            yield return null;
        }

        // Move back to the origin
        while (Vector2.Distance(origin.position, setsquare.transform.position) > 0.1f)
        {
            Vector2 returnDirection = (origin.position - setsquare.transform.position).normalized;
            rb.velocity = returnDirection * throwSpeed;
            yield return null;
        }

        // Stop the boomerang and destroy it
        rb.velocity = Vector2.zero;
        Destroy(setsquare);
    }

    public override void Initialize(Transform playerTransform)
    {
        
    }
}
