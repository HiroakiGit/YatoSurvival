using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float laserDuration = 0.2f;

    public void Fire(Vector2 direction, Transform origin)
    {
        StartCoroutine(FireLaser(direction, origin));
    }

    private IEnumerator FireLaser(Vector2 direction, Transform origin)
    {
        // Raycast�̎n�_�������̈ʒu���班���������ʒu�ɐݒ肷��
        Vector2 raycastOrigin = (Vector2)origin.position + direction * 2f; // �Ⴆ�΁A������0.1�P�ʐi�񂾈ʒu

        RaycastHit2D[] hits = Physics2D.RaycastAll(raycastOrigin, direction);
        // Raycast������
        Debug.DrawRay(raycastOrigin, direction * 10f, Color.red);

        lineRenderer.SetPosition(0, origin.position);

        //�G�ɓ��������Ƃ���܂Ń��[�U�[����
        //if (hit.collider != null)
        //{
        //lineRenderer.SetPosition(1, hit.point);
        //}
        //else
        //{
        lineRenderer.SetPosition(1, (Vector2)origin.position + direction * 10f); // �C�ӂ̋���
        //}

        lineRenderer.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        lineRenderer.enabled = false;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1); // ���[�U�[�r�[���̃_���[�W��^����
                }
            }
        }
    }
}
