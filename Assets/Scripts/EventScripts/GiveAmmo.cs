using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveAmmo : MonoBehaviour
{

    public GameObject GunAccessor;
    GunScript access_gun_script;
    WeaponSwitching access_weaponswitch_script;

    private bool ammoIsFull;

    // Start is called before the first frame update
    void Start()
    {
        RefillPlayerAmmo.current.RefillAmmo += GivePlayerFullAmmoForOneGun;

        GunAccessor = GameObject.Find("WeaponHolder");
        access_gun_script = GunAccessor.GetComponentInChildren<GunScript>();
        access_weaponswitch_script = GunAccessor.GetComponentInChildren<WeaponSwitching>();

        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(access_gun_script.ammoIsFull);
    }
    void GivePlayerFullAmmoForOneGun()
    {
        access_gun_script = GunAccessor.GetComponentInChildren<GunScript>();
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

        //Debug.Log("Hold 'f' to open Door [Cost: " + doorPrice);
        if (PlayerScore.pScore >= access_gun_script.ammoPrice && !ammoIsFull)
        {
            //subtracts the specific ammo price of currently equipped gun from the Player's score
            
            PlayerScore.pScore -= access_gun_script.ammoPrice;
            access_gun_script.MaxAmmo();


            Debug.Log("Ammo Crate - max ammo granted.");

        }
    }

    public void AccessGunComponents()
    {
        access_gun_script = GunAccessor.GetComponentInChildren<GunScript>();
        Debug.Log("access gun components");
        
    }
    private void OnDisable()
    {
        RefillPlayerAmmo.current.RefillAmmo -= GivePlayerFullAmmoForOneGun;
    }
}
