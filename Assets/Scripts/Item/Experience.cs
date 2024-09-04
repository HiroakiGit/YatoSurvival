using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : Item
{
    public SpriteRenderer spriteRenderer;
    public Sprite Sprite;
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
            spriteRenderer.sprite = null;
            other.GetComponent<Player>()._PlayerExperience.AddExperience(1);
            spriteRenderer.sprite = Sprite;

            pool.ReturnObject(gameObject);
        }
    }
}
