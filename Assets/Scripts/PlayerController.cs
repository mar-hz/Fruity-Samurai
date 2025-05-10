using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 5f;
    public float gravityMultiplier = 2f; // Add gravity multiplier

    Rigidbody rb;

    private float x;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    void Update()
    {
        // Get input here
        x = Input.GetAxis("Horizontal");

        // Jump — preserve Y velocity from physics
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.linearVelocity.y) <= 0.01f)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpSpeed, rb.linearVelocity.z);
        }
    }

    void FixedUpdate()
    {
        // Apply movement in FixedUpdate
        Vector3 velocity = new Vector3(x * speed, rb.linearVelocity.y, 0);
        rb.linearVelocity = velocity;

        // Apply custom gravity multiplier
        rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
    }

}