using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayer : MonoBehaviour {

    public static FirstPersonPlayer instance = null;

    public Vector3 startPoint;
    public float moveSpeed = 3;
    public Camera viewPoint;
    public float smoothing = 2.0f;
    public float jumpForce = 130;
    public PlayerShoot launcher;
    public int ammo = 3;

    private bool canMove = false;
    public bool canTurn = true;
    private Rigidbody rb;
    private float horizontal;
    private float vertical;
    private bool canJump = false;
    private float fireRate = 0.5f;
    private float lastTimeFire;
    private bool isActive;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        rb = GetComponent<Rigidbody>();
    }

    internal void lockMovement()
    {
        isActive = false;
    }

    public void Initialize()
    {
        isActive = true;
        canJump = true;
        rb.useGravity = true;
        transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 0));
        transform.position = startPoint;
        ammo = 0;
    }

    private void Update()
    {
        if (isActive)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            isGrounded();

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
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (isActive)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            if (canMove)
            {
                Move();
            }

            if (canTurn)
            {
                Turn();
            }
        }
	}

    private void Move()
    {
        transform.position += transform.forward * vertical * moveSpeed * Time.deltaTime + transform.right * horizontal * moveSpeed * Time.deltaTime;

    }

    private void Turn()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        //if (Mathf.Abs(Quaternion.Angle(Quaternion.LookRotation(viewPoint.transform.forward), Quaternion.LookRotation(transform.forward))) < 90f)
        float angle = -v * smoothing * Time.deltaTime;
        float preditAngle = viewPoint.transform.rotation.eulerAngles.x + angle;
        if (preditAngle > 180) preditAngle -= 360;
        Debug.Log(preditAngle);
        if (Mathf.Abs(preditAngle) < 90)
            viewPoint.transform.rotation = Quaternion.Lerp(viewPoint.transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(preditAngle, 0f, 0f)), Mathf.Abs(v) * smoothing * Time.fixedDeltaTime);

        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, h, 0f)), Mathf.Abs(h) * smoothing * Time.fixedDeltaTime);
        transform.Rotate(0, h * smoothing * Time.fixedDeltaTime, 0);

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
