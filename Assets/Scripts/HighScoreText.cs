using UnityEngine;
using TMPro;
using System;
using System.IO;

public class HighScoreText : MonoBehaviour
{
    public GameObject pina;
    public TMP_Text txt;
    private static string path = Application.persistentDataPath + "/highscore.json";

    public static void SaveHighScore(int score)
    {
        HighScoreData data = new HighScoreData { highScore = score };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public static int LoadHighScore()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
            return data.highScore;
        }
        return 0;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
