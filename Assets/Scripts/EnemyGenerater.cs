using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerater : MonoBehaviour
{
    public GameManager _gameManager;
    public GameObject EnemyPrefab;

    public void Generate()
    {
        GameObject enemy = Instantiate(EnemyPrefab, new Vector3(-3, -2, 1), Quaternion.identity);
        enemy.GetComponent<EnemyAI>().player = _gameManager.player.transform;
    }
}
