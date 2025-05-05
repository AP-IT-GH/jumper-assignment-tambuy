using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public List<GameObject> obstacles = new List<GameObject>();

    private float spawnTimer = 0f;
    public float spawnInterval = 2f; // Time to wait after obstacle is destroyed

    void Update()
    {
        // Remove destroyed obstacles from the list
        obstacles.RemoveAll(obstacle => obstacle == null);

        // If no obstacles exist, start the timer
        if (obstacles.Count == 0)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                StartCoroutine(SpawnObstacle());
                spawnTimer = 0f;
                
            }
        }
        else
        {
            // Reset the timer while obstacle exists
            spawnTimer = 0f;
        }
    }


    private string SpawnObstacle()
    {
        float randomZ = Random.Range(-5f, -3f);
        GameObject newObstacle = Instantiate(obstaclePrefab, new Vector3(0f, 0f, randomZ), Quaternion.identity);
        obstacles.Add(newObstacle);

        return "ja";
    }
}
