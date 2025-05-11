using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using static UnityEngine.LightAnchor;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
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
    bool manualJump = false;

    public int health;
    public bool alive;
    public int score = 0;
    public int comboThreshold = 5;
    public int comboCounter = 0;
    public float multiplier { get; private set; } = 1f;
    public float maxMultiplier = 10.0f;
    public float dashSpeed = 25f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1.0f;
    private float lastDashTime = -Mathf.Infinity;
    private bool isDashing = false;
    private float dashEndTime;


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();
        playerAnimator = GetComponentInChildren<Animator>();
        playerTransform.rotation = Quaternion.Euler(0, -90, 0);
        health = 100;
        alive = true;
    }

    void Update()
    {
        if (!alive)
        {
            return;
        }

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
            }
            else
            {
                canAttack = false;
            }

        }
        else
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
        bool canJump = isGrounded || manualJump;

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded && Time.time >= lastDashTime + dashCooldown)
        {
            StartDash();
        }

        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }
        else if (!canJump && doubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            doubleJumpRequested = true;
        }

        if (!stateInfo.IsName("doubleJump") && !stateInfo.IsName("wallCling"))
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

        if (health <= 0)
        {
            alive = false;
        }
    }

    float horizontalInput;
    float verticalInput;
    bool sprinting;
    bool jumpRequested, doubleJumpRequested;
    public bool isWallClinging = false, wallColliding = false;
    public int wallDir = 0; // 1 = right wall, -1 = left wall

    public void RegisterHit()
    {
        comboCounter++;
        multiplier = 1f + Mathf.Floor(comboCounter / (float)comboThreshold);
        multiplier = Mathf.Min(multiplier, maxMultiplier); // Cap it
    }

    public void OnPlayerDamagedOrMissed()
    {
        comboCounter = 0;
        multiplier = 1f;
    }
    void StartDash()
    {
        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        lastDashTime = Time.time;

        // Face direction determines dash
        float direction = horizontalInput != 0 ? Mathf.Sign(horizontalInput) : (playerTransform.rotation.eulerAngles.y == 90 ? 1f : -1f);
        playerRigidbody.linearVelocity = new Vector3(direction * dashSpeed, 0f, 0f);
    }

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

       
        Vector3 movement = new Vector3(horizontalInput * speed, playerRigidbody.linearVelocity.y, 0);


        if (knockbackTimer > 0f)
        {
            playerRigidbody.linearVelocity = externalVelocity;
            knockbackTimer -= Time.fixedDeltaTime;
        }
        else if (isDashing)
        {
            if (Time.time >= dashEndTime)
            {
                isDashing = false;
            }
        }
        else
        {
            playerRigidbody.linearVelocity = movement;
        }

        playerAnimator.SetFloat("speed", Math.Abs(movement.x));

        bool isAirborne = !isGrounded;

        if (wallColliding && isAirborne && !(jumpRequested || doubleJumpRequested) && movement.x == 0)
        {

            // Stick to wall
            playerRigidbody.linearVelocity = new Vector3(0f, 0f, 0f); // Cancel gravity
            isWallClinging = true;
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("wallCling"))
                playerAnimator.Play("walllCling", 0);

            playerAnimator.SetBool("sticking", true);
            manualJump = true;

            if (wallDir == 1)
                playerTransform.rotation = Quaternion.Euler(0, 90, 0); // Facing right wall
            else if (wallDir == -1)
                playerTransform.rotation = Quaternion.Euler(0, -90, 0); // Facing left wall

        }
        else
        {
            playerAnimator.SetBool("sticking", false);
            manualJump = false;
            isWallClinging = false;
        }
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
            Vector3 jumpDirection = Vector3.up;

            if (isWallClinging)
            {
                jumpDirection += Vector3.left * wallDir; // jump opposite to wall
                isWallClinging = false;
            }

            playerRigidbody.AddForce(jumpDirection.normalized * jumpForce, ForceMode.Impulse);
            doubleJump = true;
            jumpRequested = false;
        }
        else if (doubleJumpRequested)
        {
            playerAnimator.CrossFade("doubleJump", 0.1f);
            doubleJump = false;
            doubleJumpRequested = false;
            Vector3 jumpDirection = Vector3.up;

            if (isWallClinging)
            {
                jumpDirection += Vector3.left * wallDir; // jump opposite to wall
                isWallClinging = false;
            }
            playerRigidbody.AddForce(jumpDirection.normalized * (jumpForce * 1.1f), ForceMode.Impulse);

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

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // Get the contact point normal to determine direction
            Vector3 normal = collision.contacts[0].normal;
            float dot = Vector3.Dot(normal, transform.right);

            if (dot > 0.5f)
                wallDir = -1;
            else if (dot < -0.5f)
                wallDir = 1;
            wallColliding = true;
        }
        else
        {
            wallDir = 0;
            wallColliding = false;
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

            // Recursively search this child�s children
            ApplyShaderUniformToChildren(child.gameObject, uniformName, value);
        }
    }

    IEnumerator RestoreColor()
    {
        yield return new WaitForSeconds(0.2f);
        ApplyShaderUniformToChildren(pineappleMeshPart, "baseColorFactor", Color.white); // Or whatever the original is
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // Get the contact point normal to determine direction
            Vector3 normal = collision.contacts[0].normal;
            float dot = Vector3.Dot(normal, transform.right);

            if (dot > 0.5f)
                wallDir = -1;
            else if (dot < -0.5f)
                wallDir = 1;
            wallColliding = true;
        } else
        {
            wallDir = 0;
            wallColliding = false;
        }
    }

    void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        float rayDistance = 1f;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, Vector3.left * rayDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, Vector3.right * rayDistance);
    }
}
