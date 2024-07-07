using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Player _Player;
    public Transform weaponSpawnPoint;

    [Header("Suica")]
    public GameObject suicaWeaponPrefab;
    private SuicaWeapon _SuicaWeapon;
    public float suicaFireRate = 0.5f;
    private float nextSuicaFireTime = 0f;
    [Header("LaserBeam")]
    public GameObject laserWeaponPrefab;
    private LaserWeapon _LaserWeapon;
    public float laserFireRate = 0.75f;
    private float nextLaserFireTime = 0f;
    [Header("Chart")]
    public GameObject chartWeaponPrefab;
    private ChartWeapon _ChartWeapon;
    private int numberOfRotatingWeapons = 3;

    public void GenerateInitialWeapon()
    {
        // �����̉�]����𐶐�
        for (int i = 0; i < numberOfRotatingWeapons; i++)
        {
            float angle = i * (360f / numberOfRotatingWeapons);
            GenerateWeapon(WeaponType.Chart, angle);
        }

        GenerateWeapon(WeaponType.Suica);
        GenerateWeapon(WeaponType.Laser);
    }

    private void GenerateWeapon(WeaponType weaponType, float startingAngle = 0f)
    {
        GameObject weaponPrefab = null;

        switch (weaponType)
        {
            case WeaponType.Suica:
                weaponPrefab = suicaWeaponPrefab;
                break;
            case WeaponType.Laser:
                weaponPrefab = laserWeaponPrefab;
                break;
            case WeaponType.Chart:
                weaponPrefab = chartWeaponPrefab;
                break;
        }

        if (weaponPrefab != null)
        {
            GameObject weapon = Instantiate(weaponPrefab, weaponSpawnPoint.position, Quaternion.identity,weaponSpawnPoint);
            Weapon weaponScript = weapon.GetComponent<Weapon>();

            switch (weaponType)
            {
                case WeaponType.Suica:
                    _SuicaWeapon = weapon.GetComponent<SuicaWeapon>();
                    break;
                case WeaponType.Laser:
                    _LaserWeapon = weapon.GetComponent<LaserWeapon>();
                    break;
                case WeaponType.Chart:
                    // ��]����̏ꍇ�A�����p�x��ݒ�
                    _ChartWeapon = weapon.GetComponent<ChartWeapon>();
                    _ChartWeapon.startingAngle = startingAngle;
                    break;
            }

            weaponScript.Initialize(_Player.playerTransform);
        }
    }

    public void AddChartWeapon()
    {
        numberOfRotatingWeapons++;
        for (int i = 0; i < numberOfRotatingWeapons; i++)
        {
            float angle = i * (360f / numberOfRotatingWeapons);
            GenerateWeapon(WeaponType.Chart, angle);
        }
    }

    void Update()
    {
        //�J�n�܂ő҂�
        if (!GameManager.Instance.IsGameStarted()) return;

        HandleShooting();
    }

    void HandleShooting()
    {
        // �e�𔭎˂���^�C�~���O���`�F�b�N
        if (Time.time > nextSuicaFireTime)
        {
            FireSuica();
            nextSuicaFireTime = Time.time + suicaFireRate;
        }

        // ���[�U�[�r�[���𔭎˂���^�C�~���O���`�F�b�N
        if (Time.time > nextLaserFireTime)
        {
            FireLaser();
            nextLaserFireTime = Time.time + laserFireRate;
        }
    }

    void FireSuica()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;

        if(_SuicaWeapon != null)
        {
            // Suica�𔭎�
            _SuicaWeapon.Fire(direction, transform);
        }
    }

    void FireLaser()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;

        if (_LaserWeapon != null)
        {
            // ���[�U�[�r�[���𔭎�
            _LaserWeapon.Fire(direction, transform);
        }
    }
}
