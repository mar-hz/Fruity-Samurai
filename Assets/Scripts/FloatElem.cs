using UnityEngine;
using System.Collections;

public class FloatElem : MonoBehaviour
{
    public int direction;
    float curTime = 0f;
    float interval = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime >= interval) {
            if (direction == 0) {
                transform.Translate(0, 5, 0);
                direction = 1;
            } else {
                transform.Translate(0, -5, 0);
                direction = 0;
            }
            curTime = 0f;
        }
    }
}
