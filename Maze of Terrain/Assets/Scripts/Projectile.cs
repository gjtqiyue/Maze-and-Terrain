using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private void Start()
    {
        // automatically self-destruct after 5s
        Destroy(this.gameObject, 5);
    }

    // if it hits the wall, disappear right away
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }
}
