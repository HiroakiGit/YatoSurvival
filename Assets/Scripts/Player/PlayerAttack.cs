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
    private LaserWeapon[] _LaserWeapon = new LaserWeapon[4];
    public float laserFireRate = 0.75f;
    private float nextLaserFireTime = 0f;
    private int numberOfLaserWeapons = 4;
    [Header("Chart")]
    public GameObject chartWeaponPrefab;
    private ChartWeapon _ChartWeapon;
    private int numberOfRotatingWeapons = 3;
    [Header("SetSquare")]
    public GameObject setSquareWeaponPrefab;
    private SetSquareWeapon _SetSquareWeapon;
    public float setSquareFireRate = 0.5f;
    private float nextSetSquareFireTime = 0f;

    public void GenerateInitialWeapon()
    {
        // �����̉�]����𐶐�
        for (int i = 0; i < numberOfRotatingWeapons; i++)
        {
            float angle = i * (360f / numberOfRotatingWeapons);
            GenerateWeapon(WeaponType.Chart, angle);
        }

        GenerateWeapon(WeaponType.Suica);

        for (int i = 0; i < numberOfLaserWeapons; i++)
        {
            int lazerWeaponsCount = i;
            GenerateWeapon(WeaponType.Laser, 0, lazerWeaponsCount);
        }

        GenerateWeapon(WeaponType.SetSquare);
    }

    private void GenerateWeapon(WeaponType weaponType, float startingAngle = 0f, int lazerWeaponsCount = 0)
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
            case WeaponType.SetSquare:
                weaponPrefab = setSquareWeaponPrefab;
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
                    _LaserWeapon[lazerWeaponsCount] = weapon.GetComponent<LaserWeapon>();
                    //���[�U�[�̏ꍇ�A������ݒ�
                    _LaserWeapon[lazerWeaponsCount].direction = _LaserWeapon[lazerWeaponsCount].directions[lazerWeaponsCount]; 
                    break;
                case WeaponType.Chart:
                    // ��]����̏ꍇ�A�����p�x��ݒ�
                    _ChartWeapon = weapon.GetComponent<ChartWeapon>();
                    _ChartWeapon.startingAngle = startingAngle;
                    break;
                case WeaponType.SetSquare:
                    _SetSquareWeapon = weapon.GetComponent<SetSquareWeapon>();
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
        // Suica�𔭎˂���^�C�~���O���`�F�b�N
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

        //�O�p��K�𔭎˂���^�C�~���O���`�F�b�N
        if (Time.time > nextSetSquareFireTime)
        {
            FireSetSquare();
            nextSetSquareFireTime = Time.time + setSquareFireRate;
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
        if (_LaserWeapon != null)
        {
            // ���[�U�[�r�[���𔭎�
            for(int i = 0; i < numberOfLaserWeapons; i++)
            {
                _LaserWeapon[i].Fire();
            }
        }
    }

    void FireSetSquare()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;
        _SetSquareWeapon.Fire(direction, transform);
    }
}
