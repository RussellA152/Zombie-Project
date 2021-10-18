using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public bool isFlickering;
    public float timeDelay;
    private Light this_light;



    // Start is called before the first frame update
    void Start()
    {
        
        this_light = this.gameObject.GetComponent<Light>();
        StartCoroutine(FlickerBeginDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if(isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }
    }

    IEnumerator FlickerBeginDelay()
    {
        yield return new WaitForSeconds(1.5f);
        isFlickering = false;
    }
    IEnumerator FlickeringLight()
    {
        isFlickering = true;
        this_light.enabled = false;
        timeDelay = Random.Range(.09f, .11f);
        yield return new WaitForSeconds(timeDelay);

        this_light.enabled = true;
        timeDelay = Random.Range(8f, 10f);
        yield return new WaitForSeconds(timeDelay);
        isFlickering = false;
    }
}
