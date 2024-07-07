using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : Item
{
    private ObjectPool pool;

    void Start()
    {
        pool = GameObject.FindObjectOfType<ObjectPool>();
    }

    void Update()
    {
        if (attracted && playerTransform != null)
        {
            Attracting();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>()._PlayerExperience.AddExperience(1);
            pool.ReturnObject(gameObject);
        }
    }
}
