using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int id;
    public int type;
    private Vector3 vel;
    // Start is called before the first frame update
    void Start()
    {
        // set random velocity.  
        float randAngle = Random.Range(0, 360);
        float randSpeed = Random.Range(60, 100);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(randAngle * Mathf.Deg2Rad), Mathf.Sin(randAngle * Mathf.Deg2Rad)) * randSpeed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        bool onScreen = screenPoint.x > 0 && screenPoint.x < Screen.width && screenPoint.y > 0 && screenPoint.y < Screen.height;

        if (onScreen == false)
        {
            Vector3 newScreenPoint = new Vector3(Screen.width - screenPoint.x, Screen.height - screenPoint.y, 10.0f);
            Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(newScreenPoint);
            transform.position = screenToWorld;
        }

        ServerSend.AsteroidPosition(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        if (otherObject.tag == "Player")
        {
            otherObject.GetComponent<Player>().Destroy(0);
        }
        if (otherObject.tag != "Asteroid" && otherObject.tag != "PowerUps")
        {
            ServerSend.DestroyAsteroid(this);
            Destroy(gameObject);
        }
    }
}
