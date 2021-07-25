using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkMachine : MonoBehaviour
{
    public static bool machineIsPowered;
    private bool inTrigger;
    private bool wantsToBuyPerk;

    private bool playerHasPerk;

    private int perkPrice;


    // Start is called before the first frame update
    void Start()
    {
        machineIsPowered = false;
        wantsToBuyPerk = false;
        playerHasPerk = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (machineIsPowered)
        {
            if (Input.GetKey(KeyCode.F) && inTrigger)
            {
                wantsToBuyPerk = true;
            }
            else if (!inTrigger || Input.GetKey(KeyCode.F) == false)
            {
                wantsToBuyPerk = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inTrigger = true;
            Debug.Log("Entered perk machine trigger");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(wantsToBuyPerk && PlayerScore.pScore >= perkPrice && !playerHasPerk)
            {
                Debug.Log("You have acquired this perk!");
                playerHasPerk = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inTrigger = false;
            Debug.Log("Exited perk machine trigger");
        }
    }
    
}
