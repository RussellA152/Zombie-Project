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

    private WeaponSwitching weaponSwitchingAccessor;

    public Transform weaponHolderTransform;

    private PlayerWeaponInventory currentWeaponsList;

    private GameObject player;

    public float gunPrice;

    // Start is called before the first frame update
    void Start()
    {
        inTrigger = false;
        wantsToBuyGun = false;
        playerHasThisGun = false;

        player = GameObject.Find("Player");
        weaponHolder = GameObject.Find("WeaponHolder");
        weaponHolderTransform = weaponHolder.transform;

        currentWeaponsList = player.GetComponent<PlayerWeaponInventory>();
        weaponSwitchingAccessor = weaponHolder.GetComponent<WeaponSwitching>();

    }

    // Update is called once per frame
    void Update()
    {


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
            if (currentWeaponsList.currentWeaponsList.Contains(gunPrefab) || currentWeaponsList.currentWeaponsList.Contains(gunClone))
            {
                playerHasThisGun = true;
                Debug.Log("You have this gun");
            }
            else
            {
                playerHasThisGun = false;
                Debug.Log("You do not have this gun");
            }
            //weaponInventoryAccessor = GetComponent<WeaponSwitching>();
        }

        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //if player has less the maximum carrying capacity for weapons, then they can buy a gun without replacing 
            if (weaponSwitchingAccessor.currentWeaponInventorySize < weaponSwitchingAccessor.maxWeaponInventorySize)
            {
                if (wantsToBuyGun && !playerHasThisGun && PlayerScore.pScore >= gunPrice)
                {
                    AddGunOnPurchase();
                }
            }
            //IF player has too many weapons, then we should replace their currently equipped weapon
            if (weaponSwitchingAccessor.currentWeaponInventorySize >= weaponSwitchingAccessor.maxWeaponInventorySize)
            {
                if (wantsToBuyGun && !playerHasThisGun && PlayerScore.pScore >= gunPrice)
                {
                    ReplaceGunOnPurchase();
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
        Debug.Log("You purchased this gun!");

        //subtract gunPrice from player's score
        PlayerScore.pScore -= gunPrice;

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

        
        Debug.Log("update weapon inventory 1");
    }

    //If weapon replacement is needed, this function will be called instead of AddGunOnPurchase()
    void ReplaceGunOnPurchase()
    {
        //should replace the player's current weapon in-hand
        Debug.Log("replace current gun in-hand");


        PlayerScore.pScore -= gunPrice;

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

        Debug.Log("update weapon inventory 2");
    }
}
