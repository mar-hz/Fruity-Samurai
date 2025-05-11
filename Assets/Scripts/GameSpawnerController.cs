using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSpawnerController : MonoBehaviour
{
    public BoxCollider volumeA;
    public GameObject npcPrefab;

    public int totalCount;
    public int activeNPCs;

    public static GameSpawnerController instance;

    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 4.5f;

    private float spawnTimer;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ResetSpawnTimer();
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnNPC();
            ResetSpawnTimer();
        }
    }

    void SpawnNPC()
    {
        GameObject npc = Instantiate(npcPrefab, GetRandomPointInVolume(volumeA), Quaternion.identity);
        totalCount++;
        activeNPCs++;
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

    void ResetSpawnTimer()
    {
        float multiplier = GameObject.Find("Player").GetComponent<PlayerController>().multiplier;

        // The more the combo, the faster the spawns (but never below minSpawnInterval)
        float dynamicMax = Mathf.Max(minSpawnInterval, maxSpawnInterval - 0.2f * (multiplier - 1f));
        spawnTimer = Random.Range(minSpawnInterval, dynamicMax);
    }

    public void Replay()
    {
        SceneManager.LoadScene(0);
    }

    public void GameFinish()
    {
        // Not used anymore in infinite mode
    }
}
