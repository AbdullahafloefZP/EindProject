using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelWindow : MonoBehaviour
{
    public Text level;
    public Image expBarImage;
    public LevelSystem levelSystem;

    private void Awake()
    {
        if (levelSystem == null)
        {
            levelSystem = FindObjectOfType<LevelSystem>();
        }
        if (level == null)
        {
            level = transform.Find("level").GetComponent<Text>();
        }
        if (expBarImage == null)
        {
            expBarImage = transform.Find("ExpBar").Find("Bar").GetComponent<Image>();
        }
    }


    private void Start()
    {
        if (levelSystem == null)
        {
            return;
        }
        SetLevelSystem(levelSystem);

        SetLevel(levelSystem.GetLevelAmount());
        SetExpSize(levelSystem.GetExperienceNormalized());
    }



    private void SetExpSize(float expNormalized)
    {
        expBarImage.fillAmount = expNormalized;
    }

    private void SetLevel(int levelNumber)
    {
        level.text = "Level " + levelNumber;
    }

    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;
        SetLevel(levelSystem.GetLevelAmount());
        SetExpSize(levelSystem.GetExperienceNormalized());

        this.levelSystem.onExpChanged += LevelSystem_onExpChanged;
        this.levelSystem.onLevelChanged += LevelSystem_onLevelChanged;
    }

    private void LevelSystem_onExpChanged(object sender, System.EventArgs e)
    {
        SetExpSize(levelSystem.GetExperienceNormalized());
    }

    private void LevelSystem_onLevelChanged(object sender, System.EventArgs e)
    {
        SetLevel(levelSystem.GetLevelAmount());
    }

    private void OnDestroy() 
    {
        if (levelSystem != null) 
        {
            levelSystem.onExpChanged -= LevelSystem_onExpChanged;
            levelSystem.onLevelChanged -= LevelSystem_onLevelChanged;
        }
    }
}
