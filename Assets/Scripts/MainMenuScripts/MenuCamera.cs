using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{

    public GameObject MyMenuCamera;
    public float finalPositionZ;
    public float timeToReachMonitor;
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.moveZ(MyMenuCamera, finalPositionZ, timeToReachMonitor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
