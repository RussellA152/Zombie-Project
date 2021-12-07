using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillAmmo : MonoBehaviour
{
    public GameObject GiveAmmoReference;
    private GiveAmmo GiveAmmoAccessor;

    public GameObject GunAccessor;
    [SerializeField] private GameObject ammoCrateLid;
    GunScript access_gun_script;
    WeaponSwitching access_weaponswitch_script;

    private bool inTrigger;
    private bool wantsToBuyAmmo;
    private bool ammoIsFull;

    private bool canInteract;   //this boolean value will prevent player from spamming purchasing ammo from Ammo Crate
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        canInteract = true;
        GunAccessor = GameObject.Find("WeaponHolder");
        GiveAmmoReference = GameObject.Find("Ammo Purchaser");
        //access_gun_script = GunAccessor.GetComponentInChildren<GunScript>();
        access_weaponswitch_script = GunAccessor.GetComponentInChildren<WeaponSwitching>();
        GiveAmmoAccessor = GiveAmmoReference.GetComponent<GiveAmmo>();

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
                GiveAmmoAccessor.AccessGunComponents();
                InteractionTextbox.current.ChangeTextBoxDescription("Press 'F' to purchase ammo " + "($" + GiveAmmoAccessor.ReturnAmmoPrice() + ")");
                //access_gun_script = GunAccessor.GetComponentInChildren<GunScript>();
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
            InteractionTextbox.current.ChangeTextBoxDescription("Press 'F' to purchase ammo "+"($" + GiveAmmoAccessor.ReturnAmmoPrice() +")");
            //accesses GunScript so we can execute the MaxAmmo function (gives player full ammo in clip and capacity)
            //access_gun_script = GunAccessor.GetComponentInChildren<GunScript>();
            GiveAmmoAccessor.AccessGunComponents();


        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(wantsToBuyAmmo && canInteract)
            {
                PurchaseAmmo();
                

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

    public void PurchaseAmmo()
    {

        RefillPlayerAmmo.current.AmmoRefiller();

        //opens the top of the ammo crate (animation)
        TweenCrate();
        StartCoroutine(InteractionDelay());


    }
    public void TweenCrate()
    {
        LeanTween.rotateZ(ammoCrateLid, -125, .6f);
    }
    IEnumerator InteractionDelay()
    {
        canInteract = false;
        yield return new WaitForSeconds(1f);
        LeanTween.rotateZ(ammoCrateLid, -180, .5f);
        canInteract = true;
    }
        
}
