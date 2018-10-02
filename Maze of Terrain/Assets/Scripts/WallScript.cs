using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour {

    [SerializeField]
    private int maxPoint = 3;   // max health

    private int currentPoint;   // current health

    private void Start()
    {
        currentPoint = maxPoint;    // initialize the health
    }

    private void Update()
    {
        if (currentPoint <= 0)
        {
            Destroy(gameObject);    // destroy the wall if its health is 0
        }
    }

    // if a projectile hits it, it loses one health point
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            currentPoint--;
        }
    }

    public void SetMaxHealth(int point)
    {
        maxPoint = point;
    }
}
