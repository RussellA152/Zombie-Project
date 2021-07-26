using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuy : MonoBehaviour
{

    private bool inTrigger;
    private bool wantsToBuyGun;
    private bool playerPurchasedGun;


    public GameObject gunPrefab;
    private GameObject gunClone;
    public Transform weaponHolderTransform;

    private WeaponSwitching weaponInventoryAccessor;

    public float gunPrice;

    // Start is called before the first frame update
    void Start()
    {
        inTrigger = false;
        wantsToBuyGun = false;
        playerPurchasedGun = false;

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
            //weaponInventoryAccessor = GetComponent<WeaponSwitching>();
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //if player has less the maximum carrying capacity for weapons, then they can buy a gun without replacing 
            if (WeaponSwitching.currentWeaponInventorySize < WeaponSwitching.maxWeaponInventorySize)
            {
                if (wantsToBuyGun && !playerPurchasedGun && PlayerScore.pScore >= gunPrice)
                {
                    //gives player the gun they purchased
                    Debug.Log("You purchased this gun!");

                    //subtract gunPrice from player's score
                    PlayerScore.pScore -= gunPrice;

                    //spawn the purchased gun inside the weaponHolder
                    gunClone = Instantiate(gunPrefab, weaponHolderTransform);

                    //increase weaponInventorySize
                    WeaponSwitching.currentWeaponInventorySize++;


                    //gunClone.SetActive(false);

                    //player should not be able to repurchase the same gun twice, unless they do not have this gun in their inventory
                    playerPurchasedGun = true;
                }
            }
            //IF player has too many weapons, then we should replace their currently equipped weapon
            if (WeaponSwitching.currentWeaponInventorySize >= WeaponSwitching.maxWeaponInventorySize)
            {
                if (wantsToBuyGun && !playerPurchasedGun && PlayerScore.pScore >= gunPrice)
                {
                    //should replace the player's current weapon in-hand
                    Debug.Log("replace current gun in-hand");

                    //remove currently equipped weapon
                    Destroy(WeaponSwitching.equippedWeapon.gameObject);

                    //spawn the purchased gun inside the weaponholder
                    gunClone = Instantiate(gunPrefab, weaponHolderTransform);

                    //increase weaponInventorySize
                    WeaponSwitching.currentWeaponInventorySize++;

                    playerPurchasedGun = true;
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
}
