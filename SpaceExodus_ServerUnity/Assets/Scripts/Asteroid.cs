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
        float randSpeed = Random.Range(40, 60);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(randAngle * Mathf.Deg2Rad), Mathf.Sin(randAngle * Mathf.Deg2Rad)) * randSpeed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ServerSend.AsteroidPosition(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        ServerSend.DestroyAsteroid(this);
        Destroy(gameObject);
        Debug.Log("fuck");
    }
}
