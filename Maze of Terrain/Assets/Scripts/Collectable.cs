using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Collectable script, instantiated in the maze generator
 */
public class Collectable : MonoBehaviour {

    public float rotateSpeed = 30;     
	
	// Update is called once per frame
	void Update () {
        float angle = rotateSpeed * Time.deltaTime;
        transform.Rotate(angle, angle, angle);
	}

    // if the player touches it, the item get destroyed and player gain one ammo
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            FirstPersonPlayer.instance.ammo++;
            this.gameObject.SetActive(false);
            
        }
    }
}
