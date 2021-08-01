using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxAmmo : MonoBehaviour
{
    public bool gotMaxAmmo;
    public int id;

    public float deletionCountDown;
    private bool deletionCountDownHasStarted;

    PlayerWeaponInventory currentWeaponsList;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("Player");

        currentWeaponsList = player.GetComponent<PlayerWeaponInventory>();

        gotMaxAmmo = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !gotMaxAmmo)
        {
            //we subscribe when we triggerEnter instead of at Start() so we don't activate duplicates at the same time
            PowerUpEvent.current.onPowerUpAcquire += GiveMaxAmmo;
            PowerUpEvent.current.PowerUpAcquirement(id);
            Debug.Log("Max ammo power up event executed");

        }
    }
    void GiveMaxAmmo(int id)
    {
        if (id == this.id && this.gameObject)
        {
            MaxAllGunAmmo();
            gotMaxAmmo = true;
            Destroy(gameObject);
        }
    }

    void MaxAllGunAmmo()
    {
        
        //loops through the player's weapon inventory and gives max ammo to all of them
        foreach (GameObject gun in currentWeaponsList.currentWeaponsList)
        {
            GunScript gun_access = gun.GetComponent<GunScript>();
            gun_access.MaxAmmo();
            //Debug.Log("Give MAX AMMO!!");

        }
    }
    private void OnDestroy()
    {
        PowerUpEvent.current.onPowerUpAcquire -= GiveMaxAmmo;
    }
}