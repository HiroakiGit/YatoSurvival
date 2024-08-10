using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public float MapEdgeSpawnInterval;
    public float ScreenEdgeSpawnInterval;
    public float EnemyDamageRATIO = 1;

    public void ChangeEnemyDamageRATIO(float ratio)
    {
        EnemyDamageRATIO = ratio;
    }

    public EnemyType GetRandomEnemyType()
    {
        //TODO
        //int maxCount = Enum.GetNames(typeof(EnemyType)).Length;
        //int number = UnityEngine.Random.Range(0, maxCount);
        //return (EnemyType)Enum.ToObject(typeof(EnemyType), number);
        return EnemyType.Slime;
    }
}
