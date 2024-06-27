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
        // �}�b�v�̎l�ӂ̂ǂ����������_���ɑI��
        int side = Random.Range(0, 4);
        Vector2 spawnPosition = Vector2.zero;

        switch (side)
        {
            case 0: // ���
                spawnPosition = new Vector2(Random.Range(mapCenter.x - mapWidth / 2, mapCenter.x + mapWidth / 2), mapCenter.y + mapHeight / 2);
                break;
            case 1: // �E��
                spawnPosition = new Vector2(mapCenter.x + mapWidth / 2, Random.Range(mapCenter.y - mapHeight / 2, mapCenter.y + mapHeight / 2));
                break;
            case 2: // ����
                spawnPosition = new Vector2(Random.Range(mapCenter.x - mapWidth / 2, mapCenter.x + mapWidth / 2), mapCenter.y - mapHeight / 2);
                break;
            case 3: // ����
                spawnPosition = new Vector2(mapCenter.x - mapWidth / 2, Random.Range(mapCenter.y - mapHeight / 2, mapCenter.y + mapHeight / 2));
                break;
        }

        return spawnPosition;
    }

    // �M�Y�����g�p���ă}�b�v�̎l�ӂ�`��
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        // �l�ӂ̒��_���v�Z
        Vector2 topLeft = new Vector2(mapCenter.x - mapWidth / 2, mapCenter.y + mapHeight / 2);
        Vector2 topRight = new Vector2(mapCenter.x + mapWidth / 2, mapCenter.y + mapHeight / 2);
        Vector2 bottomLeft = new Vector2(mapCenter.x - mapWidth / 2, mapCenter.y - mapHeight / 2);
        Vector2 bottomRight = new Vector2(mapCenter.x + mapWidth / 2, mapCenter.y - mapHeight / 2);

        // �l�ӂ�`��
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
