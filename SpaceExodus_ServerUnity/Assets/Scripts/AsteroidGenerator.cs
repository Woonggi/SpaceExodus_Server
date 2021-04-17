using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    public float timer;
    public int maxAsteroids;
    public GameObject[] asteroidPrefabs;
    private int ids;
    private float tick;
    private void Start()
    {
        ids = 0; 
    }

    private void Update()
    {
        if (tick <= 0.0f && Server.started == true)
        {
            SpawnAsteroid();
            tick = timer;
        }
        tick -= Time.deltaTime;
    }

    public void SpawnAsteroid()
    {
        Vector3 spawnPosition = new Vector3(5.0f, 5.0f, 0.0f);
        int type = Random.Range(0, 4); 
        Asteroid asteroid = Instantiate(asteroidPrefabs[type], spawnPosition, Quaternion.identity).GetComponent<Asteroid>();
        asteroid.id = ids++; asteroid.type = type;
        ServerSend.SpawnAsteroid(asteroid);
    }
}
