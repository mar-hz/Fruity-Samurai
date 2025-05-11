using UnityEngine;

public class NPCRandomizer : MonoBehaviour
{
    private HumanCustomizer customizer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        customizer = gameObject.GetComponent<HumanCustomizer>();
        Color armorColor = NPCColors.GetRandomArmorColor();

        customizer.customization.skinColor = NPCColors.GetRandomSkinTone();
        customizer.customization.eyeColor = NPCColors.GetRandomEyeColor();
        customizer.customization.hairColor = NPCColors.GetRandomHairColor();
        customizer.customization.pantColor = armorColor;
        customizer.customization.shirtColor = armorColor;
        customizer.customization.helmetColor = armorColor;
        customizer.customization.hairstyle = NPCColors.GetRandomHairStyle();

        customizer.ApplyColorToParts();
        customizer.ApplyHairStyle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
