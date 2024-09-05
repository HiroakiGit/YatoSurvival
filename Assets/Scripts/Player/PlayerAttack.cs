using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerAttack : MonoBehaviour
{
    public Player _Player;
    public Transform weaponSpawnPoint;
    public BuffAndDeBuffManager _BuffAndDeBuff;

    [Header("Suica")]
    public GameObject suicaWeaponPrefab;
    private SuicaWeapon _SuicaWeapon;
    public float suicaFireRate = 0.5f;
    public float suicaDamage = 1;
    public float suicaSpeed = 3;
    public int maxSuicaCount;
    private float nextSuicaFireTime = 0f;
    private int numberOfSuicaWeapons = 0;
    [Header("LaserBeam")]
    public GameObject laserWeaponPrefab;
    public List<LaserWeapon> _LaserWeapon = new List<LaserWeapon>();
    public float laserFireRate = 0.75f;
    public float laserDamage = 1;
    public int maxLaserCount;
    private float nextLaserFireTime = 0f;
    private int numberOfLaserWeapons = 0;
    [Header("Chart")]
    public GameObject chartWeaponPrefab;
    public List<ChartWeapon> _ChartWeapon = new List<ChartWeapon>();
    public float chartDamage = 1;
    public int maxChartCount;
    private List<GameObject> chartWeaponPrefabs = new List<GameObject>();
    public float chartRotateFireRate = 200f;
    private int numberOfChartWeapons = 0;
    [Header("SetSquare")]
    public GameObject setSquareWeaponPrefab;
    private SetSquareWeapon _SetSquareWeapon;
    public float setSquareFireRate = 0.5f;
    public float setSquareDamage = 1;
    public float setSquareSpeed = 10f;
    public int maxSetSquareCount;
    private float nextSetSquareFireTime = 0f;
    private int numberOfSetSquareWeapons = 0;
    [Header("Portion")]
    public GameObject portionWeaponPrefab;
    private PortionWeapon _PortionWeapon;
    public float portionFireRate = 5f;
    public float portionDamage = 1;
    public float portionSpeed = 10f;
    public int maxPortionCount;
    private float nextPortionFireTime = 0f;
    private int numberOfPortionWeapons = 0;

    private Transform attackTarget;

    [Header("Audio")]
    public AudioClip chartRotateSoundClip;
    public AudioClip laserBeamSoundClip;
    public AudioClip fireSuicaSoundClip;
    public AudioClip fireSetSquareSoundClip;
    public AudioClip firePortionSoundClip;

    private void Start()
    {
        StartCoroutine(SetAttackTarget());
    }

    private IEnumerator SetAttackTarget()
    {
        while (true)
        {
            if(_Player._PlayerAttackIndicator.cursorInstance != null)
            {
                attackTarget = _Player._PlayerAttackIndicator.cursorInstance.transform;
                break;
            }

            yield return null;
        }
    }

    //初期武器
    public void GenerateInitialWeapon()
    {
        AddWeapon(WeaponType.Suica);
    }

    private void GenerateWeapon(WeaponType weaponType, float startingAngle = 0f, int chartWeaponsCount = 0,int lazerWeaponsCount = 0)
    {
        GameObject weaponPrefab = null;
        _BuffAndDeBuff.AddCanSelectWeaponBuffList(weaponType);

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
                    _ChartWeapon[chartWeaponsCount].rotationSpeed = chartRotateFireRate;
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

    public void AddWeapon(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Suica:
                if(numberOfSuicaWeapons == 0)
                {
                    GenerateWeapon(type);
                }
                numberOfSuicaWeapons++;
                break;
            case WeaponType.Laser:
                GenerateWeapon(type, 0, 0, numberOfLaserWeapons);
                numberOfLaserWeapons++;
                break;
            case WeaponType.Chart:
                //既存のチャートをDestroy
                foreach (GameObject chart in chartWeaponPrefabs)
                {
                    Destroy(chart.gameObject);
                }
                _ChartWeapon.Clear();

                //追加
                numberOfChartWeapons++;
                for (int i = 0; i < numberOfChartWeapons; i++)
                {
                    int chartWeaponsCount = i;
                    float angle = i * (360f / numberOfChartWeapons);
                    GenerateWeapon(type, angle, chartWeaponsCount);
                }
                break;
            case WeaponType.SetSquare:
                if (numberOfSetSquareWeapons == 0)
                {
                    GenerateWeapon(type);
                }
                numberOfSetSquareWeapons++;
                break;
            case WeaponType.Portion:
                if (numberOfPortionWeapons == 0)
                {
                    GenerateWeapon(type);
                }
                numberOfPortionWeapons++;
                break;
        }
    }

    //レート上昇
    public void DecreaseAttackInterval(WeaponType type, float dtime)
    {
        switch (type) 
        {
            case WeaponType.Suica:
                suicaFireRate = suicaFireRate - dtime;
                break;
            case WeaponType.Laser:
                laserFireRate = laserFireRate - dtime;
                break;
            case WeaponType.Chart:
                chartRotateFireRate = chartRotateFireRate * dtime;

                for (int i = 0; i < numberOfChartWeapons; i++)
                {
                    _ChartWeapon[i].rotationSpeed = chartRotateFireRate;
                }
                break;
            case WeaponType.SetSquare:
                setSquareFireRate = setSquareFireRate - dtime;
                break;
            case WeaponType.Portion:
                portionFireRate = portionFireRate - dtime;
                break;
        }
    }

    //ダメージ上昇
    public void IncreaseDamage(WeaponType type, float ddamage)
    {
        switch (type)
        {
            case WeaponType.Suica:
                suicaDamage = suicaDamage + ddamage;
                break;
            case WeaponType.Laser:
                laserDamage = laserDamage + ddamage;
                break;
            case WeaponType.Chart:
                chartDamage = chartDamage + ddamage;

                for (int i = 0; i < numberOfChartWeapons; i++)
                {
                    _ChartWeapon[i].damage = chartDamage;
                }
                break;
            case WeaponType.SetSquare:
                setSquareDamage = setSquareDamage + ddamage;
                break;
            case WeaponType.Portion:
                portionDamage = portionDamage + ddamage;
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
            StartCoroutine(FireSuica());
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
            StartCoroutine(FireSetSquare());
            nextSetSquareFireTime = Time.time + setSquareFireRate;
        }

        //ポーションを発射するタイミングをチェック
        if (Time.time > nextPortionFireTime)
        {
            StartCoroutine(FirePortion());
            nextPortionFireTime = Time.time + portionFireRate;
        }
    }

    private IEnumerator FireSuica()
    {
        Vector2 direction = (attackTarget.position - transform.position).normalized;

        if(WeaponCount(WeaponType.Suica) > 0)
        {
            for(int i = 0; i < numberOfSuicaWeapons; i++)
            {
                // Suicaを発射
                _SuicaWeapon.Fire(direction, transform, suicaSpeed,suicaDamage);

                SEAudio.Instance.PlayOneShot(fireSuicaSoundClip, 0.4f);

                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    void FireLaser()
    {
        if (WeaponCount(WeaponType.Laser) > 0)
        {
            // レーザービームを発射
            for(int i = 0; i < numberOfLaserWeapons; i++)
            {
                _LaserWeapon[i].Fire(laserDamage);
            }

            SEAudio.Instance.PlayOneShot(laserBeamSoundClip, 0.15f);
        }
    }

    private IEnumerator FireSetSquare()
    {
        if (WeaponCount(WeaponType.SetSquare) > 0)
        {
            Vector2 direction = (attackTarget.position - transform.position).normalized;

            for (int i = 0; i < numberOfSetSquareWeapons; i++)
            {
                _SetSquareWeapon.Fire(direction, transform, setSquareSpeed, setSquareDamage);
                SEAudio.Instance.PlayOneShot(fireSetSquareSoundClip, 0.15f);

                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerator FirePortion()
    {
        if (WeaponCount(WeaponType.Portion) > 0)
        {
            for (int i = 0; i < numberOfPortionWeapons; i++)
            {
                _PortionWeapon.Fire(attackTarget.position, transform, portionSpeed, portionDamage);
                SEAudio.Instance.PlayOneShot(firePortionSoundClip, 0.4f);

                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerator PlayChartRotateSound()
    {
        while (WeaponCount(WeaponType.Chart) > 0)
        {
            yield return new WaitForSeconds(0.5f);
            SEAudio.Instance.PlayOneShot(chartRotateSoundClip,0.3f);
        }
    }

    public int WeaponCount(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Suica:
                return numberOfSuicaWeapons;
            case WeaponType.Laser:
                return numberOfLaserWeapons;
            case WeaponType.Chart:
                return numberOfChartWeapons;
            case WeaponType.SetSquare:
                return numberOfSetSquareWeapons;
            case WeaponType.Portion:
                return numberOfPortionWeapons;
        }
        return 0;
    }

    public bool IsFullWeapon(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Suica:
                return numberOfSuicaWeapons >= maxSuicaCount;
            case WeaponType.Laser:
                return numberOfLaserWeapons >= maxLaserCount;
            case WeaponType.Chart:
                return numberOfChartWeapons >= maxChartCount;
            case WeaponType.SetSquare:
                return numberOfSetSquareWeapons >= maxSetSquareCount;
            case WeaponType.Portion:
                return numberOfPortionWeapons >= maxPortionCount;
        }
        return false;
    }

    public bool IsMaxRateIncrease(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Suica:
                return suicaFireRate <= 0.5f;
            case WeaponType.Laser:
                return laserFireRate <= 0.2f;
            case WeaponType.Chart:
                return chartRotateFireRate >= 500;
            case WeaponType.SetSquare:
                return setSquareFireRate <= 1f;
            case WeaponType.Portion:
                return portionFireRate <= 0.8f;
        }
        return false;
    }
}
