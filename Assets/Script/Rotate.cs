using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    //âÒì]ë¨ìx
    public float rotationSpeed = 0f;
    //åªç›ÇÃâÒì]äpìx
    private float rotation_z = 0f;

    public float Acceleration = 120f;
    private float[] rotationTimes = new float[] { 7, 5, 4, 5, 6, 7, 4, 3, 7, 6, 2, 6, 6, 4, 5, 6, 2, 7, 3, 3, 2, 6, 6, 7, 3, 5, 6, 5, 5, 5, 5, 3, 7, 7, 3, 3, 6, 2 };
    int i = 0;

    public float Timer = 0f;

    public bool start = false;
    public bool finish = false;
    public string name;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            start = true;
        }

        if(start && !finish)
        {
            rotate();
        }
    }

    void rotate()
    {
        Timer += Time.deltaTime;
        if(Timer < 1f)
        {
            rotationSpeed += Acceleration * Time.deltaTime;
        }
        else if(Timer > rotationTimes[i] - 1f)
        {
            rotationSpeed -= Acceleration * Time.deltaTime;
        }
        if(Timer > rotationTimes[i])
        {
            Acceleration *= -1;
            Timer = 0;
            i += 1;
            if(i == rotationTimes.Length)
            {
                finish = true;
                return;
            }
        }
        rotation_z += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, rotation_z);
    }
}
