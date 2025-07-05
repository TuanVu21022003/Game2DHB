using UnityEngine;

public static class GameConstants 
{
    public static int Level
    {
        get
        {
            return PlayerPrefs.GetInt("Level", 1);
        }
        set
        {
            PlayerPrefs.SetInt("Level", value);
        }
    }
}
