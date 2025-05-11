using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem.XR;

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

    HealthBar healthbar;
    TMP_Text score;
    TMP_Text multiplier;

    public static Dictionary<string, int> armorPoints = new()  
    {
        { "#FF0078", 1 },
        { "#007BFF", 3 },
        { "#7E00FF", 5 },
        { "#3DFF00", 10 },
    };

    HumanCustomizer custom;

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

        healthbar = GameObject.Find("HealthBarElem").GetComponent<HealthBar>();
        custom = gameObject.GetComponent<HumanCustomizer>();
        score = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        multiplier = GameObject.Find("MultiplierText").GetComponent<TMP_Text>();

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

                if (collision.gameObject.GetComponent<PlayerController>().alive)
                {
                    PlayerController controller = collision.gameObject.GetComponent<PlayerController>();

                    controller.score += (int)(armorPoints["#" + ColorUtility.ToHtmlStringRGB(custom.customization.shirtColor)] * controller.multiplier);
                    healthbar.SetHealth((float)collision.gameObject.GetComponent<PlayerController>().health);
                    score.text = controller.score.ToString();
                    controller.RegisterHit();
                    multiplier.text = controller.multiplier.ToString();
                }
            } else
            {
                Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

                float direction = Mathf.Sign(collision.transform.position.x - transform.position.x);

                float knockbackForce = 10f;
                Vector3 knockback = new Vector3(direction * knockbackForce, 0, 0);
                PlayerController controller = collision.gameObject.GetComponent<PlayerController>();

                controller.ApplyKnockback(knockback, 0.2f); // 0.2s knockback override
                controller.OnPlayerDamagedOrMissed();
                multiplier.text = controller.multiplier.ToString();

                if (controller.alive && !animator.GetBool("onGround") && !exploded) {
                    controller.health -= armorPoints["#" + ColorUtility.ToHtmlStringRGB(custom.customization.shirtColor)];
                    healthbar.SetHealth((float) controller.health);
                }
            }
         
            body.excludeLayers = exploder.getIgnoredLayers();
        }
        else if(collision.gameObject.CompareTag("Floor") && !exploded)
        {
           animator.SetBool("onGround", true);
           StartCoroutine(exploder.CountdownAndShrinkBob());
           body.excludeLayers = exploder.getIgnoredLayers();
            PlayerController controller = GameObject.Find("Player").GetComponent<PlayerController>();
            controller.OnPlayerDamagedOrMissed();
            multiplier.text = controller.multiplier.ToString();
        }
    }
}
