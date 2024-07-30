using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StrengtheningManager : MonoBehaviour
{
    public PlayerAttack _PlayerAttack;
    public QuestionManager _QuestionManager;
    public List<StrengtheningDetails> strengtheningDetailsList = new List<StrengtheningDetails>();
    private List<StrengtheningDetails> canSelectStrengtheningDetailsList = new List<StrengtheningDetails>();
    public int strengtheningLevel = 5;
    public int showCount;

    private List<StrengtheningDetails> selectedStrengtheningDetailsList = new List<StrengtheningDetails>();

    [Header("UI")]
    public GameObject StrengtheningCanvas;
    public Image[] weaponTypeImages;
    public Image[] infoImages;
    public Sprite[] infoSprites;
    public Text[] texts;

    private void Start()
    {
        StrengtheningCanvas.SetActive(false);
    }

    //���ɑ��݂��镐��̋������ł���List���쐬
    public void AddCanSelectStrengtheningDetails(WeaponType type)
    {
        for (int j = 0; j < strengtheningDetailsList.Count; j++)
        {
            if (strengtheningDetailsList[j].WeaponType == type)
            {
                // ���X�g�ɗv�f��������Βǉ�
                if (!canSelectStrengtheningDetailsList.Contains(strengtheningDetailsList[j]))
                {
                    canSelectStrengtheningDetailsList.Add(strengtheningDetailsList[j]);
                }
            }
        } 
    }

    //���x���A�b�v�����Ƃ�
    public void StartStrengthening(int currentLevel)
    {
        //��背�x���Ԋu�ŌĂяo��
        if (currentLevel % strengtheningLevel == 0)
        {
            StrengtheningCanvas.SetActive(true);
            GameManager.Instance.PauseGame();

            SelectStrengtheningDetails();
            ShowStrengtheningDetails();
        }
    }

    private void SelectStrengtheningDetails()
    {
        //���ꂼ��̃��X�g�������_���ɕ��ёւ�(�d�����)
        canSelectStrengtheningDetailsList = canSelectStrengtheningDetailsList.OrderBy(x => Guid.NewGuid()).ToList();
        strengtheningDetailsList = strengtheningDetailsList.OrderBy(x => Guid.NewGuid()).ToList();

        //�\���񐔂����\�����e�����肷��
        for (int i = 0; i < showCount; i++)
        {
            int r = UnityEngine.Random.Range(0, 10);

            //�\���񐔂̂ق����������Ă镐�퐔��葽���Ƃ��͒ǉ��܂��͋����܂��̓��[�g�㏸
            if (i >= canSelectStrengtheningDetailsList.Count)
            {
                int n = 0;

                //���퐔���ő�l�ɓ��B���Ă邩�m�F
                if (_PlayerAttack.IsFullWeapon(strengtheningDetailsList[i].WeaponType))
                {
                    Debug.Log($"Full! : {strengtheningDetailsList[i].WeaponType}");
                    //���[�g�㏸�܂��͋����Ɍ���
                    n = UnityEngine.Random.Range(1, 3);
                }

                //�ǉ��܂��͋����܂��̓��[�g�㏸
                AddDetail(strengtheningDetailsList, i, n);
                continue;
            }


            //�ǉ��A���[�g�㏸�A�����������_���Ō��߂�
            if (r > 7)  //8,9
            {
                int n = 0;

                //���퐔���ő�l�ɓ��B���Ă邩�m�F
                if (_PlayerAttack.IsFullWeapon(strengtheningDetailsList[i].WeaponType))
                {
                    Debug.Log($"Full! : {strengtheningDetailsList[i].WeaponType}");
                    //���[�g�㏸�܂��͋����Ɍ���
                    n = UnityEngine.Random.Range(1, 3);
                }

                //�ǉ��܂��͋����܂��̓��[�g�㏸
                AddDetail(strengtheningDetailsList, i, n);
            }
            else if (r > 3) //4,5,6,7
            {
                //���[�g�㏸
                AddDetail(canSelectStrengtheningDetailsList, i, 1);
            }
            else //0,1,2,3
            {
                //����
                AddDetail(canSelectStrengtheningDetailsList, i, 2);
            }
        }
    }

    //selectedStrengtheningDetailsList�ɑI�΂ꂽ�v�f��ǉ�
    private void AddDetail(List<StrengtheningDetails> sourceList, int index, int state)
    {
        StrengtheningDetails a = new StrengtheningDetails
        {
            state = state,
            WeaponType = sourceList[index].WeaponType,
            Sprite = sourceList[index].Sprite,
            decreaseAttackIntervalRatio = sourceList[index].decreaseAttackIntervalRatio
        };
        selectedStrengtheningDetailsList.Add(a);
    }

    private void ShowStrengtheningDetails()
    {
        //�\��
        for (int n = 0; n < showCount; n++)
        {
            weaponTypeImages[n].sprite = selectedStrengtheningDetailsList[n].Sprite;

            if (selectedStrengtheningDetailsList[n].state == 0)
            {
                texts[n].text = "�ǉ�";
                if (!_PlayerAttack.IsExistWeapon(selectedStrengtheningDetailsList[n].WeaponType))
                {
                    //New Weapon
                    infoImages[n].sprite = infoSprites[0];
                }
                else
                {
                    infoImages[n].sprite = infoSprites[1];
                }
            }
            else if (selectedStrengtheningDetailsList[n].state == 1)
            {
                texts[n].text = "���[�g�㏸";
                infoImages[n].sprite = infoSprites[2];
            }
            else
            {
                texts[n].text = "����";
                infoImages[n].sprite = infoSprites[3];
            }
        }
    }

    //�{�^�����������Ƃ�
    public void OnClickStrengtheningButton(int k)
    {
        StrengtheningCanvas.SetActive(false);

        var clickedData = selectedStrengtheningDetailsList[k];

        switch (clickedData.state)
        {
            case 0:
                //�ǉ�
                _PlayerAttack.AddWeapon(clickedData.WeaponType);
                LogManager.Instance.AddLogs($"{clickedData.WeaponType}����肵���I");
                break;
            case 1:
                //���[�g�㏸
                _PlayerAttack.DecreaseAttackInterval(clickedData.WeaponType, clickedData.decreaseAttackIntervalRatio);
                LogManager.Instance.AddLogs($"{clickedData.WeaponType}�̃��[�g���㏸�����I");
                break;
            case 2:
                //�U���͑���
                LogManager.Instance.AddLogs($"{clickedData.WeaponType}�̍U���͂����������I");
                break;
        }

        selectedStrengtheningDetailsList.Clear();

        LogManager.Instance.AddLogs($"�܂��Ȃ���肪����...");
        LogManager.Instance.AddLogs($"���������炢�����Ƃ��邩���I\n�s������������...");
        //FadeOut
        FadeUI.Instance.StartFadeOut(6.8f);

        //Log
        LogManager.Instance.Log(2, () =>
        {
            _QuestionManager.StartQuestion();
        }); 
    }
}
