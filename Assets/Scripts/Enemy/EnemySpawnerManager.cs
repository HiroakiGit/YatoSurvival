using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemySpawnerManager : MonoBehaviour
{
    public Player _player;
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;

    public float MapEdgeSpawnInterval;
    public float ScreenEdgeSpawnInterval;
    public float EnemyDamageRATIO = 1;

    [Header("Boss")]
    public int maxBossCount;
    public int bossSpawnTimeIntervalMin = 5;
    private EnemyType bossEnemyType;
    private int bossCount;

    [Header("UI")]
    public GameObject DangerCanvas;
    public GameObject dangerImage;

    private void Start()
    {
        DangerCanvas.SetActive(false);
        bossCount = 0;
    }

    public void ChangeEnemyDamageRATIO(float ratio)
    {
        EnemyDamageRATIO = ratio;
    }

    public EnemyType GetRandomEnemyType()
    {
        int number = UnityEngine.Random.Range(0, 3);//0,1,2
        return (EnemyType)Enum.ToObject(typeof(EnemyType), number);
    }

    //Boss==========================================================================
    public void StartBossSpawnProcess()
    {
        bossCount++;
        int r = bossCount;

        //ボスのカウントがボスの数より多いときランダムなボスにする
        if (bossCount >= maxBossCount)
        {
            r = UnityEngine.Random.Range(1, maxBossCount);
        }
        
        if (r == 1)
        {
            bossEnemyType = EnemyType.Slime;
        }

        StartCoroutine(DangerUIProcess(bossEnemyType));
    }

    private IEnumerator DangerUIProcess(EnemyType bossEnemyType)
    {
        DangerCanvas.SetActive(true);

        for(int i = 0; i < 3; i++)
        {
            dangerImage.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            dangerImage.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

        DangerCanvas.SetActive(false);
        SpawnBoss(bossEnemyType);
    } 

    private void SpawnBoss(EnemyType bossEnemyType)
    {
        Vector2 spawnPosition = new (0,0);
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, enemySpawnPoint);
        enemy.GetComponent<Enemy>().InitializeEnemyType(bossEnemyType, EnemyDamageRATIO);
        enemy.GetComponent<Enemy>()._player = _player;
    }
}
