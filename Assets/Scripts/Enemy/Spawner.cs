using System;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    public abstract void Spawn();

    public EnemyType GetRandomEnemyType()
    {
        int maxCount = Enum.GetNames(typeof(EnemyType)).Length;
        int number = UnityEngine.Random.Range(0, maxCount);
        return (EnemyType)Enum.ToObject(typeof(EnemyType), number);
    }
}
