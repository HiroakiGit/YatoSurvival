using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform
    public float speed = 5f; // 移動速度
    public float minDistance = 1f; // プレイヤーとの最小距離
    private SpriteRenderer spriteRenderer; // スプライトレンダラーをキャッシュ

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // スプライトレンダラーを取得
    }

    void Update()
    {
        // プレイヤーとの距離を計算
        float distance = Vector2.Distance(transform.position, player.position);

        // プレイヤーへの方向ベクトルを正規化
        Vector2 direction = (player.position - transform.position).normalized;

        // プレイヤーとの距離が最小距離より大きい場合にのみ移動
        if (distance > minDistance)
        {
            // プレイヤーに向かって移動
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }


        // 敵の向きをプレイヤーに向ける（右向きと左向きのみ）
        if (direction.x > 0)
        {
            spriteRenderer.flipX = false; // 右向き
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true; // 左向き
        }
    }
}
