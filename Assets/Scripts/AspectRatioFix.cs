using UnityEngine;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioFix : MonoBehaviour
{
    public float targetAspect = 16f / 9f;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1f)
        {
            // Add letterbox (black bars on top and bottom)
            Rect rect = cam.rect;
            rect.width = 1f;
            rect.height = scaleHeight;
            rect.x = 0f;
            rect.y = (1f - scaleHeight) / 2f;
            cam.rect = rect;
        }
        else
        {
            // Add pillarbox (black bars on sides)
            float scaleWidth = 1f / scaleHeight;

            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f;
            rect.y = 0f;
            cam.rect = rect;
        }
    }
}