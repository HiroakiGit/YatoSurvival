using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerater : MonoBehaviour
{
    public Player _player;
    public GameObject EnemyPrefab;

    public void Generate()
    {
        for(int i = 0; i<2; i++)
        {
            // �����_����X���W��Y���W�𐶐�
            float randomX = Random.Range(-5f, 5f); // X���W�͈̔͂�-5����5�܂łƂ��܂�
            float randomY = Random.Range(-3f, 3f); // Y���W�͈̔͂�-3����3�܂łƂ��܂�

            // Z���W���蓮�Őݒ�i2D�Q�[���ł͒ʏ�͖�������܂��j
            float zCoordinate = 1f; // �C�ӂ�Z���W��ݒ肵�܂�

            // �V���������_���ȍ��W���쐬
            Vector3 randomPosition = new Vector3(randomX, randomY, zCoordinate);

            GameObject enemy = Instantiate(EnemyPrefab, randomPosition, Quaternion.identity);
            enemy.GetComponent<EnemyAI>().player = _player.player.transform;
        }
    }
}
