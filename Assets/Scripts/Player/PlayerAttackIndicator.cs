using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackIndicator : MonoBehaviour
{
    [Header("Arrow")]
    public GameObject arrowPrefab; 
    public float distanceFromPlayer = 1f; 
    private GameObject arrowInstance;
    public float movementSpeed = 5f;
    [Header("AttackCursor")]
    public GameObject attackCursorPrefab;
    private GameObject cursorInstance;

    void Start()
    {
        // ���̃v���n�u���v���C���[�̎���ɐ�������
        arrowInstance = Instantiate(arrowPrefab, transform.position, Quaternion.identity, transform);
        arrowInstance.transform.localPosition = new Vector3(0f, distanceFromPlayer, 0f); 

        cursorInstance = Instantiate(attackCursorPrefab, transform.position, Quaternion.identity, transform);
    }

    void LateUpdate()
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

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorInstance.transform.position = Vector2.MoveTowards(cursorInstance.transform.position, mousePosition, 20 * Time.deltaTime);
    }
}
