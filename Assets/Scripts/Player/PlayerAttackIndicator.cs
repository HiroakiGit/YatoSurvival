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

    // 感度調整用のスケール
    float sensitivity = 0.8f; // 感度を調整する変数
    float screenWidthHalf = Screen.width / 2; // 画面の右半分の判定

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
        // 画面内の制限範囲を計算
        Vector3 screenMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 screenMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector2 touchPosition = touch.position;

            // 画面の右半分をタッチしているかチェック
            if (touchPosition.x > screenWidthHalf && touch.phase == TouchPhase.Moved)
            {
                // スワイプ量に基づいてカーソルを移動（感度調整）
                Vector2 swipeDelta = touch.deltaPosition * sensitivity;

                // 新しいカーソル位置を計算
                Vector3 newCursorPos = cursorInstance.transform.position + new Vector3(swipeDelta.x, swipeDelta.y, 0) * Time.deltaTime;

                // カーソルが画面外に行かないように制限
                newCursorPos.x = Mathf.Clamp(newCursorPos.x, screenMin.x, screenMax.x);
                newCursorPos.y = Mathf.Clamp(newCursorPos.y, screenMin.y, screenMax.y);

                // カーソル位置を更新
                cursorInstance.transform.position = newCursorPos;
            }
        }
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
