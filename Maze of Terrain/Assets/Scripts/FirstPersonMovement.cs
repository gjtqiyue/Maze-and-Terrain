using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour {

    public static FirstPersonMovement instance = null;

    public float moveSpeed = 3;
    public Camera viewPoint;
    public float smoothing = 2.0f;
    public float jumpForce = 130;
    public PlayerShoot launcher;
    public int ammo = 3;
    [HideInInspector]
    public bool canMove;

    private Rigidbody rb;
    private float horizontal;
    private float vertical;
    private bool canJump;
    private bool onGround;
    private float fireRate = 0.5f;
    private float lastTimeFire;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        rb = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
        canMove = true;
	}

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        isGrounded();

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            canJump = true;
        }
        

        if (Input.GetKeyDown(KeyCode.F) && ammo > 0 && Time.time > (fireRate + lastTimeFire))
        {
            launcher.LaunchProjectile();
            lastTimeFire = Time.time;
            ammo--;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        
        if (canMove)
        {
            Move();
        }

        Turn();
	}

    private void Move()
    {
        transform.position += transform.forward * vertical * moveSpeed * Time.deltaTime + transform.right * horizontal * moveSpeed * Time.deltaTime;

    }

    private void Turn()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        viewPoint.transform.Rotate(-v * smoothing * Time.deltaTime, 0, 0);
        transform.Rotate(0, h * smoothing * Time.deltaTime, 0);

    }

    private void Jump()
    {
        if (canJump && isGrounded())
        {
            canJump = false;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            
        }
    }

    private bool isGrounded()
    {
        if (Mathf.Abs(rb.velocity.y) >= 0.003)
        {
            
            canMove = false;
            return false;
        }
        else
        {
            canMove = true;
            return true;
        }
    }
}
