using UnityEngine;

public static class SaveSystem
{
    public static void UnlockLevel(int levelIndex)
    {
        int current = GetHighestUnlockedLevel();
        if (levelIndex > current)
        {
            PlayerPrefs.SetInt("UnlockedLevel", levelIndex);
            PlayerPrefs.Save();
        }
    }

    public static int GetHighestUnlockedLevel()
    {
        return PlayerPrefs.GetInt("UnlockedLevel", 1);
    }

    public static void SaveLevelStats(int level, float time)
    {
        string key = "Level" + level;
        float bestTime = PlayerPrefs.GetFloat(key + "_BestTime", float.MaxValue);
        if (time < bestTime)
            PlayerPrefs.SetFloat(key + "_BestTime", time);
        PlayerPrefs.Save();
    }

    public static float GetLevelBestTime(int level)
    {
        return PlayerPrefs.GetFloat("Level" + level + "_BestTime", float.MaxValue);
    }

    public static void ClearAll()
    {
        PlayerPrefs.DeleteAll();
    }
}
