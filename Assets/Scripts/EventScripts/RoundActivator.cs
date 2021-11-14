using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundActivator : MonoBehaviour
{
    //private PlayerUI player_ui_accessor;
    //[SerializeField] private GameObject pHud;

    public static bool round_must_increment;
    private void Update()
    {
        RoundActivatorFunction();
    }

    private void Start()
    {
        // pHud = GameObject.Find("Player's HUD");
        // player_ui_accessor = pHud.GetComponent<PlayerUI>();
        round_must_increment = true;
    }
    // when all zombies are dead (zombie counter = 0), start next round
    private void RoundActivatorFunction()
    {
        if(RoundController.zombieCounter == 0 && round_must_increment)
        {

            RoundChange.roundChange.RoundChanging();
            //moves round text to the center of the screen at start of new round
            
            
            
        }
    }
    
    //private void OnTriggerEnter(Collider other)
    //{
       // RoundChange.roundChange.RoundChanging();
    //}
    
}
