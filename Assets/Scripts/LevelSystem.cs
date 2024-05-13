using System;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public event EventHandler onExpChanged;
    public event EventHandler onLevelChanged;

    private int level = 0;
    private int experience = 0;
    private int experienceNextLevel = 100;

    private void Awake()
    {
        Load();
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        while (experience >= experienceNextLevel)
        {
            level++;
            experience -= experienceNextLevel;
            onLevelChanged?.Invoke(this, EventArgs.Empty);
        }
        Save();
        onExpChanged?.Invoke(this, EventArgs.Empty);
    }


    private void Save()
    {
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("experience", experience);
        PlayerPrefs.SetInt("experienceNextLevel", experienceNextLevel);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        level = PlayerPrefs.GetInt("level", 0);
        experience = PlayerPrefs.GetInt("experience", 0);
        experienceNextLevel = PlayerPrefs.GetInt("experienceNextLevel", 100);
    }

    public int GetLevelAmount()
    {
        return level;
    }

    public float GetExperienceNormalized()
    {
        return (float)experience / experienceNextLevel;
    }

    public void ResetLevel() 
    {
        level = 0;
        experience = 0;
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("experience", experience);

        onLevelChanged?.Invoke(this, EventArgs.Empty);
        onExpChanged?.Invoke(this, EventArgs.Empty);

        PlayerPrefs.Save();
    }
}
