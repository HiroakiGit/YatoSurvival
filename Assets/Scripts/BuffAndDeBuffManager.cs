using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuffAndDeBuffManager : MonoBehaviour
{
    public PlayerAttack _PlayerAttack;
    public QuestionManager _QuestionManager;

    [Header("WeaponBuff")]
    [Space(10)]
    public List<WeaponBuff> weaponBuffList = new List<WeaponBuff>();
    private List<WeaponBuff> canSelectWeaponBuffList = new List<WeaponBuff>();
    public int weaponBuffLevel = 5;
    [Range(0, 1)] public float probabilityAddWeapon = 0.2f;
    [Range(0, 1)] public float probabilityRateIncreaseWeapon = 0.4f;
    [Range(0, 1)] public float probabilityStrengthenWeapon = 0.4f;
    public int showCount;
    private List<WeaponBuff> selectedWeaponBuffList = new List<WeaponBuff>();
    [Space(30)]

    [Header("DeBuff")]
    [Space(10)]
    public Player _Player;
    public EnemySpawnerManager _EnemySpawnerManager;
    public PlayerHealth _PlayerHealth;
    public List<DeBuff> deBuffList = new List<DeBuff>();
    public float decreaseMoveSpeedRATIO;
    public float increaseEnemyDamageRATIO;
    public float decreaseHpRATIO;
    private List<DeBuff> activeDebuffList = new List<DeBuff>();
    public float duration = 30f;
    [Space(30)]

    [Header("WeaponBuffUI")]
    [Space(10)]
    public GameObject BuffCanvas;
    public Image[] weaponTypeImages;
    public Image[] infoImages;
    public Sprite[] infoSprites;
    public Text[] texts;
    [Space(30)]

    [Header("DeBuffUI")]
    [Space(10)]
    public GameObject BlindCanvas;
    [Space(30)]

    [Header("BuffAndDeBuffStateUI")]
    [Space(10)]
    public GameObject BuffAndDeBuffStateCanvas;
    public GameObject BuffAndDeBuffStateUIPrefab;
    public Transform BuffAndDeBuffStateParent;

    private void Start()
    {
        BuffCanvas.SetActive(false);
        BlindCanvas.SetActive(false);
        BuffAndDeBuffStateCanvas.SetActive(false);
    }

    //既に存在する武器の強化ができるListを作成
    public void AddCanSelectWeaponBuffList(WeaponType type)
    {
        for (int j = 0; j < weaponBuffList.Count; j++)
        {
            if (weaponBuffList[j].WeaponType == type)
            {
                // リストに要素が無ければ追加
                if (!canSelectWeaponBuffList.Contains(weaponBuffList[j]))
                {
                    canSelectWeaponBuffList.Add(weaponBuffList[j]);
                }
            }
        } 
    }

    //レベルアップしたとき
    public void StartWeaponBuffProcess(int currentLevel)
    {
        //一定レベル間隔で呼び出す
        if (currentLevel % weaponBuffLevel == 0)
        {
            BuffCanvas.SetActive(true);
            GameManager.Instance.PauseGame(false);
            GameManager.Instance.isProcessing = true;

            SelectWeaponBuff();
        }
    }

    private void SelectWeaponBuff()
    {
        //それぞれのリストをランダムに並び替え(重複回避)
        canSelectWeaponBuffList = canSelectWeaponBuffList.OrderBy(x => Guid.NewGuid()).ToList();
        weaponBuffList = weaponBuffList.OrderBy(x => Guid.NewGuid()).ToList();

        //表示回数だけ表示内容を決定する
        for (int i = 0; i < showCount; i++)
        {
            float r = UnityEngine.Random.Range(0f, 1f);

            //表示回数のほうが今持ってる武器数より多いときは追加または強化またはレート上昇
            if (i >= canSelectWeaponBuffList.Count)
            {
                int n = 0;

                //武器数が最大値に到達してるか確認
                if (_PlayerAttack.IsFullWeapon(weaponBuffList[i].WeaponType))
                {
                    Debug.Log($"Full! : {weaponBuffList[i].WeaponType}");
                    //レート上昇または強化に限る
                    n = UnityEngine.Random.Range(1, 3);
                }

                //追加または強化またはレート上昇
                AddBuff(weaponBuffList, i, n);
                continue;
            }


            //追加、レート上昇、強化をランダムで決める
            if (r <= probabilityAddWeapon) //0 ~ 0.2f
            {
                //====追加====
                int n = 0;

                //武器数が最大値に到達してるか確認
                if (_PlayerAttack.IsFullWeapon(weaponBuffList[i].WeaponType))
                {
                    Debug.Log($"Full! : {weaponBuffList[i].WeaponName}");

                    //レート上昇または強化に限る
                    n = UnityEngine.Random.Range(1, 3);

                    //武器のレートが最大値に到達してるか確認
                    if (_PlayerAttack.IsMaxRateIncrease(weaponBuffList[i].WeaponType))
                    {
                        //強化に限る
                        n = 2;
                    }
                }

                //追加または強化またはレート上昇
                AddBuff(weaponBuffList, i, n);
            }
            else if (probabilityAddWeapon < r && r <= (probabilityAddWeapon + probabilityRateIncreaseWeapon)) //0.2 ~ 0.6
            {
                //====レート上昇====
                int n = 1;

                //武器のレートが最大値に到達してるか確認
                if (_PlayerAttack.IsMaxRateIncrease(weaponBuffList[i].WeaponType))
                {
                    Debug.Log($"MaxRateIncrease! : {weaponBuffList[i].WeaponName}");

                    //追加または強化に限る
                    var result = Enumerable.Range(0, 3) //0〜2生成
                        .Where(e => e != 1) //1以外のものを抽出
                        .OrderBy(e => Guid.NewGuid()) //ランダムに並び替え
                        .First(); //先頭の要素を取得

                    n = result;

                    //武器数が最大値に到達してるか確認
                    if (_PlayerAttack.IsFullWeapon(weaponBuffList[i].WeaponType))
                    {
                        //強化に限る
                        n = 2;
                    }

                }

                //レート上昇または追加または強化
                AddBuff(canSelectWeaponBuffList, i, n);
            }
            else //0.6 ~ 1
            {
                //====強化====
                AddBuff(canSelectWeaponBuffList, i, 2);
            }
        }

        //武器バフを選んだ後、表示
        ShowWeaponBuff();
    }

    //selectedStrengtheningDetailsListに選ばれた要素を追加
    private void AddBuff(List<WeaponBuff> sourceList, int index, int state)
    {
        WeaponBuff a = new WeaponBuff
        {
            state = state,
            WeaponType = sourceList[index].WeaponType,
            Sprite = sourceList[index].Sprite,
            increaseRate = sourceList[index].increaseRate
        };
        selectedWeaponBuffList.Add(a);
    }

    private void ShowWeaponBuff()
    {
        //表示
        for (int n = 0; n < showCount; n++)
        {
            weaponTypeImages[n].sprite = selectedWeaponBuffList[n].Sprite;

            if (selectedWeaponBuffList[n].state == 0)
            {
                texts[n].text = "追加";
                if (!(_PlayerAttack.WeaponCount(selectedWeaponBuffList[n].WeaponType) > 0))
                {
                    //New Weapon
                    infoImages[n].sprite = infoSprites[0];
                }
                else
                {
                    infoImages[n].sprite = infoSprites[1];
                }
            }
            else if (selectedWeaponBuffList[n].state == 1)
            {
                texts[n].text = "レート上昇";
                infoImages[n].sprite = infoSprites[2];
            }
            else
            {
                texts[n].text = "強化";
                infoImages[n].sprite = infoSprites[3];
            }
        }
    }

    //ボタンを押したとき
    public void OnClickWeaponBuffButton(int k)
    {
        BuffCanvas.SetActive(false);

        var clickedData = selectedWeaponBuffList[k];
        int n = 0;

        for (int j = 0; j < weaponBuffList.Count; j++)
        {
            if (weaponBuffList[j].WeaponType == clickedData.WeaponType)
            {
                n = j;
            }
        }

        switch (clickedData.state)
        {
            case 0:
                //追加
                _PlayerAttack.AddWeapon(clickedData.WeaponType);
                LogManager.Instance.AddLogs($"{clickedData.WeaponName}を入手した！");
                break;
            case 1:
                //レート上昇
                _PlayerAttack.DecreaseAttackInterval(clickedData.WeaponType, clickedData.increaseRate);
                weaponBuffList[n].BuffStateCountUp(1,0);
                LogManager.Instance.AddLogs($"{clickedData.WeaponName}のレートが上昇した！");
                break;
            case 2:
                //攻撃力増加
                _PlayerAttack.IncreaseDamage(clickedData.WeaponType, clickedData.increaseDamage);
                weaponBuffList[n].BuffStateCountUp(0,1);
                LogManager.Instance.AddLogs($"{clickedData.WeaponName}の攻撃力が増加した！");
                break;
        }

        //左上簡易表示
        ShowBuffState();

        selectedWeaponBuffList.Clear();

        LogManager.Instance.AddLogs($"まもなく問題が来る...");
        LogManager.Instance.AddLogs($"正解したらいいことあるかも！\r\n不正解だったら...");
        //TODO : 下の内容
        //FadeOut
        FadeUI.Instance.StartFadeOut(1f);//6.8s

        //Log
        LogManager.Instance.Log(.1f, () => //2s
        {
            _QuestionManager.StartQuestion();
        }); 
    }

    //DEBUFF=======================================================================
    public void StartDeBuffProcess()
    {
        SelectDeBuff();
    }

    private void SelectDeBuff()
    {
        //デバフをランダムに選ぶ
        int r = UnityEngine.Random.Range(0, deBuffList.Count);
        var debuff = deBuffList[r];

        //デバフを選んだ後、実行
        ApplyingDebuff(debuff);
    }

    private void ApplyingDebuff(DeBuff debuff)
    {
        LogManager.Instance.AddLogs(debuff.Name);
        LogManager.Instance.Log(2f, null);

        if (debuff.DeBuffType == DeBuffType.DecreaseHP)
        {
            StartDeBuff(debuff);
            return;
        }

        if (activeDebuffList.Contains(debuff))
        {
            Debug.Log("もうある");
            StartCoroutine(ApplyDebuffCoroutine(debuff, true, duration));
        }
        else
        {
            activeDebuffList.Add(debuff);
            StartCoroutine(ApplyDebuffCoroutine(debuff, false, duration));
        }
    }

    private IEnumerator ApplyDebuffCoroutine(DeBuff debuff, bool alreadyExist, float duration)
    {
        GameObject stateUI = null;

        if (alreadyExist)
        {
            //すでにリストの中にある
            stateUI = debuff.stateUI;
        }
        else 
        {
            stateUI = Instantiate(BuffAndDeBuffStateUIPrefab, BuffAndDeBuffStateParent);
            debuff.stateUI = stateUI;
            StartDeBuff(debuff);
        }

        stateUI.transform.GetChild(1).GetComponent<Image>().sprite = debuff.Sprite;
        Slider debuffDurationSlider = stateUI.transform.GetChild(2).GetComponent<Slider>();

        if (alreadyExist)
        {
            //時間延長
            debuff.duration = debuff.duration + duration;
        }
        else
        {
            debuff.duration = duration;
        }

        debuffDurationSlider.maxValue = debuff.duration;

        float elapsed = 0f;
        while (elapsed < debuff.duration)
        {
            elapsed += Time.deltaTime;
            debuffDurationSlider.value = debuff.duration - elapsed;
            yield return null;
        }

        FinishDeBuff(debuff);
        Destroy(stateUI);
        activeDebuffList.Remove(debuff);
    }

    private void StartDeBuff(DeBuff debuff)
    {
        switch (debuff.DeBuffType)
        {
            case DeBuffType.WalkSlow:
                _Player._PlayerController.ChangeSpeed(decreaseMoveSpeedRATIO);
                break;
            case DeBuffType.Blind:
                BlindCanvas.SetActive(true);
                break;
            case DeBuffType.IncreaseEnemyDamage:
                _EnemySpawnerManager.ChangeEnemyDamageRATIO(increaseEnemyDamageRATIO);
                break;
            case DeBuffType.DecreaseHP:
                _PlayerHealth.TakeDamage(_PlayerHealth.currentHealth * decreaseHpRATIO);
                break;
        }
    }

    private void FinishDeBuff(DeBuff debuff)
    {
        switch (debuff.DeBuffType)
        {
            case DeBuffType.WalkSlow:
                _Player._PlayerController.ChangeSpeed(1);
                break;
            case DeBuffType.Blind:
                BlindCanvas.SetActive(false);
                break;
            case DeBuffType.IncreaseEnemyDamage:
                _EnemySpawnerManager.ChangeEnemyDamageRATIO(1);
                break;
        }

        debuff.duration = 0;
    }

    //SHOW STATE===============================================================
    private void ShowBuffState()
    {
        for (int i = 0; i < weaponBuffList.Count; i++) 
        {
            weaponBuffList[i].ShowBuffState(_PlayerAttack.WeaponCount(weaponBuffList[i].WeaponType));
        }
    }
}
