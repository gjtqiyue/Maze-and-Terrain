using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The senser that detects the player and create the new row as the player moves onward
 */
public class SenserScript : MonoBehaviour {

    public static SenserScript instance = null;

    private Vector3 origin;                         // its start position

    private float lastTriggeredTime;                // the most recent time it got triggered 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start () {
        gameObject.SetActive(true);         
        origin = new Vector3(-17.5f, 5f, 3f);       // set the origin near the entrance
        InitializePosition();
	}
	
    // set the current position to the origin
	public void InitializePosition()
    {
        transform.position = origin;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && Time.time > 0.5 + lastTriggeredTime)    // create new row if player moves forward to the cliff
        {
            // call the method to generate one more row
            MazeGenerator.instance.RandomGenerate();

            // move the collider to the next position
            transform.position += new Vector3(0f, 0f, 5);

            // update time
            lastTriggeredTime = Time.time;
        }
        else if (other.gameObject.tag == "Projectile")          // if the player shoots a projectile towards it, end the maze
        {
            MazeGenerator.instance.EndMaze();

            // move the collider to the next position
            transform.position += new Vector3(0f, 0f, 5);
        }
        
    }
}
