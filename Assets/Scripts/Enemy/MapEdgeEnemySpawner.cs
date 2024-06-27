using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEdgeEnemySpawner : MonoBehaviour
{
    public Player _player;
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;
    public float spawnInterval = 2f;
    public Vector2 mapCenter = Vector2.zero;
    public float mapWidth = 20f;
    public float mapHeight = 20f;

    private float timeSinceLastSpawn;
    public Color gizmoColor = Color.red;

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPosition = GetRandomPositionOnMapEdge();
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, enemySpawnPoint);
        enemy.GetComponent<EnemyAI>()._player = _player;
    }

    Vector2 GetRandomPositionOnMapEdge()
    {
        // マップの四辺のどこかをランダムに選択
        int side = Random.Range(0, 4);
        Vector2 spawnPosition = Vector2.zero;

        switch (side)
        {
            case 0: // 上辺
                spawnPosition = new Vector2(Random.Range(mapCenter.x - mapWidth / 2, mapCenter.x + mapWidth / 2), mapCenter.y + mapHeight / 2);
                break;
            case 1: // 右辺
                spawnPosition = new Vector2(mapCenter.x + mapWidth / 2, Random.Range(mapCenter.y - mapHeight / 2, mapCenter.y + mapHeight / 2));
                break;
            case 2: // 下辺
                spawnPosition = new Vector2(Random.Range(mapCenter.x - mapWidth / 2, mapCenter.x + mapWidth / 2), mapCenter.y - mapHeight / 2);
                break;
            case 3: // 左辺
                spawnPosition = new Vector2(mapCenter.x - mapWidth / 2, Random.Range(mapCenter.y - mapHeight / 2, mapCenter.y + mapHeight / 2));
                break;
        }

        return spawnPosition;
    }

    // ギズモを使用してマップの四辺を描画
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        // 四辺の頂点を計算
        Vector2 topLeft = new Vector2(mapCenter.x - mapWidth / 2, mapCenter.y + mapHeight / 2);
        Vector2 topRight = new Vector2(mapCenter.x + mapWidth / 2, mapCenter.y + mapHeight / 2);
        Vector2 bottomLeft = new Vector2(mapCenter.x - mapWidth / 2, mapCenter.y - mapHeight / 2);
        Vector2 bottomRight = new Vector2(mapCenter.x + mapWidth / 2, mapCenter.y - mapHeight / 2);

        // 四辺を描画
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
