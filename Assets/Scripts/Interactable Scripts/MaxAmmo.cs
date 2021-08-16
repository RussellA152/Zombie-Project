using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxAmmo : MonoBehaviour
{
    public bool gotMaxAmmo;
    public int id;

    [SerializeField] private float deletionCountDown;
    private bool deletionCountDownHasStarted;

    PlayerWeaponInventory currentWeaponsList;

    private GameObject player;

    private Coroutine deletionCountDownCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("Player");

        currentWeaponsList = player.GetComponent<PlayerWeaponInventory>();

        gotMaxAmmo = false;

        deletionCountDownCoroutine = StartCoroutine(DeletePowerUpTimer());
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
    //makes max ammo power up delete after some time
    IEnumerator DeletePowerUpTimer()
    {
        yield return new WaitForSeconds(deletionCountDown);
        Destroy(gameObject);

    }
    private void OnDestroy()
    {
        //not sure if we need to stop coroutine since it gets destroyed but this is here just in case
        StopCoroutine(deletionCountDownCoroutine);
        PowerUpEvent.current.onPowerUpAcquire -= GiveMaxAmmo;
    }
}