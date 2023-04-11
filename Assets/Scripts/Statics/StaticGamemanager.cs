using Newtonsoft.Json;
using UnityEngine;

public static class StaticGamemanager
{
    public static GameDataStructure gameDataStructure = new();
    static StaticConstructor staticConstructor = new();
    public static S_Collections s_Collections;

    public static void loadGameDataStructure()
    {
        string loadedString = PlayerPrefs.GetString("GameDataStructure");
        if (loadedString.Length > 0)
        {
            try
            {
                gameDataStructure = JsonConvert.DeserializeObject<GameDataStructure>(loadedString);
            }
            catch (System.Exception)
            {
                SaveGameDataStructure(true);
            }
        }
        else
        {
            SaveGameDataStructure(true);
        }

        s_Collections = Resources.Load<S_Collections>("S_Collections");
    }

    public static void SaveGameDataStructure(bool newObj = false)
    {
        if (gameDataStructure == null || newObj)
        {
            gameDataStructure = new();
            Debug.LogError("newObj : " + newObj + "," + gameDataStructure);
        }

        string serializedData = JsonConvert.SerializeObject(gameDataStructure);
        PlayerPrefs.SetString("GameDataStructure", serializedData);
    }
}


public class StaticConstructor
{
    public StaticConstructor()
    {
        StaticGamemanager.loadGameDataStructure();
    }
}