using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float GenerateRandomTime(float minTime, float maxTime)
    {
        return Random.Range(minTime, maxTime);
    }
}
