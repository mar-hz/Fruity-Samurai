using UnityEngine;
using static HumanCustomization;

public class NPCColors
{
    public static Color[] eyeColors = {
        new Color(0.6f, 0.8f, 0.9f),   // Light Blue
        new Color(0.3f, 0.5f, 0.8f),   // Deep Blue
        new Color(0.2f, 0.3f, 0.1f),   // Dark Green
        new Color(0.5f, 0.8f, 0.3f),   // Bright Green
        new Color(0.6f, 0.4f, 0.2f),   // Hazel
        new Color(0.4f, 0.3f, 0.1f),   // Light Brown
        new Color(0.2f, 0.15f, 0.05f), // Dark Brown
        new Color(0.15f, 0.15f, 0.15f) // Black
    };

    public static Color[] hairColors = {
        new Color(1.0f, 0.85f, 0.6f),   // Blonde
        new Color(0.8f, 0.6f, 0.4f),    // Light Brown
        new Color(0.6f, 0.4f, 0.2f),    // Medium Brown
        new Color(0.3f, 0.2f, 0.1f),    // Dark Brown
        new Color(0.15f, 0.1f, 0.05f),  // Black
        new Color(0.9f, 0.3f, 0.2f),    // Redhead (Ginger)
        new Color(0.5f, 0.5f, 0.5f),    // Gray
        new Color(1.0f, 1.0f, 1.0f)     // White
    };

    public static Color[] armorColors = {
        FromHex("#ff0078"), 
        FromHex("#007bff"),   
        FromHex("#7e00ff"),  
        FromHex("#3dff00"),   
    };

    public static Color GetRandomSkinTone()
    {
        // Define a range of human skin tones in HSV
        float hue = Random.Range(0.05f, 0.12f); // Skin tones fall roughly in this hue range
        float saturation = Random.Range(0.3f, 0.6f); // Avoid extreme saturation
        float value = Random.Range(0.4f, 0.9f); // Control light/dark variation

        return Color.HSVToRGB(hue, saturation, value);
    }

    public static Hairstyle GetRandomHairStyle()
    {
        return Hairstyle.values[Random.Range(0, Hairstyle.values.Length)];
    }

    public static Color GetRandomEyeColor()
    {
        return NPCColors.eyeColors[Random.Range(0, NPCColors.eyeColors.Length)];
    }

    public static Color GetRandomHairColor()
    {
        return NPCColors.hairColors[Random.Range(0, NPCColors.hairColors.Length)];
    }

    public static Color GetRandomPantColor()
    {
        return NPCColors.armorColors[Random.Range(0, NPCColors.armorColors.Length)];
    }

    public static Color GetRandomArmorColor()
    {
        return NPCColors.armorColors[Random.Range(0, NPCColors.armorColors.Length)];
    }

    public static Color GetRandomPastelColor()
    {
        float r = Random.Range(0.5f, 1.0f); // Light Red
        float g = Random.Range(0.5f, 1.0f); // Light Green
        float b = Random.Range(0.5f, 1.0f); // Light Blue
        return new Color(r, g, b);
    }

    public static Color FromHex(string hex)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }
        else
        {
            Debug.LogWarning("Invalid hex string: " + hex);
            return Color.white;
        }
    }

}
