using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAttack : MonoBehaviour
{
    public Player _Player;
    public Transform weaponSpawnPoint;
    public StrengtheningManager _StrengtheningManager;

    [Header("Suica")]
    public GameObject suicaWeaponPrefab;
    private SuicaWeapon _SuicaWeapon;
    public float suicaFireRate = 0.5f;
    public int maxSuicaCount;
    private float nextSuicaFireTime = 0f;
    private int numberOfSuicaWeapons = 1;
    [Header("LaserBeam")]
    public GameObject laserWeaponPrefab;
    public List<LaserWeapon> _LaserWeapon = new List<LaserWeapon>();
    public float laserFireRate = 0.75f;
    public int maxLaserCount;
    private float nextLaserFireTime = 0f;
    private int numberOfLaserWeapons = 0;
    [Header("Chart")]
    public GameObject chartWeaponPrefab;
    public List<ChartWeapon> _ChartWeapon = new List<ChartWeapon>();
    public int maxChartCount;
    private List<GameObject> chartWeaponPrefabs = new List<GameObject>();
    private float nextRotationSpeed = 200f;
    private int numberOfChartWeapons = 0;
    [Header("SetSquare")]
    public GameObject setSquareWeaponPrefab;
    private SetSquareWeapon _SetSquareWeapon;
    public float setSquareFireRate = 0.5f;
    public int maxSetSquareCount;
    private float nextSetSquareFireTime = 0f;
    private int numberOfSetSquareWeapons = 0;
    [Header("Portion")]
    public GameObject portionWeaponPrefab;
    private PortionWeapon _PortionWeapon;
    public float portionFireRate = 5f;
    public int maxPortionCount;
    private float nextPortionFireTime = 0f;
    private int numberOfPortionWeapons = 0;

    [Header("Audio")]
    public AudioClip chartRotateSoundClip;
    public AudioClip laserBeamSoundClip;
    public AudioClip fireSuicaSoundClip;

    //初期武器
    public void GenerateInitialWeapon()
    {
        GenerateWeapon(WeaponType.Suica);
    }

    private void GenerateWeapon(WeaponType weaponType, float startingAngle = 0f, int chartWeaponsCount = 0,int lazerWeaponsCount = 0)
    {
        GameObject weaponPrefab = null;
        _StrengtheningManager.AddCanSelectStrengtheningDetails(weaponType);

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
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;

        if(IsExistWeapon(WeaponType.Suica))
        {
            for(int i = 0; i < numberOfSuicaWeapons; i++)
            {
                // Suicaを発射
                _SuicaWeapon.Fire(direction, transform);

                PlayerAudio.Instance.PlayOneShot(fireSuicaSoundClip, 0.4f);

                yield return new WaitForSeconds(0.1f);
            }
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

    private IEnumerator FireSetSquare()
    {
        if (IsExistWeapon(WeaponType.SetSquare))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - transform.position).normalized;

            for (int i = 0; i < numberOfSetSquareWeapons; i++)
            {
                _SetSquareWeapon.Fire(direction, transform);
                //TODO:Audio

                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerator FirePortion()
    {
        if (IsExistWeapon(WeaponType.Portion))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            for (int i = 0; i < numberOfPortionWeapons; i++)
            {
                _PortionWeapon.Fire(mousePosition, transform);
                //TODO:Audio

                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerator PlayChartRotateSound()
    {
        while (IsExistWeapon(WeaponType.Chart))
        {
            yield return new WaitForSeconds(0.5f);
            PlayerAudio.Instance.PlayOneShot(chartRotateSoundClip,0.3f);
        }
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

    public bool IsExistWeapon(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Suica:
                return numberOfSuicaWeapons > 0;
            case WeaponType.Laser:
                return numberOfLaserWeapons > 0;
            case WeaponType.Chart:
                return numberOfChartWeapons > 0;
            case WeaponType.SetSquare:
                return numberOfSetSquareWeapons > 0;
            case WeaponType.Portion:
                return numberOfPortionWeapons > 0;
        }
        return false;
    }
}
