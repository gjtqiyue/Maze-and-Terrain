using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    public Projectile bullet;       // bullet reference
    public float launchForce = 15;  // projectile launch speed
	
	public void LaunchProjectile()
    {
        // instantiate a bullet projectile in the scene and shoot it forward based on the launchForce
        Projectile instance = Instantiate(bullet, transform.position, Quaternion.identity);
        instance.GetComponent<Rigidbody>().AddForce(transform.forward * launchForce, ForceMode.Impulse);
    }
}
