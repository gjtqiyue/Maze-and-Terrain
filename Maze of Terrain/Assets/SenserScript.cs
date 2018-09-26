using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenserScript : MonoBehaviour {

    private Vector3 origin;

    private float lastTriggeredTime;

	// Use this for initialization
	void Start () {
        gameObject.SetActive(true);
        origin = new Vector3(-17.5f, 5f, 3f);
        transform.position = origin;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && Time.time > 0.5 + lastTriggeredTime)
        {
            // call the method to generate one more row
            MazeGenerator.instance.RandomGenerate();

            // move the collider to the next position
            transform.position += new Vector3(0f, 0f, 5);

            // update time
            lastTriggeredTime = Time.time;
        }
        else if (other.gameObject.tag == "Projectile")
        {
            bool ended = MazeGenerator.instance.EndMaze();

            if (ended)
                gameObject.SetActive(false);
        }
        
    }
}
