using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int index;
    public int bulletId;
    public int damage;
    private void FixedUpdate()
    {
        ServerSend.BulletPosition(this);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        Player player = otherObject.GetComponent<Player>();
        if (otherObject.tag == "Player" && player.id != bulletId)
        {
            if (player.health > 0)
            {
                player.health -= damage;
                ServerSend.PlayerHit(player);
            }
            else
            {
                player.Destroy(bulletId);
            }

            ServerSend.BulletDestroy(this);
            Destroy(gameObject);
        }
        else if (otherObject.tag == "Asteroid")
        {
            ServerSend.BulletDestroy(this);
            Destroy(gameObject);
        }
    }
}
