using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundActivator : MonoBehaviour
{
    private void Update()
    {
        RoundActivatorFunction();
    }
    // when all zombies are dead (zombie counter = 0), start next round
    private void RoundActivatorFunction()
    {
        if(RoundController.zombieCounter == 0)
        {
            RoundChange.roundChange.RoundChanging();
        }
    }
    
    //private void OnTriggerEnter(Collider other)
    //{
       // RoundChange.roundChange.RoundChanging();
    //}
    
}
