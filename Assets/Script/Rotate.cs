using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    //��]���x
    public float rotationSpeed = 60f;
    //x����]�p�x�̍ő�l
    public float max_rotation_x = 60f;
    //���݂̉�]�p�x
    private float rotation_x = 0f;
    private float rotation_y = 0f;

    // Update is called once per frame
    void Update()
    {
        rotation_y += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, rotation_y);
    }
}
