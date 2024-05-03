using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public LevelSystem levelSystem;
    public LevelWindow levelWindow;

    private void OnEnable()
    {
        DamageFlash.OnEnemyDeath += HandleEnemyDeath;
        levelWindow.SetLevelSystem(levelSystem);
    }

    private void OnDisable()
    {
        DamageFlash.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(object sender, System.EventArgs e)
    {
        levelSystem.AddExperience(10);
    }
}
