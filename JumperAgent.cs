using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using System.Collections.Generic;

public class JumperAgent : Agent
{
    public GameObject obstaclePrefab;
    private Rigidbody rb;
    private Vector3 startPos;
    private float spawnTimer;
    private float spawnInterval = 2f; // elke 2 seconden nieuw obstakel
    private float obstacleSpeed;

    private bool isJumping = false;
    
    private List<GameObject> obstacles = new List<GameObject>();

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = startPos;
        rb.velocity = Vector3.zero;
        isJumping = false;

        foreach (GameObject obs in obstacles)
        {
            Destroy(obs);
        }
        obstacles.Clear();
        spawnTimer = 0f;
        obstacleSpeed = Random.Range(2f, 5f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position.y);

        if (obstacles.Count > 0)
        {
            sensor.AddObservation(obstacles[0].transform.position.z);  // Changed x to z
        }
        else
        {
            sensor.AddObservation(10f); // dummy waarde als geen obstakels
        }

        sensor.AddObservation(obstacleSpeed);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];

        if (action == 1 && !isJumping)
        {
            Jump();
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnInterval)
        {
            SpawnObstacle();
            spawnTimer = 0f;
        }

        // Beweeg alle obstakels op de Z-as
        for (int i = obstacles.Count - 1; i >= 0; i--)
        {
            GameObject obs = obstacles[i];
            obs.transform.position += Vector3.forward * obstacleSpeed * Time.deltaTime;  // Moves along Z-axis

            if (obs.transform.position.z > 5f)  // Destroy obstacles when they reach z = 5
            {
                Destroy(obs);
                obstacles.RemoveAt(i);
                // Reward for successfully avoiding obstacle (or just letting it pass)
                SetReward(0.1f);  // Slight positive reward for avoiding an obstacle
            }

            // Check if the agent jumps over the obstacle
            if (obs.transform.position.z < transform.position.z + 0.5f && 
                obs.transform.position.z > transform.position.z - 0.5f)
            {
                if (transform.position.y > 1f)  // The agent has jumped over the obstacle
                {
                    SetReward(0.5f);  // Extra reward for jumping over the obstacle
                }
            }
        }

        // Check for collisions with obstacles
        foreach (GameObject obs in obstacles)
        {
            if (obs.transform.position.z < transform.position.z + 0.5f && 
                obs.transform.position.z > transform.position.z - 0.5f)
            {
                if (transform.position.y < 0.5f)  // The agent hits the obstacle while on the ground
                {
                    SetReward(-1f);  // Penalty for hitting an obstacle
                    EndEpisode();  // End the episode if the agent hits an obstacle
                    return;
                }
            }
        }

        // Check if the agent is above a certain height, which might be considered an error state
        if (transform.position.y > 2f)
        {
            SetReward(-0.5f);  // Penalty for jumping too high
            EndEpisode();  // End the episode if the agent jumps too high
        }
    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void Jump()
    {
        isJumping = true;
        rb.AddForce(Vector3.up * 5f, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    // private void OnCollisionStay(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         isJumping = false;  // The agent is considered not jumping as long as it stays on the ground
    //     }
    // }

    private void SpawnObstacle()
    {
        // Spawn obstacles at random positions along the Z-axis between -5 and 5
        float randomZ = Random.Range(-5, -3f);  // Corrected Z-range for spawning
        GameObject newObstacle = Instantiate(obstaclePrefab, new Vector3(0f, 0f, randomZ), Quaternion.identity);
        obstacles.Add(newObstacle);
    }
}
