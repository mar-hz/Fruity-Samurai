using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCExploder : MonoBehaviour
{
    public float explosionForce = 300f;
    public float explosionUpwardModifier = 2f;
    public float explosionRadius = 5f;
    public float countdownTime;
    public float shrinkSpeed;
    private List<Transform> explodedParts = new List<Transform>();
    public LayerMask excludedLayers;
    public string deadEnemies;
    public void Explode()
    {
        GetComponent<Animator>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;

        Transform[] parts = GetComponentsInChildren<Transform>(includeInactive: false);

        for (int i = 1; i < parts.Length; i++)
        {
            Transform part = parts[i];
            part.gameObject.layer = LayerMask.NameToLayer(deadEnemies);
            // Skip if it already has a Rigidbody (optional)
            if (part.GetComponent<Rigidbody>() != null)
                continue;

            // Add Rigidbody and Collider
            Rigidbody rb = part.gameObject.AddComponent<Rigidbody>();
            rb.mass = 2.5f;
            rb.excludeLayers = excludedLayers;
            if (part.GetComponent<Collider>() == null)
            {
                Collider col = part.gameObject.AddComponent<BoxCollider>();
                col.excludeLayers = excludedLayers;
            }

            // Optional: reparent to world if you're destroying Bob later
            // part.SetParent(null);

            // Apply explosion force with random direction
            Vector3 explosionOrigin = transform.position + Vector3.up;

            // Randomize explosion direction (within a spherical range)
            Vector3 randomDirection = Random.insideUnitSphere;  // Returns a random point inside a unit sphere
            randomDirection.Normalize();  // Normalize to keep consistent force magnitude

            // Apply random explosion force
            rb.AddForce(randomDirection * explosionForce, ForceMode.Impulse);

            // Apply additional explosion force (optional)
            rb.AddExplosionForce(
                explosionForce,
                explosionOrigin,
                explosionRadius,
                explosionUpwardModifier,
                ForceMode.Impulse
            );

            if (part != transform && part.GetComponent<MeshRenderer>() != null)
            {
                explodedParts.Add(part);
                part.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
        }
        StartCoroutine(CountdownAndShrink());
    }

    public LayerMask getIgnoredLayers()
    {
        return excludedLayers;
    }

    private IEnumerator CountdownAndShrink()
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

            foreach (Transform part in explodedParts)
            {
                part.localScale = new Vector3(shrinkFactor, shrinkFactor, shrinkFactor); // Shrink the parts
            }
            yield return null;
        }

        // Destroy the parts after shrinking is complete
        foreach (Transform part in explodedParts)
        {
            Destroy(part.gameObject); // Destroy each part
        }

        // Destroy the root Bob object as well, if needed
        Destroy(gameObject);
    }

    public IEnumerator CountdownAndShrinkMainobject()
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
}
