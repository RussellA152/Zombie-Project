using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweringLights : MonoBehaviour
{
    public GameObject[] Lights;

    // Start is called before the first frame update
    void Start()
    {
        PowerEvent.OnPowered += TurnOnLight;

        foreach (GameObject light in Lights)
        {
            light.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        PowerEvent.OnPowered -= TurnOnLight;
    }

    void TurnOnLight()
    {
        foreach(GameObject light in Lights)
        {
            light.SetActive(true);
        }
        Debug.Log("Lights are now powered and on");

        //Should we unsubscribed after turning on lights?
        //PowerEvent.OnPowered -= TurnOnLight;
    }
}
