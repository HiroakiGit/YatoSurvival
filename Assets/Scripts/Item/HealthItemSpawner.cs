using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItemSpawner : Spawner
{
    public Timer _Timer;
    private MapManager _MapManager;

    public GameObject healthItemPrefab;
    public Transform itemSpawnPoint;
    public float minSpawnInterval = 5f; 
    public float maxSpawnInterval = 15f;

    private float spawnInterval;
    private float timeSinceLastSpawn;
    private bool isGetRandomSpawnTime = false;


    private void Start()
    {
        // マップマネージャーを取得
        _MapManager = FindObjectOfType<MapManager>();
    }

    private void Update()
    {
        //開始まで待つ
        if (!GameManager.Instance.IsGameStarted()) return;

        //ランダムなスポーン間隔を取得
        if (!isGetRandomSpawnTime)
        {
            spawnInterval = _Timer.GenerateRandomTime(minSpawnInterval, maxSpawnInterval);
            isGetRandomSpawnTime = true;
        }

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            Spawn();
            timeSinceLastSpawn = 0f;
            spawnInterval = 0f;
            isGetRandomSpawnTime = false;
        }
    }

    public override void Spawn()
    {
        // ランダムな位置を生成
        Vector2 spawnPosition = new Vector2(
            Random.Range(_MapManager.mapSizeMin.x, _MapManager.mapSizeMax.x),
            Random.Range(_MapManager.mapSizeMin.y, _MapManager.mapSizeMax.y)
        );

        // 回復アイテムを生成
        GameObject healItem = Instantiate(healthItemPrefab, spawnPosition, Quaternion.identity, itemSpawnPoint);
        healItem.GetComponent<HealItem>().Initialize();
    }
}
