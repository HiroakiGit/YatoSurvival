using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool attracted = false;
    private Transform player;
    private ObjectPool pool;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        pool = GameObject.FindObjectOfType<ObjectPool>();
    }

    void Update()
    {
        if (attracted && player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    public void AttractToPlayer(Vector2 playerPosition, float speed)
    {
        attracted = true;
        moveSpeed = speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>()._playerExperience.AddExperience(1);
            pool.ReturnObject(gameObject);
        }
    }

    public void Initialize()
    {
        attracted = false;
    }
}
