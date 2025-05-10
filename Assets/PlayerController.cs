using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float sprintSpeed = 20f;
    public float speedLimit = 30f;
    public float jumpForce = 5f;
    private Transform playerTransform;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    bool isGrounded = true;
    float idleCoolDown = 4f;
    float elapsedTime = 0f;
    float previousYVelocity = 0f;
    private float yAcceleration = 0f;
   
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();
        playerAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        playerAnimator.SetFloat("time", elapsedTime);
        if (elapsedTime > idleCoolDown) elapsedTime = 0f;
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        // Calculate Y acceleration
        float currentYVelocity = playerRigidbody.linearVelocity.y;
        yAcceleration = (currentYVelocity - previousYVelocity) / Time.fixedDeltaTime;
        previousYVelocity = currentYVelocity;

        playerAnimator.SetFloat("yAccel", yAcceleration);

        playerAnimator.SetFloat("direction", horizontalInput);
        if (horizontalInput > 0)
        {
            playerTransform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (horizontalInput < 0)
        {
            playerTransform.rotation = Quaternion.Euler(0, -90, 0);
        }

        if (isGrounded && verticalInput > 0)
        {
            //isGrounded = false;
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        Vector3 movement = Input.GetKey(KeyCode.LeftShift) 
            ? new Vector3(horizontalInput * sprintSpeed, 0, 0) 
            : new Vector3(horizontalInput * speed, 0, 0);

        playerRigidbody.AddForce(movement, ForceMode.Force);
        if(playerRigidbody.linearVelocity.x > speedLimit) {
            playerRigidbody.linearVelocity = new Vector3(speedLimit, playerRigidbody.linearVelocity.y, playerRigidbody.linearVelocity.z);
        }

        playerAnimator.SetFloat("speed", Math.Abs(movement.x));

        if (Input.GetKeyDown(KeyCode.E))
        {
            AttackUp();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Attack();
        }
    }

    void AttackUp()
    {
        playerAnimator.SetTrigger("attkUp");
    }

    void Attack()
    {
        playerAnimator.SetTrigger("attk");
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is colliding with the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Vector3 velocity = playerRigidbody.linearVelocity;
            velocity.y = 0; // Reset Y velocity on ground contact
            playerRigidbody.linearVelocity = velocity;
            yAcceleration = 0; // Reset Y acceleration on ground contact
            playerAnimator.SetBool("isGrounded", true);
            playerAnimator.SetFloat("yAccel", yAcceleration); // Reset Y acceleration in animator
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Check if the player is leaving the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            playerAnimator.SetBool("isGrounded", false);
        }
    }

}
