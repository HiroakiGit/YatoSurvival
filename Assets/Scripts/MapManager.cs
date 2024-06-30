using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Vector2 mapSizeMin; // �}�b�v�̍����̍��W
    public Vector2 mapSizeMax; // �}�b�v�̉E��̍��W

    public Vector2 edgeSizeMin; // �}�b�v�̕��̍����̍��W
    public Vector2 edgeSizeMax; // �}�b�v�̕��̉E��̍��W

    private void OnDrawGizmos()
    {
        // �}�b�v�͈̔͂�Gizmos�Ŏ��o��
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(mapSizeMin.x, mapSizeMin.y, 0), new Vector3(mapSizeMax.x, mapSizeMin.y, 0));
        Gizmos.DrawLine(new Vector3(mapSizeMax.x, mapSizeMin.y, 0), new Vector3(mapSizeMax.x, mapSizeMax.y, 0));
        Gizmos.DrawLine(new Vector3(mapSizeMax.x, mapSizeMax.y, 0), new Vector3(mapSizeMin.x, mapSizeMax.y, 0));
        Gizmos.DrawLine(new Vector3(mapSizeMin.x, mapSizeMax.y, 0), new Vector3(mapSizeMin.x, mapSizeMin.y, 0));

        // �}�b�v�̕��͈̔͂�Gizmos�Ŏ��o��
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(edgeSizeMin.x, edgeSizeMin.y, 0), new Vector3(edgeSizeMax.x, edgeSizeMin.y, 0));
        Gizmos.DrawLine(new Vector3(edgeSizeMax.x, edgeSizeMin.y, 0), new Vector3(edgeSizeMax.x, edgeSizeMax.y, 0));
        Gizmos.DrawLine(new Vector3(edgeSizeMax.x, edgeSizeMax.y, 0), new Vector3(edgeSizeMin.x, edgeSizeMax.y, 0));
        Gizmos.DrawLine(new Vector3(edgeSizeMin.x, edgeSizeMax.y, 0), new Vector3(edgeSizeMin.x, edgeSizeMin.y, 0));
    }
}

