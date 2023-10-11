using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioClip sound1;
    AudioSource audioSource;
    public bool start;
    public float Timer;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            start = true;
        }

        if (start)
        {
            Timer += Time.deltaTime;
            if(Timer > 60)
            {
                audioSource.PlayOneShot(sound1);
                Timer = 0;
            }
        }
    }
}
