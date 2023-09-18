using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrKillZone : MonoBehaviour
{
 //Trigger que mata la personaje cuando toca la zona de muerte
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
            //Transforma la colisión a player
            scrPlayer player  = other.GetComponent<scrPlayer>();
            player.Die();
        }
    }
}