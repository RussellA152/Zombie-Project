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

    [SerializeField] private AudioClip purchase_successful_sound;
    [SerializeField] private AudioClip weapon_upgrade_sound;
    [SerializeField] private AudioClip purchase_failed_sound;

    private bool canInteract;

    // Start is called before the first frame update
    void Start()
    {
        weaponSwitchAccessor = weaponHolder.GetComponent<WeaponSwitching>();

        upgradePrice = 5000;
        inTrigger = false;
        wantsToUpgradeGun = false;
        canInteract = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (inTrigger)
        {
            currentlyEquippedWeaponSI = weaponSwitchAccessor.equippedWeapon.gameObject;
            CheckTextBox();
            
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
                CheckTextBox();

                
            }
            else
            {
                InteractionTextbox.current.ChangeTextBoxDescription("The power must be turned on!");
            }
            


        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (wantsToUpgradeGun && canInteract)
            {
                if (!currentlyEquippedWeaponSI.GetComponent<GunScript>().isUpgraded && PlayerScore.pScore >= upgradePrice)
                {
                    //upgrade the currently held weapon
                    canInteract = false;
                    InteractAudioSource.current.PlayInteractClip(purchase_successful_sound, 0.5f);
                    InteractAudioSource.current.PlayInteractClip(weapon_upgrade_sound, 0.5f);
                    PlayerScore.pScore -= upgradePrice;
                    currentlyEquippedWeaponSI.GetComponent<GunScript>().UpgradeGun();
                    StartCoroutine(InteractionDelay());
                }
                else
                {
                    canInteract = false;
                    InteractAudioSource.current.PlayInteractClip(purchase_failed_sound, 0.5f);
                    StartCoroutine(InteractionDelay());
                    
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
            InteractionTextbox.current.CloseTextBox();
        }
    }
    IEnumerator InteractionDelay()
    {
        yield return new WaitForSeconds(1f);
        canInteract = true;
    }

    private void CheckTextBox()
    {
        if (!currentlyEquippedWeaponSI.GetComponent<GunScript>().isUpgraded)
            InteractionTextbox.current.ChangeTextBoxDescription("Press 'F' to upgrade " + currentlyEquippedWeaponSI.GetComponent<GunScript>().name + " (" + upgradePrice + ")");
        else
            InteractionTextbox.current.ChangeTextBoxDescription("Weapon is already upgraded!");
    }
}
