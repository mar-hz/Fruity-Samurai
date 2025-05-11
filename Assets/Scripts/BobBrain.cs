using UnityEngine;

public class BobBrain : MonoBehaviour
{
    Animator animator;
    Rigidbody body;
    BobExploder exploder;
    Collider collider;
    Transform bobRoot;
    bool exploded = false;
    public float gravitySlowdown = 0.5f;
    public float knockbackForce = 1000f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody>();
        exploder = gameObject.GetComponent<BobExploder>();
        collider = gameObject.transform.Find("Collider").gameObject.GetComponent<Collider>();
        bobRoot = gameObject.transform.Find("Character");

        gameObject.transform.Find("Collider").gameObject.GetComponent<GenericListener>().onCollisionEnter.AddListener(OnCollisionEnter);
        body.linearVelocity = body.linearVelocity + Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        if (collider != null)
            collider.transform.rotation = bobRoot.rotation;
    }

    void FixedUpdate()
    {
        // Reduce gravity effect by applying upward force
        Vector3 reducedGravity = Physics.gravity * gravitySlowdown; // 0.5 = half gravity
        body.AddForce(-reducedGravity, ForceMode.Acceleration);

        if (Mathf.Abs(body.linearVelocity.y) < 0.1f && !exploded)
        {
            animator.SetBool("onGround", true);
            StartCoroutine(exploder.CountdownAndShrinkBob());
            body.excludeLayers = exploder.getIgnoredLayers();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerController>().attacking)
            {
                exploder.Explode();
                Destroy(collider);
                collider = null;
                exploded = true;
            } else
            {
                Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

                float direction = Mathf.Sign(collision.transform.position.x - transform.position.x);

                float knockbackForce = 10f;
                Vector3 knockback = new Vector3(direction * knockbackForce, 0, 0);

                collision.gameObject.GetComponent<PlayerController>().ApplyKnockback(knockback, 0.2f); // 0.2s knockback override
            }
            body.excludeLayers = exploder.getIgnoredLayers();
        }
        else if(collision.gameObject.CompareTag("Floor") && !exploded)
        {
           animator.SetBool("onGround", true);
           StartCoroutine(exploder.CountdownAndShrinkBob());
           body.excludeLayers = exploder.getIgnoredLayers();
        }
    }
}
