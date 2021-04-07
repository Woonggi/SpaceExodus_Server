using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletId;
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        Player player = otherObject.GetComponent<Player>();
        if (otherObject.tag == "Player" && player.id != bulletId)
        {
            if (player.health > 0)
            {
                player.health--;
                ServerSend.PlayerHit(other.gameObject.GetComponent<Player>());
            }
            else
            {
                player.Destroy(bulletId);
            }
            Destroy(gameObject);
        }
    }
}
