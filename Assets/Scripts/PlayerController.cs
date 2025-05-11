using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float sprintSpeed = 20f;
    public float speedLimit = 30f;
    public float jumpForce = 5f;
    public float fallMultiplier = 1.5f;
    private Transform playerTransform;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    bool isGrounded = true;
    float idleCoolDown = 4f;
    float elapsedTime = 0f;
    float previousYVelocity = 0f;
    private float yAcceleration = 0f;
    bool canAttack = true;
    bool doubleJump = true;
    public bool attacking = false;
    public float knockbackTimer;
    public Vector3 externalVelocity;
    public GameObject pineappleMeshPart;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();
        playerAnimator = GetComponentInChildren<Animator>();
        playerTransform.rotation = Quaternion.Euler(0, -90, 0);
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

        if (stateInfo.IsName("doubleJump"))
        {
            if (stateInfo.normalizedTime >= 1f)
            {
                // Animation finished
                canAttack = true;
                playerAnimator.SetBool("attacking", false);
            } else
            {
                canAttack = false;
            }

        } else
        {
            canAttack = true;
        }

        if (stateInfo.IsName("attack") || stateInfo.IsName("attackUp"))
        {
            if (stateInfo.normalizedTime >= 1f)
            {
                // Animation finished
                attacking = false;
                playerAnimator.SetBool("attacking", false);
            }
            else
            {
                attacking = true;
            }

        }
        else
        {
            attacking = false;
        }

        if (canAttack)
        {
            if (Input.GetKey(KeyCode.E))
            {
                AttackUp();
                //canAttack = false;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                Attack();
                //canAttack = false;
            }
        }
        sprinting = Input.GetKey(KeyCode.LeftShift);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }
        else if (!isGrounded && doubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            doubleJumpRequested = true;
        }

        if (!stateInfo.IsName("doubleJump"))
        {
            if (horizontalInput > 0)
            {
                playerTransform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (horizontalInput < 0)
            {
                playerTransform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
    }

    float horizontalInput;
    float verticalInput;
    bool sprinting;
    bool jumpRequested, doubleJumpRequested;

    void FixedUpdate()
    {
       
        // Calculate Y acceleration
        float currentYVelocity = playerRigidbody.linearVelocity.y;
        yAcceleration = (currentYVelocity - previousYVelocity) / Time.fixedDeltaTime;
        previousYVelocity = currentYVelocity;

        playerAnimator.SetFloat("yAccel", yAcceleration);
        playerAnimator.SetFloat("yVel", playerRigidbody.linearVelocity.y);

        playerAnimator.SetFloat("direction", horizontalInput);

        Jump();
        
        // Custom gravity
        if (!isGrounded)
        {
            Vector3 extraGravity = Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
            playerRigidbody.AddForce(extraGravity, ForceMode.VelocityChange);
        }
        

        Vector3 movement = sprinting
            ? new Vector3(horizontalInput * sprintSpeed, playerRigidbody.linearVelocity.y, 0) 
            : new Vector3(horizontalInput * speed, playerRigidbody.linearVelocity.y, 0);


        if (knockbackTimer > 0f)
        {
            playerRigidbody.linearVelocity = externalVelocity;
            knockbackTimer -= Time.fixedDeltaTime;
        }
        else
        {
            playerRigidbody.linearVelocity = movement;
        }

        playerAnimator.SetFloat("speed", Math.Abs(movement.x));
    }

    public void ApplyKnockback(Vector3 force, float duration)
    {
        externalVelocity = force;
        knockbackTimer = duration;

        // Set color to red when damaged
        ApplyShaderUniformToChildren(pineappleMeshPart, "baseColorFactor", Color.red);

        // Restore after 0.2 seconds
        StartCoroutine(RestoreColor());
    }

    void Jump()
    {
        if (jumpRequested)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            doubleJump = true;
            jumpRequested = false;
        }
        else if (doubleJumpRequested)
        {
            playerAnimator.CrossFade("doubleJump", 0.1f);
            playerRigidbody.AddForce(Vector3.up * (jumpForce * 1.1f), ForceMode.Impulse);
            doubleJump = false;
            doubleJumpRequested = false;
        }
    }

    void AttackUp()
    {
        playerAnimator.Play("attackUp");
        playerAnimator.SetBool("attacking", true);
    }

    void Attack()
    {
        playerAnimator.Play("attack");
        playerAnimator.SetBool("attacking", true);
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

    void ApplyShaderUniformToChildren(GameObject root, string uniformName, Color value)
    {
        foreach (Transform child in root.transform)
        {
            MeshRenderer renderer = child.GetComponent<MeshRenderer>();
            if (renderer != null && renderer.material != null)
            {
                renderer.material.SetColor(uniformName, value);
            }

            // Recursively search this child’s children
            ApplyShaderUniformToChildren(child.gameObject, uniformName, value);
        }
    }


    IEnumerator RestoreColor()
    {
        yield return new WaitForSeconds(0.2f);
        ApplyShaderUniformToChildren(pineappleMeshPart, "baseColorFactor", Color.white); // Or whatever the original is
    }
}
