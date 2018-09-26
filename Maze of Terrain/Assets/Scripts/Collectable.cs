using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public float rotateSpeed = 30;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float angle = rotateSpeed * Time.deltaTime;
        transform.Rotate(angle, angle, angle);
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            FirstPersonMovement.instance.ammo++;
            this.gameObject.SetActive(false);
            
        }
    }
}
