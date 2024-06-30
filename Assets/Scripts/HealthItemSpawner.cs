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
        // �}�b�v�}�l�[�W���[���擾
        mapManager = FindObjectOfType<MapManager>();

        // �ŏ��̉񕜃A�C�e���̐������J�n����
        StartCoroutine(SpawnHealthItem());
    }

    private IEnumerator SpawnHealthItem()
    {
        while (true)
        {
            // �����_���Ȏ��ԊԊu��҂�
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);

            // �����_���Ȉʒu�𐶐�
            Vector2 spawnPosition = new Vector2(
                Random.Range(mapManager.mapSizeMin.x, mapManager.mapSizeMax.x),
                Random.Range(mapManager.mapSizeMin.y, mapManager.mapSizeMax.y)
            );

            // �񕜃A�C�e���𐶐�
            GameObject healItem = Instantiate(healthItemPrefab, spawnPosition, Quaternion.identity);
            healItem.GetComponent<HealItem>().Initialize();
        }
    }
}
