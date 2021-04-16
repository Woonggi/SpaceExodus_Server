using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        Player player = otherObject.GetComponent<Player>();
        if (otherObject.tag == "Player")
        {
            if (player.weaponLevel < 3)
            {
                player.weaponLevel++;
                ServerSend.PowerUp(player);
            }
            Destroy(gameObject);
        }
    }
}
