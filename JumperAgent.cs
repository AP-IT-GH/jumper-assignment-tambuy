using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using System.Collections.Generic;

public class JumperAgent : Agent
{
    private Rigidbody rb;
    private Vector3 startPos;
    private bool isJumping = false;

    public Manager manager;
    private List<GameObject> obstacles;
    public ObstacleMovement obstacle;

    private void Update()
    {
        if (obstacles != null && obstacles.Count > 0)
        {
            GameObject closestObstacle = obstacles[0];
            if (closestObstacle.transform.position.z < transform.position.z)
            {
                SetReward(0.1f); // Small reward for passing
                obstacles.RemoveAt(0); // Remove the obstacle so it doesn't give rewards multiple times
            }
        }
    }


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

        obstacles = manager.obstacles;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position.y);

        if (obstacles != null && obstacles.Count > 0)
            sensor.AddObservation(obstacles[0].transform.position.z);
        else
            sensor.AddObservation(10f);

        if (obstacle != null)
            sensor.AddObservation(obstacle.obstacleSpeed);
        else
            sensor.AddObservation(0f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];

        if (action == 1 && !isJumping)
        {
            Jump();
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
        rb.AddForce(Vector3.up * 7f, ForceMode.VelocityChange); // Stronger jump
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            SetReward(-1f); // Negative reward for touching obstacle
            EndEpisode();
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Obstacle");
            foreach (GameObject obj in objects)
                {
                    Destroy(obj);
                }
        }
    }
}
