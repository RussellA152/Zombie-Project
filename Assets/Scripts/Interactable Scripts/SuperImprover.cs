using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperImprover : MonoBehaviour
{
    public GameObject weaponHolder;
    private WeaponSwitching weaponSwitchAccessor;

    private int upgradePrice;

    private bool inTrigger;
    private bool wantsToUpgradeGun;

    private GameObject currentlyEquippedWeaponSI;    //SI refers to the super improver version of equipped weapon (not to be confused with the variable in WeaponSwitching)

    // Start is called before the first frame update
    void Start()
    {
        weaponSwitchAccessor = weaponHolder.GetComponent<WeaponSwitching>();

        upgradePrice = 5000;
        inTrigger = false;
        wantsToUpgradeGun = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (inTrigger)
        {
            currentlyEquippedWeaponSI = weaponSwitchAccessor.equippedWeapon.gameObject;
            
        }

        if (inTrigger && Input.GetKey(KeyCode.F))
        {
            wantsToUpgradeGun = true;
        }
        else if (!inTrigger || Input.GetKey(KeyCode.F) == false)
        {
            wantsToUpgradeGun = false;
        }


    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            if (PowerEvent.powerIsTurnedOn)
            {
                //if player enters the trigger of the ammo crate, set inTrigger to true
                inTrigger = true;

                currentlyEquippedWeaponSI = weaponSwitchAccessor.equippedWeapon.gameObject;
            }
            else
            {
                Debug.Log("You must turn on the power!");
            }
            


        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (wantsToUpgradeGun && PlayerScore.pScore >= upgradePrice)
            {
                if (!currentlyEquippedWeaponSI.GetComponent<GunScript>().isUpgraded)
                {
                    //upgrade the currently held weapon
                    PlayerScore.pScore -= upgradePrice;
                    currentlyEquippedWeaponSI.GetComponent<GunScript>().UpgradeGun();
                }   
            }
                
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //when player exits ammo crate's trigger, set inTrigger to false
        if (other.gameObject.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }
}
