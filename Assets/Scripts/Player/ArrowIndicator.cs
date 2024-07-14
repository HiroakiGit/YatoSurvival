using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public GameObject arrowPrefab; // ���̃v���n�u��Inspector�Őݒ肷��
    public float distanceFromPlayer = 1f; // �v���C���[����̋���
    public float movementSpeed = 5f; // ���̈ړ����x

    private GameObject arrowInstance; // �������ꂽ���̃C���X�^���X

    void Start()
    {
        // ���̃v���n�u���v���C���[�̎���ɐ�������
        arrowInstance = Instantiate(arrowPrefab, transform.position, Quaternion.identity, transform);
        arrowInstance.transform.localPosition = new Vector3(0f, distanceFromPlayer, 0f); // �v���C���[����̋�����ݒ�
    }

    void Update()
    {
        // �v���C���[�̈ʒu����ɂ����}�E�X�J�[�\���̕������擾����
        Vector3 playerToMouse = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg;

        // ��󂪃}�E�X�J�[�\���̕����������悤�ɂ���
        arrowInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle-90);

        // �v���C���[�̎�����ړ�����
        Vector3 offset = Quaternion.Euler(0f, 0f, angle-90) * Vector3.up * distanceFromPlayer;
        Vector3 targetPosition = transform.position + offset;
        arrowInstance.transform.position = Vector3.MoveTowards(arrowInstance.transform.position, targetPosition, movementSpeed * Time.deltaTime);
    }
}
