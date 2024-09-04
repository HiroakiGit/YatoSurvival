using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemAndExperience : MonoBehaviour
{
    public float attractionRadius = 1.5f;

    void Update()
    {
        AttractNearbyExperience();
    }

    private void AttractNearbyExperience()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attractionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Item"))
            {
                if(collider.GetComponent<Item>().itemType == ItemType.Experience)
                {
                    Experience experience = collider.GetComponent<Experience>();
                    if (experience != null)
                    {
                        experience.AttractToPlayer(transform);
                    }
                }
                else if(collider.GetComponent<Item>().itemType == ItemType.Heal)
                {
                    HealItem healitem = collider.GetComponent<HealItem>();
                    if (healitem != null)
                    {
                        healitem.AttractToPlayer(transform);
                    }
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
