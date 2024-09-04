using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public float attractionSpeed;
    public Transform playerTransform;
    public bool attracted = false;
    public ItemType itemType;

    public void AttractToPlayer(Transform playerPosition)
    {
        attracted = true;
        playerTransform = playerPosition;
    }

    public void Attracting()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, attractionSpeed * Time.deltaTime);
    }

    public void Initialize()
    {
        attracted = false;
    }
}
