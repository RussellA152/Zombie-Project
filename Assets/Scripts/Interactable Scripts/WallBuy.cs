using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuy : MonoBehaviour
{

    private bool inTrigger;
    private bool wantsToBuyGun;
    private bool playerHasThisGun;


    public GameObject gunPrefab;
    private GameObject gunClone;
    public GameObject weaponHolder;
    public GameObject ammoCrate;

    private GameObject GiveAmmoReference;
    private GiveAmmo GiveAmmoAccessor;

    private WeaponSwitching weaponSwitchingAccessor;

    public Transform weaponHolderTransform;

    private PlayerWeaponInventory currentWeaponsList;

    private RefillAmmo refillAmmoAccessor;
 
    private bool ammoIsFull;


    private GameObject player;

    public float gunPrice;

    private bool canInteract;   //this bool determines if the player will be able to interact/use this wallbuy (so player cannot spam interactions with wall buy, I.E. purchasing ammo really fast and losing lots of money)
    private int purchaseDetermine;  // if this value equals 1, the player purchased a weapon without replacement, if it equals 2, the player replaced a weapon on wall buy purchase 

    [SerializeField] private AudioClip purchase_successful_sound;
    [SerializeField] private AudioClip purchase_failed_sound;

    // Start is called before the first frame update
    void Start()
    {
        canInteract = true;
        inTrigger = false;
        wantsToBuyGun = false;
        playerHasThisGun = false;

        player = GameObject.Find("Player");
        weaponHolder = GameObject.Find("WeaponHolder");
        GiveAmmoReference = GameObject.Find("Ammo Purchaser");


        weaponHolderTransform = weaponHolder.transform;

        //will use an event system to make buying ammo easier
        ammoCrate = GameObject.Find("AmmoBox Trigger");

        currentWeaponsList = player.GetComponent<PlayerWeaponInventory>();
        weaponSwitchingAccessor = weaponHolder.GetComponent<WeaponSwitching>();
        refillAmmoAccessor = ammoCrate.GetComponent<RefillAmmo>();

        GiveAmmoAccessor = GiveAmmoReference.GetComponent<GiveAmmo>();


    }

    // Update is called once per frame
    void Update()
    {
        //Doesn't help with wall buy ammo bug (not detecting gun at times)
        if (inTrigger)
        {
            if (weaponSwitchingAccessor.previousSelectedWeapon != weaponSwitchingAccessor.selectedWeapon)
            {
                GiveAmmoAccessor.AccessGunComponents();
            }
        }

        //Debug.Log(ammoCrate);
        if (inTrigger && Input.GetKey(KeyCode.F))
        {
            wantsToBuyGun = true;
        }
        else if (!inTrigger || Input.GetKey(KeyCode.F) == false)
        {
            wantsToBuyGun = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inTrigger = true;
            //checking if the player currently has the wall buy weapon in their weapon inventory 
            if (currentWeaponsList.currentWeaponsList.Contains(gunPrefab) || currentWeaponsList.currentWeaponsList.Contains(gunClone))
            {
                GiveAmmoAccessor.AccessGunComponents();
                playerHasThisGun = true;
                Debug.Log("You have this gun");
            }
            else
            {
                playerHasThisGun = false;
                Debug.Log("You do not have this gun");
            }
        }

        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (weaponSwitchingAccessor.equippedWeapon.gameObject == gunClone || weaponSwitchingAccessor.equippedWeapon.gameObject == gunPrefab)
            {
                if ((wantsToBuyGun && playerHasThisGun && canInteract))
                {
                    BuyAmmoInsteadOfGun();
                    StartCoroutine(InteractionDelay());


                }

            }

            //if player has less the maximum carrying capacity for weapons, then they can buy a gun without replacing 
            if (weaponSwitchingAccessor.currentWeaponInventorySize < weaponSwitchingAccessor.maxWeaponInventorySize)
            {
                if (wantsToBuyGun && !playerHasThisGun && canInteract)
                {
                    if (PlayerScore.pScore >= gunPrice)
                    {
                        InteractAudioSource.current.PlayInteractClip(purchase_successful_sound, 0.5f);
                        purchaseDetermine = 1;
                        StartCoroutine(WeaponSwapOnPurchaseDelay());

                        StartCoroutine(InteractionDelay());
                    }
                    else
                    {
                        InteractAudioSource.current.PlayInteractClip(purchase_failed_sound, 0.5f);
                        StartCoroutine(InteractionDelay());
                    }

                }
            }
            //IF player has too many weapons, then we should replace their currently equipped weapon
            if (weaponSwitchingAccessor.currentWeaponInventorySize >= weaponSwitchingAccessor.maxWeaponInventorySize)
            {
                if (wantsToBuyGun && !playerHasThisGun && canInteract)
                {
                    if (PlayerScore.pScore >= gunPrice)
                    {
                        InteractAudioSource.current.PlayInteractClip(purchase_successful_sound, 0.5f);
                        purchaseDetermine = 2;
                        StartCoroutine(WeaponSwapOnPurchaseDelay());

                        StartCoroutine(InteractionDelay());
                    }
                    else
                    {
                        InteractAudioSource.current.PlayInteractClip(purchase_failed_sound, 0.5f);
                        StartCoroutine(InteractionDelay());
                    }
                }
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }

    //Function for purchasing a gun without replacing
    void AddGunOnPurchase()
    {
        //gives player the gun they purchased
        //Debug.Log("You purchased this gun!");

        //subtract gunPrice from player's score


        weaponHolder.GetComponentInChildren<GunScript>().animator.SetBool("Reloading", false);

        //spawn the purchased gun inside the weaponHolder
        gunClone = Instantiate(gunPrefab, weaponHolderTransform);
        
        currentWeaponsList.currentWeaponsList.Add(gunClone);



        //increase weaponInventorySize
        weaponSwitchingAccessor.currentWeaponInventorySize++;

        //A TEMPORARY SOLUTION: prevents player from having two guns equipped at once
        weaponSwitchingAccessor.selectedWeapon++;
        weaponSwitchingAccessor.SelectWeapon();

        //gunClone.SetActive(false);

        //player should not be able to repurchase the same gun twice, unless they do not have this gun in their inventory
        playerHasThisGun = true;

        
        //Debug.Log("update weapon inventory 1");
        GiveAmmoAccessor.AccessGunComponents();
    }

    //If weapon replacement is needed, this function will be called instead of AddGunOnPurchase()
    void ReplaceGunOnPurchase()
    {
        //should replace the player's current weapon in-hand
        //Debug.Log("replace current gun in-hand");


        weaponHolder.GetComponentInChildren<GunScript>().animator.SetBool("Reloading", false);

        //remove currently equipped weapon
        currentWeaponsList.currentWeaponsList.Remove(weaponSwitchingAccessor.equippedWeapon.gameObject);
        Destroy(weaponSwitchingAccessor.equippedWeapon.gameObject);


        //spawn the purchased gun inside the weaponholder
        gunClone = Instantiate(gunPrefab, weaponHolderTransform);

        //checks index position of gun so we can have the same position of gun position when we buy a new gun (prevents us from always having new gun be in the hotkey 2 position)
        switch (weaponSwitchingAccessor.selectedWeapon)
        {
            case 0:
                gunClone.transform.SetSiblingIndex(0);
                weaponSwitchingAccessor.SelectWeapon();
                break;
            case 1:
                gunClone.transform.SetSiblingIndex(1);
                weaponSwitchingAccessor.SelectWeapon();
                break;
        }

        
        
        //updates player's weapon inventory since we just acquired a new gun and removed one
        currentWeaponsList.currentWeaponsList.Add(gunClone);

        
        

        playerHasThisGun = true;

        //Debug.Log("update weapon inventory 2");
        GiveAmmoAccessor.AccessGunComponents();
    }
    void BuyAmmoInsteadOfGun()
    {
        
        Debug.Log("Buy Ammo! instead of buying gun");
        
        RefillPlayerAmmo.current.AmmoRefiller();
    }
    IEnumerator InteractionDelay()
    {
        canInteract = false;
        yield return new WaitForSeconds(1f);
        canInteract = true;
    }

    //This coroutine plays the weapon swapping animation when purchasing a wall buy (gives impression of exchanging weapons)
    //We play the animation, then wait a little bit, then call the weapon purchasing functions, then stop the weapon swap animation
    IEnumerator WeaponSwapOnPurchaseDelay()
    {
        //normally we would call this in the AddGunOnPurchase or ReplaceGunOnPurchase functions, but we will call this here instead because the swap delay will make the transaction take longer
        PlayerScore.pScore -= gunPrice;

        weaponSwitchingAccessor.animator.SetBool("Swapping", true);
        weaponHolder.GetComponentInChildren<GunScript>().isSwapping = true;
        yield return new WaitForSeconds(1f - .25f);

        if(purchaseDetermine == 1)
        {
           AddGunOnPurchase();
        }
        else if( purchaseDetermine == 2)
        {
           ReplaceGunOnPurchase();
        }

        weaponSwitchingAccessor.animator.SetBool("Swapping", false);
        yield return new WaitForSeconds(.25f);
        weaponHolder.GetComponentInChildren<GunScript>().isSwapping = false;
    }
}
