using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static float playerHealth;
    [SerializeField] float originalPlayerHealth;

    private float lastCallTime;

    //public bool has_Life_Savior_Perk;
    //public bool has_Health_Increase_Perk;

    private PlayerPerkInventory perkInventory;
    private PlayerMovement playerMovementAccessor;

    private void Start()
    {
        //canTakeDamage = true;

        perkInventory = GetComponent<PlayerPerkInventory>();
        playerMovementAccessor = GetComponent<PlayerMovement>();

        //has_Health_Increase_Perk = false;
        //has_Life_Savior_Perk = false;

        originalPlayerHealth = 150f;

        playerHealth = originalPlayerHealth;
    }
    private void Update()
    {


        if (Time.time - lastCallTime >= 0.2f)
        {
            playerDeath();
        }

    }

    public void IncreaseHealth()
    {
        if (perkInventory.has_Health_Increase_Perk)
        {
            originalPlayerHealth = 250f;
            playerHealth = originalPlayerHealth;
        }
    }

    private void DecreaseHealth()
    {
        originalPlayerHealth = 150f;
        playerHealth = originalPlayerHealth;
    }

    public void SavePlayerLife()
    {
        // these two functions basically reset the player to their original health and speed states before purchasing speed perks
        if (perkInventory.has_Life_Savior_Perk)
        {
            DecreaseHealth();
            if(perkInventory.has_Sprint_Speed_Perk == true)
                playerMovementAccessor.DecreaseSpeed();

            //if player was saved by life savior perk, remove all of their perks so they have to repurchase them
            perkInventory.has_Life_Savior_Perk = false;
            perkInventory.has_Reload_Speed_Perk = false;
            perkInventory.has_Sprint_Speed_Perk = false;
            perkInventory.has_Health_Increase_Perk = false; 
  
        }
        else if (!perkInventory.has_Life_Savior_Perk)
        {
            Debug.Log("You Died!");
        }
    }
    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && canTakeDamage)
        {
            playerTakeDamage();
        }
        
    }
    private void playerTakeDamage()
    {
        playerHealth -= 25f;    //this value SHOULD depend on how ever much damage the particular enemy does, right now its hard coded to 25
        StartCoroutine(DamageTakingCooldown());
    }
    IEnumerator DamageTakingCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }
    */
    private void playerDeath()
    {
        if(playerHealth <= 0f)
        {
            SavePlayerLife();
            lastCallTime = Time.time;
            
        }
    }

}
