﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FlockSpawn : MonoBehaviour
{

    // Bird object
    public GameObject birdPrefab;

    // Size of bounding box that dictates both boid spawn position, and movement bounds
    public static int flockBounds = 50;
    // Amount of boids that are spawned
    private static int flockSize = 300;

    // Array that stores all birds in the scene
    public static GameObject[] birdsArray = new GameObject[flockSize];

    // This node contains all spawn nodes as children
    public GameObject goalPositionParentNode;

    // Array of all possible goals in the scene
    private GameObject[] goalPositionsArray;

    // Array of index of the current goal
    private int currentGoalIndex;

    // Position of the current goal
    public static Vector3 currentGoalPosition;
    // Starting spawn position of each bird
    private Vector3 birdSpawnPosition;

    // Target object (If this moves, move the flock)
    private Vector3 targetPositon;

    private float timer = 0.0f;
    private float newGoalTimer = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Gets size of array from child count
        int arraySize = goalPositionParentNode.transform.childCount;
        // Create array with above size
        goalPositionsArray = new GameObject[arraySize];

        // Get all child nodes from parent node - Store in array
        for (int i = 0; i < arraySize; i++)
        {
            goalPositionsArray[i] = goalPositionParentNode.transform.GetChild(i).gameObject;
        }

        // Select initial goal
        SelectGoal();

        // Get players starting position
        targetPositon = this.transform.position;

        // Spawn initial bird oids
        SpawnBirds();
    }

    private void Update()
    {
        // Update player position to keep gizmos updating
        targetPositon = this.transform.position;

        // Select new goal after newGoalTimer has elapsed
        if (timer > newGoalTimer)
        {
            SelectGoal();
            // Reset timer to 0
            timer = 0.0f;
        }
        // Increment timer every frame
        timer += Time.deltaTime;
    }

    // Set new goal position when called
    void SelectGoal()
    {
        // Choose random starting index
        currentGoalIndex = Random.Range(0, goalPositionsArray.Length);

        // Get current goal position;
        currentGoalPosition = goalPositionsArray[currentGoalIndex].transform.position;
    }

    // Spawns in birds within flock bounds set above, with a random position and location
    void SpawnBirds()
    {
        for (int i = 0; i < flockSize; i++)
        {
            // Choose random position within bounds
            birdSpawnPosition = new Vector3(Random.Range(targetPositon.x - flockBounds, targetPositon.x + flockBounds),
                                            Random.Range(targetPositon.y + 10f, targetPositon.y + flockBounds),
                                            Random.Range(targetPositon.z - flockBounds, targetPositon.z + flockBounds));
            // Create birds at above created position with random rotation
            birdsArray[i] = Instantiate(birdPrefab, birdSpawnPosition, Quaternion.LookRotation(new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f))));
        }
    }

    void OnDrawGizmos()
    {
        // Display boid bounds in the scene
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new Vector3(targetPositon.x, targetPositon.y + 32.50f, targetPositon.z), new Vector3(flockBounds * 2, 45, flockBounds * 2));
    }
}
