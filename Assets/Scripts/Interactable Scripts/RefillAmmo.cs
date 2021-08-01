using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillAmmo : MonoBehaviour
{

    public GameObject GunAccessor;
    GunScript access_gun_script;
    WeaponSwitching access_weaponswitch_script;

    private bool inTrigger;
    private bool wantsToBuyAmmo;
    private bool ammoIsFull;

    // Start is called before the first frame update
    void Start()
    {
        GunAccessor = GameObject.Find("WeaponHolder");
        access_gun_script = GunAccessor.GetComponentInChildren<GunScript>();
        access_weaponswitch_script = GunAccessor.GetComponentInChildren<WeaponSwitching>();

        inTrigger = false;
        wantsToBuyAmmo = false;
        
    }

    private void Update()
    {
        //Debug.Log(PlayerScore.pScore);

        //Checking if player has swapped weapons while inside the ammo crate's trigger, if so, then we must update the getComponent so we can buy ammo for the new gun
        if (inTrigger)
        {
            if(access_weaponswitch_script.previousSelectedWeapon != access_weaponswitch_script.selectedWeapon)
            {
                Debug.Log("get component weapon switch now!");
                access_gun_script = GunAccessor.GetComponentInChildren<GunScript>();
            }
        }


        //checking if the player is inside the trigger, AND if they are pressing the 'f' key,
        //if so, they want to buy ammo
        if (inTrigger && Input.GetKey(KeyCode.F))
        {
            wantsToBuyAmmo = true;
        }
        else if (!inTrigger || Input.GetKey(KeyCode.F) == false)
        {
            wantsToBuyAmmo = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Player"))
        {
            //if player enters the trigger of the ammo crate, set inTrigger to true
            inTrigger = true;

            //accesses GunScript so we can execute the MaxAmmo function (gives player full ammo in clip and capacity)
            access_gun_script = GunAccessor.GetComponentInChildren<GunScript>();

                        
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //we have to check if our ammo is full, if so, then you cannot purchase ammo
        //if your ammo is not full, then you can purchase ammo
        if (access_gun_script.ammoIsFull)
        {
            ammoIsFull = true;
        }
        else
        {
            ammoIsFull = false;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Hold 'f' to open Door [Cost: " + doorPrice);
            if (PlayerScore.pScore >= access_gun_script.ammoPrice && wantsToBuyAmmo && !ammoIsFull)
            {
                //subtracts the specific ammo price of currently equipped gun from the Player's score
                PlayerScore.pScore -= access_gun_script.ammoPrice;
                access_gun_script.MaxAmmo();

                Debug.Log("Ammo Crate - max ammo granted.");

            }
            else
            {
                Debug.Log("You didn't buy ammo.");
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
