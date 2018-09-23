using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    public Projectile bullet;
    public float launchForce = 15;

	// Use this for initialization
	void Start () {
		
	}
	
	public void LaunchProjectile()
    {
        Projectile instance = Instantiate(bullet, transform.position, Quaternion.identity);
        instance.GetComponent<Rigidbody>().AddForce(transform.forward * launchForce, ForceMode.Impulse);
    }
}
