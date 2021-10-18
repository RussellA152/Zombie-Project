using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public GameObject Light;
    private bool lightActive;
    // Start is called before the first frame update
    void Start()
    {
        lightActive = true;
        Light.SetActive(lightActive);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !lightActive)
        {
            lightActive = true;
            Light.SetActive(lightActive);
        }
        else if(Input.GetKeyDown(KeyCode.T) && lightActive)
        {
            lightActive = false;
            Light.SetActive(lightActive);
        }
    }
}
