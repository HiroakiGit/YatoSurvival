using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartWeapon : Weapon
{
    public Transform playerTransform;
    public float rotationSpeed = 200f; // 回転速度
    public float distanceFromPlayer = 2f; // プレイヤーからの距離
    public float damage = 1; // 武器のダメージ量
    public float startingAngle = 0f; // 武器の初期角度

    private Quaternion initialRotation;

    public override void Initialize(Transform playerT)
    {
        playerTransform = playerT;
        initialRotation = transform.rotation;
        SetInitialPosition();
    }

    void Update()
    {
        // プレイヤーの周りを回転する
        transform.RotateAround(playerTransform.position, Vector3.forward, rotationSpeed * Time.deltaTime);

        // プレイヤーからの一定距離を保つ
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        transform.position = playerTransform.position + (Vector3)direction * distanceFromPlayer;

        // 武器の向きを初期回転に戻す
        transform.rotation = initialRotation;
        
    }

    void SetInitialPosition()
    {
        // 初期位置を設定
        float angleRad = startingAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0) * distanceFromPlayer;
        transform.position = playerTransform.position + offset;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // 敵にダメージを与える
            }
        }
    }
}
