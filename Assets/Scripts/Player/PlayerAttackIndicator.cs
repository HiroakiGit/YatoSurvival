using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackIndicator : MonoBehaviour
{
    public Player _Player;
    public MachineChangeAdaptor _MachineChangeAdaptor;
    [Header("Arrow")]
    public GameObject arrowPrefab; 
    public float distanceFromPlayer = 1f; 
    public GameObject arrowInstance;
    [Header("AttackCursor")]
    public GameObject attackCursorPrefab;
    public GameObject cursorInstance;

    public float movementSpeed = 5f;

    void Start()
    {
        // 矢印のプレハブをプレイヤーの周りに生成する
        arrowInstance = Instantiate(arrowPrefab, transform.position, Quaternion.identity, _Player.PlayerHaveObjectsParent);
        arrowInstance.transform.localPosition = new Vector3(0f, distanceFromPlayer, 0f); 

        cursorInstance = Instantiate(attackCursorPrefab, new Vector2(transform.position.x + 0.3f, transform.position.y), Quaternion.identity, _Player.PlayerHaveObjectsParent);
    }

    void LateUpdate()
    {
#if UNITY_STANDALONE
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorInstance.transform.position = Vector2.MoveTowards(cursorInstance.transform.position, mousePosition, 20 * Time.deltaTime);
#endif
#if UNITY_ANDROID || UNITY_IOS
        // ジョイスティックの入力を取得
        float horizontal = _MachineChangeAdaptor.inputRotate.Horizontal; // ジョイスティックの水平入力
        float vertical = _MachineChangeAdaptor.inputRotate.Vertical;     // ジョイスティックの垂直入力
        float speed = _MachineChangeAdaptor.rotateSpeed;
        // ジョイスティックの入力をベクトルとして保持
        Vector2 joystickInput = new Vector2(horizontal, vertical);

        // 移動させる位置を計算（移動速度を調整）
        cursorInstance.transform.position = Vector2.MoveTowards(
            cursorInstance.transform.position,
            cursorInstance.transform.position + (Vector3)joystickInput,
            speed * Time.deltaTime);
#endif

        // プレイヤーの位置を基準にしたマウスカーソルの方向を取得する
        Vector3 playerToMouse = (cursorInstance.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg;

        // 矢印がマウスカーソルの方向を向くようにする
        arrowInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle-90);

        // プレイヤーの周りを移動する
        Vector3 offset = Quaternion.Euler(0f, 0f, angle-90) * Vector3.up * distanceFromPlayer;
        Vector3 targetPosition = transform.position + offset;
        arrowInstance.transform.position = Vector3.MoveTowards(arrowInstance.transform.position, targetPosition, movementSpeed * Time.deltaTime);
    }
}
