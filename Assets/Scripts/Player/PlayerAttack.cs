using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Player _player;
    public Transform weaponSpawnPoint;

    [Header("Suica")]
    public GameObject suicaWeaponPrefab;
    private SuicaWeapon _suicaWeapon;
    [Header("LaserBeam")]
    public GameObject laserWeaponPrefab;
    private LaserWeapon _laserWeapon;
    [Header("Chart")]
    public GameObject chartWeaponPrefab;
    private ChartWeapon _chartWeapon;
    private int numberOfRotatingWeapons = 3;

    [Header("Fire")]
    public float fireRate;
    private float nextFireTime = 0f;

    

    private void Start()
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
                    _suicaWeapon = weapon.GetComponent<SuicaWeapon>();
                    break;
                case WeaponType.Laser:
                    _laserWeapon = weapon.GetComponent<LaserWeapon>();
                    break;
                case WeaponType.Chart:
                    // ��]����̏ꍇ�A�����p�x��ݒ�
                    _chartWeapon = weapon.GetComponent<ChartWeapon>();
                    _chartWeapon.startingAngle = startingAngle;
                    break;
            }

            weaponScript.Initialize(_player.playerTransform);
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
        if (Time.time > nextFireTime)
        {
            FireWeapons();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireWeapons()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;

        if(_suicaWeapon != null)
        {
            // �e�𔭎�
            _suicaWeapon.Fire(direction, transform);
        }
        
        if(_laserWeapon != null)
        {
            // ���[�U�[�r�[���𔭎�
            _laserWeapon.Fire(direction, transform);
        }
    }
}
