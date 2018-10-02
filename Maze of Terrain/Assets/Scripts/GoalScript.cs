using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Goal of the game, trigger win text display
 */
public class GoalScript : MonoBehaviour {

    // if the player triggers it, tell the game manager the game is finished
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.Win();
        }
    }
}
