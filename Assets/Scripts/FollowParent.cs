using UnityEngine;

public class FollowParent : MonoBehaviour
{
    public Quaternion shadowRotation = Quaternion.Euler(0f, 0f, 0f); // Set your specific rotation here
    GameObject olParent;
    public Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        olParent = transform.parent.gameObject;
        transform.parent = null; // Unparent this object at runtime
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        if (olParent == null || olParent.GetComponent<BobBrain>() != null && olParent.GetComponent<BobBrain>().exploded || olParent.GetComponent<AppleBrain>() !=null && olParent.GetComponent<AppleBrain>().exploded)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 worldPos = olParent.transform.position + offset;
        worldPos.y = 2.628906f;  
        transform.position = worldPos;

        transform.rotation = shadowRotation;

    }

}
