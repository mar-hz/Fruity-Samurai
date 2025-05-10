using UnityEngine;

public class BobBrain : MonoBehaviour
{
    Animator animator;
    Rigidbody body;
    BobExploder exploder;
    Collider collider;
    Transform bobRoot;
    bool exploded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody>();
        exploder = gameObject.GetComponent<BobExploder>();
        collider = gameObject.transform.Find("Collider").gameObject.GetComponent<Collider>();
        bobRoot = gameObject.transform.Find("Character");

        gameObject.transform.Find("Collider").gameObject.GetComponent<GenericListener>().onCollisionEnter.AddListener(OnCollisionEnter);
    }

    // Update is called once per frame
    void Update()
    {
        if (collider != null)
            collider.transform.rotation = bobRoot.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            exploder.Explode();
            body.excludeLayers = exploder.getIgnoredLayers();
            Destroy(collider);
            collider = null;
            exploded = true;
        } else if(collision.gameObject.tag == "Floor" && !exploded)
        {
           animator.SetBool("onGround", true);
           StartCoroutine(exploder.CountdownAndShrinkBob());
        }
    }
}
