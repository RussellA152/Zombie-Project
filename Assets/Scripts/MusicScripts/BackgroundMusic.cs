using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic current;
    private AudioSource this_audiosource;

    private void Awake()
    {
        if (current != this)
        {
            if (current != null)
            {
                Destroy(current.gameObject);
            }
            DontDestroyOnLoad(gameObject);
            current = this;
        }
    }
    private void Start()
    {
        //this_audiosource = GetComponent<AudioSource>();

    }
}
