using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartWeapon : Weapon
{
    public Transform playerTransform;
    public float rotationSpeed = 200f; // ��]���x
    public float distanceFromPlayer = 2f; // �v���C���[����̋���
    public float damage = 1; // ����̃_���[�W��
    public float startingAngle = 0f; // ����̏����p�x

    private Quaternion initialRotation;

    public override void Initialize(Transform playerT)
    {
        playerTransform = playerT;
        initialRotation = transform.rotation;
        SetInitialPosition();
    }

    void Update()
    {
        // �v���C���[�̎������]����
        transform.RotateAround(playerTransform.position, Vector3.forward, rotationSpeed * Time.deltaTime);

        // �v���C���[����̈�苗����ۂ�
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        transform.position = playerTransform.position + (Vector3)direction * distanceFromPlayer;

        // ����̌�����������]�ɖ߂�
        transform.rotation = initialRotation;
        
    }

    void SetInitialPosition()
    {
        // �����ʒu��ݒ�
        float angleRad = startingAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0) * distanceFromPlayer;
        transform.position = playerTransform.position + offset;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // �G�Ƀ_���[�W��^����
            }
        }
    }
}
