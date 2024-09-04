using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Vector2 mapSizeMin; // マップの左下の座標
    public Vector2 mapSizeMax; // マップの右上の座標

    public Vector2 edgeSizeMin; // マップの淵の左下の座標
    public Vector2 edgeSizeMax; // マップの淵の右上の座標

    private void OnDrawGizmos()
    {
        // マップの範囲をGizmosで視覚化
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(mapSizeMin.x, mapSizeMin.y, 0), new Vector3(mapSizeMax.x, mapSizeMin.y, 0));
        Gizmos.DrawLine(new Vector3(mapSizeMax.x, mapSizeMin.y, 0), new Vector3(mapSizeMax.x, mapSizeMax.y, 0));
        Gizmos.DrawLine(new Vector3(mapSizeMax.x, mapSizeMax.y, 0), new Vector3(mapSizeMin.x, mapSizeMax.y, 0));
        Gizmos.DrawLine(new Vector3(mapSizeMin.x, mapSizeMax.y, 0), new Vector3(mapSizeMin.x, mapSizeMin.y, 0));

        // マップの淵の範囲をGizmosで視覚化
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(edgeSizeMin.x, edgeSizeMin.y, 0), new Vector3(edgeSizeMax.x, edgeSizeMin.y, 0));
        Gizmos.DrawLine(new Vector3(edgeSizeMax.x, edgeSizeMin.y, 0), new Vector3(edgeSizeMax.x, edgeSizeMax.y, 0));
        Gizmos.DrawLine(new Vector3(edgeSizeMax.x, edgeSizeMax.y, 0), new Vector3(edgeSizeMin.x, edgeSizeMax.y, 0));
        Gizmos.DrawLine(new Vector3(edgeSizeMin.x, edgeSizeMax.y, 0), new Vector3(edgeSizeMin.x, edgeSizeMin.y, 0));
    }
}

