using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    public int experiencePoints = 0;
    public int currentLevel = 1;
    public int experienceToNextLevel = 100;
    public Text levelText;
    public Slider experienceSlider;

    void Start()
    {
        UpdateExperienceUI();
    }

    public void AddExperience(int amount)
    {
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
        Debug.Log("Level Up! Current Level: " + currentLevel);
        UpdateExperienceUI();
    }

    private int CalculateExperienceForNextLevel()
    {
        return currentLevel * 2;
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
