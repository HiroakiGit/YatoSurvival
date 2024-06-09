using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerater : MonoBehaviour
{
    public Player _player;
    public GameObject EnemyPrefab;

    public void Generate()
    {
        for(int i = 0; i<2; i++)
        {
            // ランダムなX座標とY座標を生成
            float randomX = Random.Range(-5f, 5f); // X座標の範囲は-5から5までとします
            float randomY = Random.Range(-3f, 3f); // Y座標の範囲は-3から3までとします

            // Z座標を手動で設定（2Dゲームでは通常は無視されます）
            float zCoordinate = 1f; // 任意のZ座標を設定します

            // 新しいランダムな座標を作成
            Vector3 randomPosition = new Vector3(randomX, randomY, zCoordinate);

            GameObject enemy = Instantiate(EnemyPrefab, randomPosition, Quaternion.identity);
            enemy.GetComponent<EnemyAI>().player = _player.player.transform;
        }
    }
}
