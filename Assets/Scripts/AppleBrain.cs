using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SocialPlatforms.Impl;

public class AppleBrain : MonoBehaviour
{
    public float gravitySlowdown = 0.5f;
    public float countdownTime;
    public float shrinkSpeed;
    public LayerMask ignoredLayersOnDeath;
    public int appleDamage;
    Collider collider;
    HealthBar healthbar;
    Rigidbody body;
    Animator animator;
    NPCExploder exploder;
    public bool exploded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = gameObject.GetComponent<Collider>();
        body = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
        body.linearVelocity = body.linearVelocity + Vector3.down;
        exploder = gameObject.GetComponent<NPCExploder>();

        healthbar = GameObject.Find("HealthBarElem").GetComponent<HealthBar>();
    }

    void FixedUpdate()
    {
        // Reduce gravity effect by applying upward force
        Vector3 reducedGravity = Physics.gravity * gravitySlowdown; // 0.5 = half gravity
        body.AddForce(-reducedGravity, ForceMode.Acceleration);

        if (Mathf.Abs(body.linearVelocity.y) < 0.1f)
        {
            animator.SetBool("onGround", true);
            StartCoroutine(CountdownAndShrinkApple());
            body.excludeLayers = ignoredLayersOnDeath;
        }
    }


    public IEnumerator CountdownAndShrinkApple()
    {
        // Countdown logic
        float timeRemaining = countdownTime;
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        // Shrink parts gradually
        float shrinkFactor = 1f;
        while (shrinkFactor > 0)
        {
            shrinkFactor -= shrinkSpeed * Time.deltaTime; // Shrink over time
            shrinkFactor = Mathf.Clamp(shrinkFactor, 0f, 1f); // Clamp the shrink factor to prevent going below 0

            gameObject.transform.localScale = new Vector3(shrinkFactor, shrinkFactor, shrinkFactor); // Shrink the parts
            yield return null;
        }

        // Destroy the root Bob object as well, if needed
        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerController>().attacking)
            {
                PlayerController controller = collision.gameObject.GetComponent<PlayerController>();

                controller.health -= appleDamage;
                healthbar.SetHealth((float)controller.health);
                controller.OnPlayerDamagedOrMissed();
                exploder.Explode();
                animator.SetBool("dead", true);
                exploded = true;
            }

        }
        else if (collision.gameObject.CompareTag("Floor") && !exploded)
        {
            animator.SetBool("onGround", true);
            StartCoroutine(CountdownAndShrinkApple());
        }
        body.excludeLayers = ignoredLayersOnDeath;
    }
}
