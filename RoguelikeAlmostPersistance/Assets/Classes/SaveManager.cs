using System.IO;
using UnityEngine;
using System.Collections.Generic;

public static class SaveManager
{
    public static void SavePlayer(PlayerStats data)
    {
        if (string.IsNullOrEmpty(data.nombre))
        {
            Debug.LogError("No se puede guardar: nombre vacío");
            return;
        }

        string json = JsonUtility.ToJson(data, true);
        string encrypted = EncryptionUtility.Encrypt(json);

        string fileName = $"player_{data.nombre}.json";
        string path = Path.Combine(Application.persistentDataPath, fileName);

        File.WriteAllText(path, encrypted);

        Debug.Log($" Partida guardada: {fileName}");
    }


    public static PlayerStats LoadPlayer(string path)
    {
        string encrypted = File.ReadAllText(path);
        string json = EncryptionUtility.Decrypt(encrypted);
        return JsonUtility.FromJson<PlayerStats>(json);
    }



    public static void SaveAchievements( List<Achievement> data)
    {
            AchievementWrapper achievement = new AchievementWrapper();

            achievement.Achievements = data;

            string json = JsonUtility.ToJson(achievement, true);
            string encrypted = EncryptionUtility.Encrypt(json);

            string path = Path.Combine(Application.persistentDataPath, "global_achievements.json");

        File.WriteAllText(path, encrypted);

        Debug.Log("Achievements guardados correctamente");

    }


    public static AchievementWrapper LoadAchievements()
    {

        string path = Path.Combine(Application.persistentDataPath, "global_achievements.json");

        if (!File.Exists(path))
            return null;

        string encrypted = File.ReadAllText(path);
        string json = EncryptionUtility.Decrypt(encrypted);

        return JsonUtility.FromJson<AchievementWrapper>(json);
    }






}
