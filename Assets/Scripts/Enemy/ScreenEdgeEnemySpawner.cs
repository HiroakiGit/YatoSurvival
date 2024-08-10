using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEdgeEnemySpawner : Spawner
{
    public EnemySpawnerManager _EnemySpawnerManager;
    public Player _player;
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;

    private float timeSinceLastSpawn;
    private Camera mainCamera;

    public override void Spawn()
    {
        Vector2 spawnPosition = GetRandomPositionOnScreenEdge();
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, enemySpawnPoint);
        enemy.GetComponent<Enemy>().InitializeEnemyType(_EnemySpawnerManager.GetRandomEnemyType(), _EnemySpawnerManager.EnemyDamageRATIO);
        enemy.GetComponent<EnemyMoveAndAnime>()._Enemy._player = _player;
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        //�J�n�܂ő҂�
        if (!GameManager.Instance.IsGameStarted()) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= _EnemySpawnerManager.ScreenEdgeSpawnInterval)
        {
            Spawn();
            timeSinceLastSpawn = 0f;
        }
    }

    Vector2 GetRandomPositionOnScreenEdge()
    {
        // �X�N���[���̎l�ӂ̂ǂ����������_���ɑI��
        int side = Random.Range(0, 4);
        Vector2 spawnPosition = Vector2.zero;

        switch (side)
        {
            case 0: // ���
                spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), 1f, mainCamera.nearClipPlane));
                break;
            case 1: // �E��
                spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(1f, Random.Range(0f, 1f), mainCamera.nearClipPlane));
                break;
            case 2: // ����
                spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), 0f, mainCamera.nearClipPlane));
                break;
            case 3: // ����
                spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(0f, Random.Range(0f, 1f), mainCamera.nearClipPlane));
                break;
        }

        return spawnPosition;
    }
}
