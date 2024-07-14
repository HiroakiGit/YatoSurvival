using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public GameObject arrowPrefab; // 矢印のプレハブをInspectorで設定する
    public float distanceFromPlayer = 1f; // プレイヤーからの距離
    public float movementSpeed = 5f; // 矢印の移動速度

    private GameObject arrowInstance; // 生成された矢印のインスタンス

    void Start()
    {
        // 矢印のプレハブをプレイヤーの周りに生成する
        arrowInstance = Instantiate(arrowPrefab, transform.position, Quaternion.identity, transform);
        arrowInstance.transform.localPosition = new Vector3(0f, distanceFromPlayer, 0f); // プレイヤーからの距離を設定
    }

    void Update()
    {
        // プレイヤーの位置を基準にしたマウスカーソルの方向を取得する
        Vector3 playerToMouse = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg;

        // 矢印がマウスカーソルの方向を向くようにする
        arrowInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle-90);

        // プレイヤーの周りを移動する
        Vector3 offset = Quaternion.Euler(0f, 0f, angle-90) * Vector3.up * distanceFromPlayer;
        Vector3 targetPosition = transform.position + offset;
        arrowInstance.transform.position = Vector3.MoveTowards(arrowInstance.transform.position, targetPosition, movementSpeed * Time.deltaTime);
    }
}
