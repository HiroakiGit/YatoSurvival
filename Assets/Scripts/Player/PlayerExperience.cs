using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    public BuffAndDeBuffManager _BuffAndDeBuff;
    public int experiencePoints = 0;
    public int currentLevel = 1;
    public int experienceToNextLevel = 100;
    private int count = 0;
    public Text levelText;
    public Slider experienceSlider;

    [Header("Audio")]
    public AudioClip getExperienceSoundClip;

    void Start()
    {
        UpdateExperienceUI();
    }

    public void AddExperience(int amount)
    {
        SEAudio.Instance.PlayOneShot(getExperienceSoundClip, 0.1f);

        experiencePoints += amount;
        CheckLevelUp();
        UpdateExperienceUI();
    }

    private void CheckLevelUp()
    {
        if (experiencePoints >= experienceToNextLevel)
        {
            experiencePoints -= experienceToNextLevel;
            currentLevel++;
            experienceToNextLevel = CalculateExperienceForNextLevel();
            OnLevelUp();
        }
    }

    private void OnLevelUp()
    {
        UpdateExperienceUI();

        _BuffAndDeBuff.StartWeaponBuffProcess(currentLevel);
    }

    private int CalculateExperienceForNextLevel()
    {
        if(currentLevel % 3 == 0) count++;
        return experienceToNextLevel + 3 + count;
    }

    private void UpdateExperienceUI()
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + currentLevel;
        }
        if (experienceSlider != null)
        {
            experienceSlider.maxValue = experienceToNextLevel;
            experienceSlider.value = experiencePoints;
        }
    }
}
