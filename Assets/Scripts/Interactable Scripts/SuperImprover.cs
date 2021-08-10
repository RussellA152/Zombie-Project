using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperImprover : MonoBehaviour
{
    public GameObject weaponHolder;
    private GunScript gunScriptAccessor;
    private WeaponSwitching weaponSwitchAccessor;

    private int upgradePrice;

    private bool inTrigger;
    private bool wantsToUpgradeGun;

    // Start is called before the first frame update
    void Start()
    {
        gunScriptAccessor = weaponHolder.GetComponentInChildren<GunScript>();
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
            if (weaponSwitchAccessor.previousSelectedWeapon != weaponSwitchAccessor.selectedWeapon)
            {
                //if player swaps weapons while in trigger, get component again so we can check for new gun details
                gunScriptAccessor = weaponHolder.GetComponentInChildren<GunScript>();
                
            }
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

                gunScriptAccessor = weaponHolder.GetComponentInChildren<GunScript>();
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
            if (wantsToUpgradeGun && !gunScriptAccessor.isUpgraded && PlayerScore.pScore >= upgradePrice)
            {
                //upgrade the currently held weapon
                PlayerScore.pScore -= upgradePrice;
                gunScriptAccessor.UpgradeGun();
                
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
