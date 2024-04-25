using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    // Data model representing player's progress
    [System.Serializable]
    public class PlayerData
    {
        public int moneyAmount;
        public int purchasedWeaponIndex;
    }

    private string savePath;

    void Awake()
    {
        savePath = Application.persistentDataPath + "/save.json";
    }

    public void Save(PlayerData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, jsonData);
    }

    public PlayerData Load()
    {
        if (File.Exists(savePath))
        {
            string jsonData = File.ReadAllText(savePath);
            return JsonUtility.FromJson<PlayerData>(jsonData);
        }
        else
        {
            return null;
        }
    }
}