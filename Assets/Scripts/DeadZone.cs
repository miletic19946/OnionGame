using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Defines a zone that triggers game over when specific objects enter it
public class DeadZone : MonoBehaviour
{
    // Called when another collider enters this trigger zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is either a target or the player
        if (collision.tag == "Target" || collision.tag == "Player")
        {
            // End the game by showing the game over screen
            UI.instance.OpenEndScreen(); // this will end the game
        }
    }
}
