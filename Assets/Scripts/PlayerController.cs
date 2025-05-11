using System;
using UnityEditor;
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
    bool canAttack = true;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();
        playerAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Space))
        {
            verticalInput = 1;
        }

        elapsedTime += Time.deltaTime;
        playerAnimator.SetFloat("time", elapsedTime);
        if (elapsedTime > idleCoolDown) elapsedTime = 0f;
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("attack") || stateInfo.IsName("attackUp"))
        {
            if (stateInfo.normalizedTime >= 1f)
            {
                // Animation finished
                canAttack = true;
            }
        } else
        {
            canAttack = true;
        }

        if (canAttack)
        {
            if (Input.GetKey(KeyCode.E))
            {
                AttackUp();
                canAttack = false;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                Attack();
                canAttack = false;
            }
        }
        sprinting = Input.GetKey(KeyCode.LeftShift);
    }

    float horizontalInput;
    float verticalInput;
    bool sprinting;

    void FixedUpdate()
    {
       
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

        Vector3 movement = sprinting
            ? new Vector3(horizontalInput * sprintSpeed, playerRigidbody.linearVelocity.y, 0) 
            : new Vector3(horizontalInput * speed, playerRigidbody.linearVelocity.y, 0);

        playerRigidbody.linearVelocity = movement;

        playerAnimator.SetFloat("speed", Math.Abs(movement.x));


        
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
        if (collision.gameObject.CompareTag("Floor"))
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
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
            playerAnimator.SetBool("isGrounded", false);
        }
    }

}
