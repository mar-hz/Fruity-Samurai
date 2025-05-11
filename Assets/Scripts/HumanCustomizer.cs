using Unity.VisualScripting;
using UnityEngine;
using UnityGLTF;
using static HumanCustomization;

public class HumanCustomizer : MonoBehaviour
{
    public HumanCustomization customization;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void ApplyColorToParts()
    {
        Transform shirtT = transform.Find("Character/Torso/shirt");
        if (shirtT != null)
        {
            Material shirtMat = shirtT.gameObject.GetComponent<MeshRenderer>().material;
            shirtMat.SetColor("baseColorFactor", customization.shirtColor);
        }

        Transform skirt = transform.Find("Character/Torso/skirt/shirt");
        if (skirt != null)
        {
            Material shirtMat = skirt.gameObject.GetComponent<MeshRenderer>().material;
            shirtMat.SetColor("baseColorFactor", customization.shirtColor);
        }


        Transform legL = transform.Find("Character/Legs/LegL/sphere");
        if (legL != null)
        {
            Material pantMat = legL.gameObject.GetComponent<MeshRenderer>().material;
            pantMat.SetColor("baseColorFactor", customization.pantColor);
        }

        Transform legR = transform.Find("Character/Legs/LegR/sphere");
        if (legR != null)
        {
            Material pantMat = legR.gameObject.GetComponent<MeshRenderer>().material;
            pantMat.SetColor("baseColorFactor", customization.pantColor);
        }

        Transform leftArmT = transform.Find("Character/Torso/armL");
        if (leftArmT != null)
        {
            Transform realArm = leftArmT.Find("armL");
            if (realArm != null)
            {
                Material shirtMat = realArm.gameObject.GetComponent<MeshRenderer>().material;
                shirtMat.SetColor("baseColorFactor", customization.shirtColor);
            }

            Transform realHand = leftArmT.Find("handL");
            if (realHand != null)
            {
                Material skinMaterial = realHand.gameObject.GetComponent<MeshRenderer>().material;
                skinMaterial.SetColor("baseColorFactor", customization.skinColor);
            }

            Transform shoulderPad = leftArmT.Find("bone/shoulderPadL");
            if (shoulderPad != null)
            {
                Material shirtMat = shoulderPad.gameObject.GetComponent<MeshRenderer>().material;
                shirtMat.SetColor("baseColorFactor", customization.shirtColor);
            }
        }

        Transform rightArmT = transform.Find("Character/Torso/armR");
        if (rightArmT != null)
        {
            Transform realArm = rightArmT.Find("armR");
            if (realArm != null)
            {
                Material shirtMat = realArm.gameObject.GetComponent<MeshRenderer>().material;
                shirtMat.SetColor("baseColorFactor", customization.shirtColor);
            }

            Transform realHand = rightArmT.Find("handR");
            if (realHand != null)
            {
                Material skinMaterial = realHand.gameObject.GetComponent<MeshRenderer>().material;
                skinMaterial.SetColor("baseColorFactor", customization.skinColor);
            }

            Transform shoulderPad = rightArmT.Find("bone2/shoulderPadR");
            if (shoulderPad != null)
            {
                Material shirtMat = shoulderPad.gameObject.GetComponent<MeshRenderer>().material;
                shirtMat.SetColor("baseColorFactor", customization.shirtColor);
            }
        }

        Transform neckT = transform.Find("Character/Torso/neck");
        if (neckT != null)
        {
            Material skinMaterial = neckT.gameObject.GetComponent<MeshRenderer>().material;
            skinMaterial.SetColor("baseColorFactor", customization.skinColor);
        }

        Transform headT = transform.Find("Character/Torso/head");
        if (headT != null)
        {
            Transform helmet = headT.Find("hat");
            if (helmet != null)
            {
                Material helmetMat = helmet.gameObject.GetComponent<MeshRenderer>().material;
                helmetMat.SetColor("baseColorFactor", customization.helmetColor);
            }

            Transform faceT = headT.Find("face");
            if (faceT != null)
            {
                Transform earR = faceT.Find("earR");
                if (earR != null)
                {
                    Material skinMaterial = earR.gameObject.GetComponent<MeshRenderer>().material;
                    skinMaterial.SetColor("baseColorFactor", customization.skinColor);
                }

                Transform earL = faceT.Find("earL");
                if (earL != null)
                {
                    Material skinMaterial = earL.gameObject.GetComponent<MeshRenderer>().material;
                    skinMaterial.SetColor("baseColorFactor", customization.skinColor);
                }

                Transform face = faceT.Find("face");
                if (face != null)
                {
                    Material skinMaterial = face.gameObject.GetComponent<MeshRenderer>().material;
                    skinMaterial.SetColor("baseColorFactor", customization.skinColor);
                }

                Transform eyelids = faceT.Find("eyelids");
                if (eyelids != null)
                {
                    Material skinMaterial = eyelids.gameObject.GetComponent<MeshRenderer>().material;
                    skinMaterial.SetColor("baseColorFactor", customization.skinColor);

                }

                Transform pupils = faceT.Find("pupils");
                if (pupils != null)
                {
                    Material pupilMaterial = pupils.gameObject.GetComponent<MeshRenderer>().material;
                    pupilMaterial.SetColor("baseColorFactor", customization.eyeColor);

                }


                Transform hairShortT = faceT.Find("hairShort");
                if (hairShortT != null)
                {
                    Material hairMaterial = hairShortT.gameObject.GetComponent<MeshRenderer>().material;
                    hairMaterial.SetColor("baseColorFactor", customization.hairColor);
                }

                Transform hairLongT = faceT.Find("hairLong");
                if (hairLongT != null)
                {
                    Material hairMaterial = hairLongT.gameObject.GetComponent<MeshRenderer>().material;
                    hairMaterial.SetColor("baseColorFactor", customization.hairColor);
                }
            }
        }
    }

    public void ApplyHairStyle()
    {
        foreach (Hairstyle toDisable in Hairstyle.values)
        {
            if (toDisable.Id != customization.hairstyle.Id)
            {
                transform.Find(toDisable.ModelPath).GetComponent<MeshRenderer>().enabled = false;
            } 
        }
        transform.Find(customization.hairstyle.ModelPath).GetComponent<MeshRenderer>().enabled = true;
    }

    public void HideHat(bool hide)
    {
        transform.Find("Character/Torso/head/hat").GetComponent<MeshRenderer>().enabled = !hide;
    }
    public void HideVest(bool hide)
    {
        transform.Find("Character/Torso/vest").GetComponent<MeshRenderer>().enabled = !hide;
        transform.Find("Character/Torso/stripes").GetComponent<MeshRenderer>().enabled = !hide;
    }

    void Start()
    {
        ApplyColorToParts();
        ApplyHairStyle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
