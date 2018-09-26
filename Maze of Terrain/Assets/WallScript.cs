using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour {

    public int maxPoint = 3;

    private int currentPoint;

    private void Start()
    {
        currentPoint = maxPoint;
    }

    private void Update()
    {
        if (currentPoint <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            currentPoint--;
        }
    }
}
