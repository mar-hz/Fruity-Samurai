using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSpawnerController : MonoBehaviour
{
    public BoxCollider volumeA;
    public GameObject npcPrefab;
    public GameObject apple;

    public int totalCount;
    public int activeNPCs;

    public static GameSpawnerController instance;
    
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 4.5f;

    private float spawnTimer;
    public int score;
    public float appleChance = 0.05f;
    public bool spawnEnabled = true;
    public AudioSource sfxSource;
    public AudioClip gameOver;

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
        HighScoreText.LoadHighScore();
    }

    void Update()
    {
        if (!spawnEnabled) return;

        if (volumeA == null)
            volumeA = GameObject.Find("Spawner").GetComponent<BoxCollider>();

        if (sfxSource == null)
            sfxSource = GameObject.Find("SFX_Samp").GetComponent<AudioSource>();

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnNPC();
            ResetSpawnTimer();
        }
    }

    void SpawnNPC()
    {
        GameObject prefabToSpawn = Random.value < appleChance ? apple : npcPrefab;
        GameObject npc = Instantiate(prefabToSpawn, GetRandomPointInVolume(volumeA), Quaternion.identity);
        npc.GetComponent<NPCExploder>().sfxSource = sfxSource;
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
        if (GameObject.Find("Player") == null) return;

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
        if (score >= HighScoreText.highScore)
        {
            HighScoreText.SaveHighScore(score);
        }
        StartCoroutine(PlaySoundThenLoad(2));
    }

    IEnumerator PlaySoundThenLoad(int sceneToLoad)
    {
        sfxSource.PlayOneShot(gameOver);
        yield return new WaitForSeconds(gameOver.length);
        SceneManager.LoadScene(sceneToLoad);
    }
}
