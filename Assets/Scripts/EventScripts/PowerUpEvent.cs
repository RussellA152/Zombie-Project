using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEvent : MonoBehaviour
{
    public static PowerUpEvent current;

    public bool hasDoublePoints;
    public bool hasInstantKill;

    private void Start()
    {
        hasDoublePoints = false;
        hasInstantKill = false;
    }
    private void Update()
    {
        //Debug.Log(PowerUpEvent.current.hasInstantKill);
        //Debug.Log(hasDoublePoints);
    }
    private void Awake()
    {
        current = this;
    }

    //we take an int ID as a parameter into our power up event so each power up is independent
    public event Action<int> onPowerUpAcquire;

    public void PowerUpAcquirement(int id)
    {
        if(onPowerUpAcquire != null)
        {
            onPowerUpAcquire(id);
        }
    }
}
