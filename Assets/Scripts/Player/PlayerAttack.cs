using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Player _Player;
    public Transform weaponSpawnPoint;
    public List<WeaponType> existWeapontypes = new List<WeaponType>();
    public StrengtheningAndAddWeapon _StrengtheningAndAddWeapon;
    public int maxWeaponCount = 5;

    [Header("Suica")]
    public GameObject suicaWeaponPrefab;
    private SuicaWeapon _SuicaWeapon;
    public float suicaFireRate = 0.5f;
    private float nextSuicaFireTime = 0f;
    [Header("LaserBeam")]
    public GameObject laserWeaponPrefab;
    public List<LaserWeapon> _LaserWeapon = new List<LaserWeapon>();
    public float laserFireRate = 0.75f;
    private float nextLaserFireTime = 0f;
    private int numberOfLaserWeapons = 2;
    [Header("Chart")]
    public GameObject chartWeaponPrefab;
    public List<ChartWeapon> _ChartWeapon = new List<ChartWeapon>();
    private List<GameObject> chartWeaponPrefabs = new List<GameObject>();
    private int numberOfChartWeapons = 3;
    private float nextRotationSpeed = 200f;
    [Header("SetSquare")]
    public GameObject setSquareWeaponPrefab;
    private SetSquareWeapon _SetSquareWeapon;
    public float setSquareFireRate = 0.5f;
    private float nextSetSquareFireTime = 0f;
    [Header("Portion")]
    public GameObject portionWeaponPrefab;
    private PortionWeapon _PortionWeapon;
    public float portionFireRate = 5f;
    private float nextPortionFireTime = 0f;

    [Header("Audio")]
    public AudioClip chartRotateSoundClip;
    public AudioClip laserBeamSoundClip;
    public AudioClip fireSuicaSoundClip;

    public void GenerateInitialWeapon()
    {
        // 初期の回転武器を生成
        for (int i = 0; i < numberOfChartWeapons; i++)
        {
            int chartWeaponsCount = i;
            float angle = i * (360f / numberOfChartWeapons);
            GenerateWeapon(WeaponType.Chart, angle, chartWeaponsCount);
        }

        GenerateWeapon(WeaponType.Suica);

        for (int i = 0; i < numberOfLaserWeapons; i++)
        {
            int lazerWeaponsCount = i;
            GenerateWeapon(WeaponType.Laser, 0, 0,lazerWeaponsCount);
        }

        GenerateWeapon(WeaponType.SetSquare);
        GenerateWeapon(WeaponType.Portion);
    }

    private void GenerateWeapon(WeaponType weaponType, float startingAngle = 0f, int chartWeaponsCount = 0,int lazerWeaponsCount = 0)
    {
        GameObject weaponPrefab = null;
        AddWeaponType(weaponType);

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
            case WeaponType.Portion:
                weaponPrefab = portionWeaponPrefab;
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
                    _LaserWeapon.Add(weapon.GetComponent<LaserWeapon>());
                    //レーザーの場合、向きを設定
                    _LaserWeapon[lazerWeaponsCount].direction = _LaserWeapon[lazerWeaponsCount].directions[lazerWeaponsCount]; 
                    break;
                case WeaponType.Chart:
                    // 回転武器の場合、初期角度を設定
                    _ChartWeapon.Add(weapon.GetComponent<ChartWeapon>());
                    chartWeaponPrefabs.Add(weapon);
                    _ChartWeapon[chartWeaponsCount].startingAngle = startingAngle;
                    _ChartWeapon[chartWeaponsCount].rotationSpeed = nextRotationSpeed;
                    if(numberOfChartWeapons == 1) StartCoroutine(PlayChartRotateSound());
                    break;
                case WeaponType.SetSquare:
                    _SetSquareWeapon = weapon.GetComponent<SetSquareWeapon>();
                    break;
                case WeaponType.Portion:
                    _PortionWeapon = weapon.GetComponent<PortionWeapon>();
                    break;
            }

            weaponScript.Initialize(_Player.playerTransform);
        }
    }

    private void AddWeaponType(WeaponType type)
    {
        _StrengtheningAndAddWeapon.AddCanSelectStrengtheningDetails(type);

        if (existWeapontypes.Count != 0)
        {
            bool isExisted = false;

            for (int i = 0; i < existWeapontypes.Count; i++)
            {
                if (existWeapontypes[i] == type) isExisted = true;
            }
            if (!isExisted)
            {
                existWeapontypes.Add(type);
            }
        }
        else
        {
            existWeapontypes.Add(type);
        }
    }

    public void AddWeapon(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Chart:
                foreach (GameObject chart in chartWeaponPrefabs)
                {
                    Destroy(chart.gameObject);
                }
                _ChartWeapon.Clear();

                numberOfChartWeapons++;
                for (int i = 0; i < numberOfChartWeapons; i++)
                {
                    int chartWeaponsCount = i;
                    float angle = i * (360f / numberOfChartWeapons);
                    GenerateWeapon(WeaponType.Chart, angle, chartWeaponsCount);
                }
                break;
        }
    }

    public void DecreaseAttackInterval(WeaponType type, float strengtheningRatio)
    {
        switch (type) 
        {
            case WeaponType.Suica:
                suicaFireRate = suicaFireRate * strengtheningRatio;
                break;
            case WeaponType.Laser:
                laserFireRate = laserFireRate * strengtheningRatio;
                break;
            case WeaponType.Chart:
                nextRotationSpeed = nextRotationSpeed * strengtheningRatio;

                for (int i = 0; i < numberOfChartWeapons; i++)
                {
                    _ChartWeapon[i].rotationSpeed = nextRotationSpeed;
                }
                break;
            case WeaponType.SetSquare:
                setSquareFireRate = setSquareFireRate * strengtheningRatio;
                break;
            case WeaponType.Portion:
                portionFireRate = portionFireRate * strengtheningRatio;
                break;
        }
    }

    void Update()
    {
        //開始まで待つ
        if (!GameManager.Instance.IsGameStarted()) return;

        HandleShooting();
    }

    void HandleShooting()
    {
        // Suicaを発射するタイミングをチェック
        if (Time.time > nextSuicaFireTime)
        {
            FireSuica();
            nextSuicaFireTime = Time.time + suicaFireRate;
        }

        // レーザービームを発射するタイミングをチェック
        if (Time.time > nextLaserFireTime)
        {
            FireLaser();
            nextLaserFireTime = Time.time + laserFireRate;
        }

        //三角定規を発射するタイミングをチェック
        if (Time.time > nextSetSquareFireTime)
        {
            FireSetSquare();
            nextSetSquareFireTime = Time.time + setSquareFireRate;
        }

        //ポーションを発射するタイミングをチェック
        if (Time.time > nextPortionFireTime)
        {
            FirePortion();
            nextPortionFireTime = Time.time + portionFireRate;
        }
    }

    void FireSuica()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;

        if(IsExistWeapon(WeaponType.Suica))
        {
            // Suicaを発射
            _SuicaWeapon.Fire(direction, transform);

            PlayerAudio.Instance.PlayOneShot(fireSuicaSoundClip, 0.4f);
        }
    }

    void FireLaser()
    {
        if (IsExistWeapon(WeaponType.Laser))
        {
            // レーザービームを発射
            for(int i = 0; i < numberOfLaserWeapons; i++)
            {
                _LaserWeapon[i].Fire();
            }

            PlayerAudio.Instance.PlayOneShot(laserBeamSoundClip, 0.15f);
        }
    }

    void FireSetSquare()
    {
        if (IsExistWeapon(WeaponType.SetSquare))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - transform.position).normalized;
            _SetSquareWeapon.Fire(direction, transform);
        }
    }

    void FirePortion()
    {
        if (IsExistWeapon(WeaponType.Portion))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _PortionWeapon.Fire(mousePosition, transform);
        }
    }

    private IEnumerator PlayChartRotateSound()
    {
        while (IsExistWeapon(WeaponType.Chart))
        {
            yield return new WaitForSeconds(0.5f);
            PlayerAudio.Instance.PlayOneShot(chartRotateSoundClip,0.08f);
        }
    }

    private bool IsExistWeapon(WeaponType type)
    {
        bool exist = false;

        for (int i = 0; i < existWeapontypes.Count; i++)
        {
            if (existWeapontypes[i] == type) exist = true;
        }

        return exist;
    }
}
