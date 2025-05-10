using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSpawnerController : MonoBehaviour
{
    public int totalCount;

    public BoxCollider volumeA;
    public GameObject npcPrefab;
   
    public int activeNPCs;

    public static GameSpawnerController instance;
    public float startTime = 10f;        // Starting time in seconds
    public float currentTime;           // The current time countdown
    public float spawnInterval = 2f;     // Initial spawn interval (time between NPC spawns)
    public float spawnTimer;            // Timer to track time between spawns


    bool shouldUpdate;

    void Awake()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<GameSpawnerController>();
        }
        else
        {
            Destroy(gameObject);
        }

        instance.GameStart();
       
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Moved to the closeInstructionsPanel method
        /* 
        currentTime = startTime;         // Set the timer to start
        spawnTimer = spawnInterval;
        SpawnNPC();
        activeNPCs = 1;
        */
        currentTime = startTime;         // Set the timer to start
        spawnTimer = spawnInterval;

    }

    Vector3 GetRandomPointInVolume(BoxCollider volume)
    {
        Vector3 extents = volume.size / 2f;
        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            0
        );

        return volume.transform.TransformPoint(point);

    }

    bool finished = false;
    // Update is called once per frame
    void Update()
    {

        if (currentTime <= 0)
        {
            shouldUpdate = false;
            if (activeNPCs == 0 && !finished)
            {
                GameFinish();
                finished = true;
            }
        }

        if (!shouldUpdate) return;

        currentTime -= Time.deltaTime;   // Decrease the time
        spawnTimer -= Time.deltaTime;    // Decrease spawn timer

        if (spawnTimer <= 0)
        {
            SpawnNPC();
            spawnTimer = Mathf.Max(spawnInterval - (startTime - currentTime) * 0.2f, 0.8f); // Decrease spawn interval as time runs out
        }

        currentTime = Mathf.Clamp(currentTime, 0f, startTime);

    }

    void SpawnNPC()
    {
        BoxCollider spawnBox = volumeA;

        GameObject instance = Instantiate(npcPrefab, GetRandomPointInVolume(spawnBox), Quaternion.identity);


        totalCount++;
        activeNPCs++;
    }

    public void Replay()
    {
        SceneManager.LoadScene(0);
    }

    public void GameStart()
    {
        shouldUpdate = true;
    }
    public void GameFinish()
    {

    }

    float CalculateScore(int actualCount, int playerGuess, int maxValue, float k = 1f)
    {
        // Calculate the squared difference between the correct count and the player's guess
        int difference = Mathf.Abs(actualCount - playerGuess);
        float penalty = k * Mathf.Pow(difference, 2);  // Exponential penalty for larger errors

        // Calculate the score with the penalty applied
        float score = Mathf.Max(0, maxValue - penalty);

        return score;
    }

}
