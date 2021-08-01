using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEvent : MonoBehaviour
{
    public static PowerUpEvent current;
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
