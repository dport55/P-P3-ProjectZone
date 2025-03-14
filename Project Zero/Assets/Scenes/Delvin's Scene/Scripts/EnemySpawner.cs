using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] int timeBetweenSpawns;
    [SerializeField] Transform[] spawnPoints;

    float spawnTimer;

    int spawnCount;

    bool startedSpawning;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (startedSpawning)
        {
            if (spawnCount < numToSpawn && spawnTimer >= timeBetweenSpawns)
            {
                Spawn();
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !startedSpawning)
        {
            startedSpawning = true;

        }
    }

    void Spawn()
    {
        int arrayPos = Random.Range(0, spawnPoints.Length);

        Instantiate(objectToSpawn, spawnPoints[arrayPos].position, spawnPoints[arrayPos].rotation);
        spawnCount++;
        spawnTimer = 0;

    }
}
