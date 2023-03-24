using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab; // Reference to the platform prefab
    public float platformDestroyY = -10f; // Y position at which platforms are destroyed
    public float platformVerticalSpacing = 1f; // Vertical spacing between the platforms
    public Text scoreText;

    private List<GameObject> platforms = new List<GameObject>(); // List of spawned platforms
    private float platformMoveSpeed = 2f; // Speed at which the platforms move downwards
    private int platformNumber = 3;
    private int score = 0;
    private int bestScore = 0; // Best score variable

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        // Spawn the initial platforms
        for (int i = 0; i < platformNumber; ++i)
        {
            // Instantiate a new platform game object from the platform prefab
            GameObject platformObject = Instantiate(platformPrefab, transform);

            // Set the position of the platform
            float yPos = platformVerticalSpacing * (i+1);
            platformObject.transform.position = new Vector3(0f, yPos, 0f);
            platforms.Add(platformObject);
        }
        scoreText.text = "Score: " + score + "\nBest: " + bestScore;
    }

    private void Update()
    {
        // Move all platforms downwards
        foreach (GameObject platform in platforms)
        {
            platform.transform.Translate(Vector3.down * platformMoveSpeed * Time.deltaTime);
        }

        // Check if the bottom platform should be destroyed
        if (platforms.Count > 0 && platforms[0].transform.position.y < platformDestroyY)
        {
            // Destroy the bottom platform
            Destroy(platforms[0]);
            platforms.RemoveAt(0);
            if(platformMoveSpeed < 5)
                platformMoveSpeed *= 1.05f;
            score++;
            if(score > bestScore)
            {
                bestScore = score;
                PlayerPrefs.SetInt("BestScore", bestScore);
                PlayerPrefs.Save();
            }
            scoreText.text = "Score: " + score + "\nBest: " + bestScore;
        }

        // Spawn a new platform if there are less than 3 platforms
        if (platforms.Count < platformNumber)
        {
            SpawnPlatform();
        }
    }

    private void SpawnPlatform()
    {
        // Instantiate a new platform game object from the platform prefab
        GameObject platformObject = Instantiate(platformPrefab, transform);

        // Set the position of the platform
        float yPos = (platforms.Count == 0) ? 0f : platforms[platforms.Count - 1].transform.position.y + platformObject.GetComponent<PlatformGenerator>().platformHeight + platformVerticalSpacing;
        platformObject.transform.position = new Vector3(0f, yPos, 0f);

        // Add the platform to the list of spawned platforms
        platforms.Add(platformObject);
    }
}
