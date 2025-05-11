using UnityEngine;
using TMPro;
using System;
using System.IO;

public class HighScoreText : MonoBehaviour
{
    public TMP_Text txt;
    private static string path;
    public static int highScore;

    public static void SaveHighScore(int score)
    {
        HighScoreData data = new HighScoreData { highScore = score };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public static int LoadHighScore()
    {
        path = Application.persistentDataPath + "/highscore.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
            highScore = data.highScore;
            return data.highScore;
        }
        highScore = 0;
        return 0;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        path = Application.persistentDataPath + "/highscore.json";
        txt.text = "Highscore: " + LoadHighScore();
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}

[System.Serializable]
public class HighScoreData
{
    public int highScore;
}
