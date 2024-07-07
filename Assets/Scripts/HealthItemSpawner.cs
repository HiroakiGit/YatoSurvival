using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItemSpawner : Spawner
{
    public Timer _Timer;
    private MapManager _MapManager;

    public GameObject healthItemPrefab; 
    public float minSpawnInterval = 5f; 
    public float maxSpawnInterval = 15f;

    private float spawnInterval;
    private float timeSinceLastSpawn;
    private bool isGetRandomSpawnTime = false;


    private void Start()
    {
        // �}�b�v�}�l�[�W���[���擾
        _MapManager = FindObjectOfType<MapManager>();
    }

    private void Update()
    {
        //�J�n�܂ő҂�
        if (!GameManager.Instance.IsGameStarted()) return;

        //�����_���ȃX�|�[���Ԋu���擾
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
        // �����_���Ȉʒu�𐶐�
        Vector2 spawnPosition = new Vector2(
            Random.Range(_MapManager.mapSizeMin.x, _MapManager.mapSizeMax.x),
            Random.Range(_MapManager.mapSizeMin.y, _MapManager.mapSizeMax.y)
        );

        // �񕜃A�C�e���𐶐�
        GameObject healItem = Instantiate(healthItemPrefab, spawnPosition, Quaternion.identity);
        healItem.GetComponent<HealItem>().Initialize();
    }
}
