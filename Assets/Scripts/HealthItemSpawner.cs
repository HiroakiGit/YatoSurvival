using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItemSpawner : MonoBehaviour
{
    public GameObject healthItemPrefab; 
    public float minSpawnInterval = 5f; 
    public float maxSpawnInterval = 15f; 
    private MapManager mapManager; 

    private void Start()
    {
        // マップマネージャーを取得
        mapManager = FindObjectOfType<MapManager>();

        // 最初の回復アイテムの生成を開始する
        StartCoroutine(SpawnHealthItem());
    }

    private IEnumerator SpawnHealthItem()
    {
        while (true)
        {
            // ランダムな時間間隔を待つ
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);

            // ランダムな位置を生成
            Vector2 spawnPosition = new Vector2(
                Random.Range(mapManager.mapSizeMin.x, mapManager.mapSizeMax.x),
                Random.Range(mapManager.mapSizeMin.y, mapManager.mapSizeMax.y)
            );

            // 回復アイテムを生成
            GameObject healItem = Instantiate(healthItemPrefab, spawnPosition, Quaternion.identity);
            healItem.GetComponent<HealItem>().Initialize();
        }
    }
}
