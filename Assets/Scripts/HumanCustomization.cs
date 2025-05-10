using System;
using UnityEngine;
using static HumanCustomization;
using UnityEngine.Rendering;

[System.Serializable]
public class HumanCustomization : ICloneable
{
    public Color hairColor = Color.white;

    public Color skinColor = Color.white;

    public Color eyeColor = Color.white;

    public Color pantColor = Color.white;

    public Color shirtColor = Color.white;

    public Color helmetColor = Color.white;

    public Hairstyle hairstyle = Hairstyle.Short;

    public object Clone()
    {
        return new HumanCustomization
        {
            hairColor = this.hairColor,
            skinColor = this.skinColor,
            eyeColor = this.eyeColor,
            pantColor = this.pantColor,
            shirtColor = this.shirtColor,
            helmetColor = this.helmetColor,
            hairstyle = this.hairstyle
        };
    }

    public static string ToHex(Color color)
    {
        Color32 c = color;
        return $"{c.r:X2}{c.g:X2}{c.b:X2}";
    }

    public static Color FromHex(string hex)
    {
        hex = "#" + hex;
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
            return color;
        Debug.LogWarning($"Invalid hex color: {hex}");
        return Color.white;
    }


    [System.Serializable]
    public class Hairstyle
    {
        public static readonly Hairstyle Short = new Hairstyle("Short", 1, "Character/Torso/head/face/hairShort");
        public static readonly Hairstyle Long = new Hairstyle("Long", 2, "Character/Torso/head/face/hairLong");

        public string Name { get; }
        public int Id { get; }

        public string ModelPath { get; }
        private Hairstyle(string name, int id, string path)
        {
            Name = name;
            Id = id;
            ModelPath = path;
        }

        public static Hairstyle byId(int id)
        {
            foreach (Hairstyle style in values)
            {
                if (style.Id == id)
                    return style;
            }
            Debug.LogWarning("Invalid Hairstyle by id " + id);
            return Short;
        }

        public override string ToString() => Name;

        public static readonly Hairstyle[] values = { Short, Long };
    }

}
