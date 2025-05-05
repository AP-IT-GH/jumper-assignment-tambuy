using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float obstacleSpeed = 0.5f;
    public GameObject obstaclePrefab;
    void Start()
    {
        
    }

    void Update()
    {
         transform.position += Vector3.forward * obstacleSpeed * Time.deltaTime;

        if (transform.position.z > 5f) 
        {
            Destroy(obstaclePrefab);
        }
    }
}
