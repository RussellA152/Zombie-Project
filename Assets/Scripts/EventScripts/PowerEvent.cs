using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerEvent : MonoBehaviour
{
    private bool wantsToTurnOnPower;
    private bool inTrigger;
    public static bool powerIsTurnedOn;

    public delegate void PowerSwitchedOn();
    public static event PowerSwitchedOn OnPowered;

    [SerializeField] private AudioClip power_on_sound;
    [SerializeField] private GameObject power_lever;


    private void Start()
    {
        wantsToTurnOnPower = false;
        inTrigger = false;
        powerIsTurnedOn = false;
        
    }

    void Update()
    {
        //Checking if the player is inside the Trigger of the Power Switch
        //If they are inside the trigger,check if they're pressing "F" to turn on power

        if (inTrigger && Input.GetKey(KeyCode.F))
        {
            wantsToTurnOnPower = true;
        }
        else if (!inTrigger || Input.GetKey(KeyCode.F) == false)
        {
            wantsToTurnOnPower = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //if player enters Trigger, inTrigger is set to true
        if (other.gameObject.CompareTag("Player"))
        {
            inTrigger = true;
            Debug.Log("Press 'F' to turn on power.");
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        //while the player is inside the trigger, check if wantsToTurnOnPower is true and IF the power is not turned on
        if (other.gameObject.CompareTag("Player"))
        {
            //if power is not turned on, and they want to turn on power, then turn on the power
            if(wantsToTurnOnPower && !powerIsTurnedOn)
            {
                powerIsTurnedOn = true;
                Debug.Log("Power is now on");

                //if powerIsTurnedOn is true, then execute the power event (this should only occur once since powerIsTurnedOn wont ever become false again)
                //Our power event is now executed here
                if (OnPowered != null)
                {
                    Debug.Log("Power Event system is on");
                    InteractAudioSource.current.PlayInteractClip(power_on_sound,0.5f);
                    LeanTween.rotateLocal(power_lever, new Vector3(-120, 0, 0), 1f);
                    OnPowered();
                }
                
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //if player exits trigger, then inTrigger is set to false
        if (other.gameObject.CompareTag("Player"))
        {
            inTrigger = false;
            Debug.Log("Not in power Trigger");
            
        }
    }
}
