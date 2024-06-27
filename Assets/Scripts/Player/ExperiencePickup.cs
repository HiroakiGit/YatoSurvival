using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePickup : MonoBehaviour
{
    public float attractionRadius = 1.5f;
    public float attractionSpeed = 2f;

    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        AttractNearbyExperience();
    }

    private void AttractNearbyExperience()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attractionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Experience"))
            {
                Experience experience = collider.GetComponent<Experience>();
                if (experience != null)
                {
                    experience.AttractToPlayer(transform.position, attractionSpeed);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
}
