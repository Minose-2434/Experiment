using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    //‰ñ“]‘¬“x
    public float rotationSpeed = 60f;
    //x²‰ñ“]Šp“x‚ÌÅ‘å’l
    public float max_rotation_x = 60f;
    //Œ»İ‚Ì‰ñ“]Šp“x
    private float rotation_x = 0f;
    private float rotation_y = 0f;

    // Update is called once per frame
    void Update()
    {
        rotation_y += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, rotation_y);
    }
}
