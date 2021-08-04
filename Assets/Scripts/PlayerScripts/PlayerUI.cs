using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public string healthText;

    public Text healthElement;

    public Text ammoElement;

    public Text pointElement;

    public Text roundElement;

    public Text zombieCounterElement;

    private GameObject weaponHolder;
    private GunScript gunScriptAccessor;
    private WeaponSwitching weaponSwitchAccessor;

    private float nextActionTime = 0.0f;
    private float period = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        weaponHolder = GameObject.Find("WeaponHolder");
        gunScriptAccessor = weaponHolder.GetComponentInChildren<GunScript>();
        //weaponSwitchAccessor = weaponHolder.GetComponent<WeaponSwitching>();

        healthElement.text = healthText + PlayerHealth.playerHealth;
        ammoElement.text = gunScriptAccessor.current_mag_size + "/" + gunScriptAccessor.ammoCapacity;

    }

    // Update is called once per frame
    void Update()
    {
        //if(weaponSwitchAccessor.previousSelectedWeapon != weaponSwitchAccessor.selectedWeapon)
            //gunScriptAccessor = weaponHolder.GetComponentInChildren<GunScript>();

        //should we update this every frame?
        //UpdatePlayerAmmoUI();

        if(Time.time > nextActionTime)
        {
            //updates player's health and score a few times per second (fast, but not every frame)
            //updates the round and zombie counter
            UpdatePlayerHealthUI();
            UpdatePlayerPointsUI();
            UpdateRoundUI();
            UpdateZombieCounterUI();

            nextActionTime = Time.time + period;
        }

    }
    void UpdatePlayerHealthUI()
    {
        healthElement.text = healthText + PlayerHealth.playerHealth;
        //Debug.Log("Player health UI updated");
    }
    void UpdatePlayerPointsUI()
    {
        pointElement.text = "Points: " + PlayerScore.pScore;
    }
    public void UpdatePlayerAmmoUI()
    {
        ammoElement.text = gunScriptAccessor.current_mag_size + " / " + gunScriptAccessor.ammoCapacity;
    }
    void UpdateRoundUI()
    {
        roundElement.text = "Round: " + RoundController.round;
    }
    void UpdateZombieCounterUI()
    {
        zombieCounterElement.text = "Zombies Left: " + RoundController.zombieCounter;
    }

    public void RetrieveAmmoInfo()
    {
        gunScriptAccessor = weaponHolder.GetComponentInChildren<GunScript>();
    }
}
