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

    // private Vector3 direction; 
    private float moveSpeed = 5f / Constants.TICS_PER_SEC;
    private float rotSpeed = 50f / Constants.TICS_PER_SEC;
    private bool[] inputs;

    public GameObject projectilePrefab;
    public Vector3 spawnPosition;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        angle = 0.0f;
        inputs = new bool[5];
        health = GameSettings.PLAYER_MAX_HEALTH;
        spawnPosition = Vector3.zero;
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
        transform.position += inputDirection * moveSpeed;
        transform.Rotate(inputAngle * rotSpeed);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void SetInputs(bool[] _inputs)
    {
        inputs = _inputs;
    }

    public void Shooting(int weaponLevel, float projectileSpeed)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.GetComponent<Bullet>().bulletId = id;
        float heading = transform.rotation.eulerAngles.z + 90.0f;
        Vector3 direction = new Vector3(Mathf.Cos(heading * Mathf.Deg2Rad), Mathf.Sin(heading * Mathf.Deg2Rad));
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        ServerSend.PlayerShooting(this);
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
        ServerSend.PlayerDestroy(this, killerId);
        Debug.Log($"player {killerId} killed player {id}");
        StartCoroutine("Respawn");

    }
    public IEnumerator Respawn()
    {
        Debug.Log("Respawning...");
        yield return new WaitForSeconds(5f);
        health = GameSettings.PLAYER_MAX_HEALTH;
        Active(true);
        ServerSend.PlayerRespawn(this);
    }
}

