using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletId;
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        if (otherObject.tag == "Player" && otherObject.GetComponent<Player>().id != bulletId)
        {
            if (other.gameObject.GetComponent<Player>().health > 0)
            {
                other.gameObject.GetComponent<Player>().health--;
                ServerSend.PlayerHit(other.gameObject.GetComponent<Player>());
            }
            else
            {
                Destroy(other.gameObject);
                ServerSend.PlayerDestroy(other.gameObject.GetComponent<Player>());
            }
            Destroy(gameObject);
        }
    }
}
