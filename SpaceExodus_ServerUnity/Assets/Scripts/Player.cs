using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;

    public float angle;
    public float heading;
    public int health;
    public int weaponLevel = 1;
    public bool dead = false;

    // private Vector3 direction; 
    private float moveSpeed = 5f / Constants.TICS_PER_SEC;
    private float rotSpeed = 50f / Constants.TICS_PER_SEC;
    private bool[] inputs;
    private bool onScreen;
    private int bulletIndex;

    public GameObject[] projectilePrefab;
    public GameObject powerUpPrefab;
    public Vector3 spawnPosition;

    public void Initialize(int _id, string _username)
    {
        bulletIndex = 0;
        id = _id;
        username = _username;
        angle = 0.0f;
        inputs = new bool[5];
        health = GameSettings.PLAYER_MAX_HEALTH;
        spawnPosition = new Vector3(5.0f, 5.0f, 0.0f);
        transform.position = spawnPosition;
    }
    public void FixedUpdate()
    {
        float heading = transform.rotation.eulerAngles.z + 90.0f;
        Vector3 inputDirection = Vector3.zero;
        Vector3 inputAngle = Vector3.zero; 
        // W
        if (inputs[0] == true)
        {
            inputDirection = new Vector3(Mathf.Cos(heading * Mathf.Deg2Rad), Mathf.Sin(heading * Mathf.Deg2Rad), 0.0f);
        }
        // S
        if (inputs[1] == true)
        {
        }
        // A}
        if (inputs[2] == true)
        {
            inputAngle.z += 3.0f;
        }
        // D
        if (inputs[3] == true)
        {
            inputAngle.z -= 3.0f;
        }

        Move(inputDirection, inputAngle);
    }

    private void Move(Vector3 inputDirection, Vector3 inputAngle)
    {
        if (dead == false)
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            onScreen = screenPoint.x > 0 && screenPoint.x < Screen.width && screenPoint.y > 0 && screenPoint.y < Screen.height;

            if (onScreen == false)
            {
                Vector3 newScreenPoint = new Vector3(Screen.width - screenPoint.x, Screen.height - screenPoint.y, 10.0f);
                Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(newScreenPoint);
                transform.position = screenToWorld; 
            }
            transform.position += inputDirection * moveSpeed;
            transform.Rotate(inputAngle * rotSpeed);
            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
        }
    }

    public void SetInputs(bool[] _inputs)
    {
        inputs = _inputs;
    }

    public void Shooting(int weaponLevel, float projectileSpeed)
    {
        if (dead == false)
        {
            GameObject projectile = Instantiate(projectilePrefab[weaponLevel - 1], transform.position, transform.rotation);
            projectile.GetComponent<Bullet>().bulletId = id;
            projectile.GetComponent<Bullet>().index = bulletIndex++;
            float heading = transform.rotation.eulerAngles.z + 90.0f;
            Vector3 direction = new Vector3(Mathf.Cos(heading * Mathf.Deg2Rad), Mathf.Sin(heading * Mathf.Deg2Rad));
            projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
            ServerSend.PlayerShooting(this, projectile.GetComponent<Bullet>());
        }
    }
   
    private void Active(bool active)
    {
        gameObject.GetComponent<Renderer>().enabled = active;
        gameObject.GetComponent<Collider2D>().enabled = active;
        gameObject.GetComponent<Player>().enabled = active;
    }

    public void Destroy(int killerId)
    {
        Active(false);
        dead = true;
        SpawnPowerUps();
        weaponLevel = 1;
        ServerSend.SpawnPowerUp(this);
        ServerSend.PlayerDestroy(this, killerId);
        StartCoroutine("Respawn");
    }
 
    public IEnumerator Respawn()
    {
        Debug.Log("Respawning...");
        yield return new WaitForSeconds(3f);
        health = GameSettings.PLAYER_MAX_HEALTH;
        Active(true);
        dead = false;
        Vector3 spawnPositionScreen = new Vector3(Random.Range(10, Screen.width), Random.Range(10, Screen.height), 10.0f);
        Vector3 respawnPosition = Camera.main.ScreenToWorldPoint(spawnPositionScreen);
        transform.position = respawnPosition;
        ServerSend.PlayerRespawn(this, respawnPosition);
    }

    private void SpawnPowerUps()
    {
        Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
    }
}

