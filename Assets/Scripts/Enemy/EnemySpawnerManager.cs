using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemySpawnerManager : MonoBehaviour
{
    public Player _player;
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;

    public float MapEdgeSpawnInterval;
    public float ScreenEdgeSpawnInterval;
    public float EnemyDamageRATIO = 1;

    [Header("Wave")]
    public int enemyWaveTimeIntervalMin = 2;
    public List<EnemyWaveSpawnData> WaveSpawnDataList = new List<EnemyWaveSpawnData>();
    public float waveMoveSpeed = 1;
    private AudioClip beepClip;

    [Header("Boss")]
    public int maxBossCount;
    public int bossSpawnTimeIntervalMin = 5;
    private EnemyType bossEnemyType;
    private int bossCount;
    AudioClip bossComingClip;

    [Header("UI")]
    public GameObject DangerCanvas;
    public GameObject dangerImage;
    public Text dangerText;

    private Coroutine currentDangerCoroutine;

    private void Start()
    {
        DangerCanvas.SetActive(false);
        beepClip = SoundClipCreator.Instance.CreateClip(180, 180, 0.3f, false);
        bossComingClip = SoundClipCreator.Instance.CreateClip(100, 50, 1f, true);
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

    //EnemyWave=====================================================================
    public void StartEnemyWaveProcess()
    {
        ShowDangerMessage("敵が大勢来る...", false, EnemyType.Weak);
    }

    private EnemyWaveSpawnData GetRandomWaveSpawnData()
    {
        return WaveSpawnDataList[UnityEngine.Random.Range(0, WaveSpawnDataList.Count)];
    }

    public void SpawnEnemiesInLine(Vector2 startPosition, Vector2 moveDirection, int enemyCount, float spacing)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // 進行方向に対して垂直な方向を計算（2Dの場合）
            Vector2 perpendicularDirection = new Vector3(-moveDirection.y, moveDirection.x, 0).normalized;
            // 各敵のスポーン位置を計算
            Vector2 spawnPosition = startPosition + perpendicularDirection.normalized * spacing * i;

            // 敵をスポーン（Instantiate）
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, enemySpawnPoint);
            enemy.GetComponent<Enemy>().InitializeEnemyType(EnemyType.Weak, EnemyDamageRATIO, true);
            enemy.GetComponent<Enemy>()._player = _player;

            // 敵の移動
            StartCoroutine(enemy.GetComponent<EnemyMoveAndAttackAndAnime>().NormalEnemyWaveMove(moveDirection, waveMoveSpeed));
        }
    }

    //Boss==========================================================================
    public void StartBossSpawnProcess()
    {
        bossCount++;
        int r = bossCount;

        //ボスのカウントがボスの数より多いときランダムなボスにする
        if (bossCount > maxBossCount)
        {
            r = UnityEngine.Random.Range(1, maxBossCount + 1);
        }
        
        if (r == 1)
        {
            bossEnemyType = EnemyType.MimoriSlime;
        }
        else if (r == 2)
        {
            bossEnemyType = EnemyType.OkaSlime;
        }

        ShowDangerMessage("ボスが来る...", true, bossEnemyType);
    }

    private void SpawnBoss(EnemyType bossEnemyType)
    {
        Vector2 spawnPosition = new(0, 0);
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, enemySpawnPoint);
        enemy.GetComponent<Enemy>().InitializeEnemyType(bossEnemyType, EnemyDamageRATIO);
        enemy.GetComponent<Enemy>()._player = _player;
    }

    //Danger===========================================================================

    public void ShowDangerMessage(string message, bool isBoss, EnemyType type)
    {
        if (currentDangerCoroutine != null)
        {
            StopCoroutine(currentDangerCoroutine);
        }
        currentDangerCoroutine = StartCoroutine(DangerUIProcess(message, isBoss, type));
    }


    private IEnumerator DangerUIProcess(string m, bool isBoss, EnemyType bossEnemyType)
    {
        DangerCanvas.SetActive(true);
        dangerText.text = m;

        for(int i = 0; i < 3; i++)
        {
            SEAudio.Instance.PlayOneShot(beepClip, 0.1f);

            dangerImage.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            dangerImage.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

        DangerCanvas.SetActive(false);

        if (isBoss)
        {
            SpawnBoss(bossEnemyType);
            SEAudio.Instance.PlayOneShot(bossComingClip, 0.25f);
        }
        else
        {
            EnemyWaveSpawnData spawndata = GetRandomWaveSpawnData();
            SpawnEnemiesInLine(spawndata.spawnPosition, spawndata.Direction, 40, 0.3f);
        }
    } 
}
