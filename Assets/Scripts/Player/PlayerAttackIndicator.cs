using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackIndicator : MonoBehaviour
{
    public Player _Player;
    [Header("Arrow")]
    public GameObject arrowPrefab; 
    public float distanceFromPlayer = 1f; 
    private GameObject arrowInstance;
    public float movementSpeed = 5f;
    [Header("AttackCursor")]
    public GameObject attackCursorPrefab;
    private GameObject cursorInstance;

    void Start()
    {
        // 矢印のプレハブをプレイヤーの周りに生成する
        arrowInstance = Instantiate(arrowPrefab, transform.position, Quaternion.identity, _Player.PlayerHaveObjectsParent);
        arrowInstance.transform.localPosition = new Vector3(0f, distanceFromPlayer, 0f); 

        cursorInstance = Instantiate(attackCursorPrefab, transform.position, Quaternion.identity, _Player.PlayerHaveObjectsParent);
    }

    void LateUpdate()
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

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorInstance.transform.position = Vector2.MoveTowards(cursorInstance.transform.position, mousePosition, 20 * Time.deltaTime);
    }
}
