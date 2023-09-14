using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReset : MonoBehaviour
{
    public GameObject Camera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            this.gameObject.transform.position -= Camera.transform.position;
            this.gameObject.transform.rotation = Quaternion.Euler(-Camera.transform.eulerAngles.x, -Camera.transform.eulerAngles.y, -Camera.transform.eulerAngles.z);
            Debug.Log("get");
        }
    }
}
