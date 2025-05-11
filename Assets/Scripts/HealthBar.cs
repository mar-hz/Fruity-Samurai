using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
    public float Health;
    float MaxHealth = 100f;

    public RectTransform healthBar;
    float originalWidth = 500f;

    void Start()
    {
        Health = MaxHealth;
        // Debug.Log("Assigned bar: " + healthBar.name);
        // Debug.Log("Initial width: " + originalWidth);
    }

    public void SetHealth(float health)
    {
        // Debug.Log("received health " + health);
        Health = health;
        // Debug.Log("og " + originalWidth);
        float newWidth = (Health / MaxHealth) * originalWidth; 
        healthBar.sizeDelta = new Vector2(newWidth, 60f);
        // Debug.Log("new width " + healthBar.sizeDelta);
    }
}
