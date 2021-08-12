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

    private void Start()
    {
        //canTakeDamage = true;

        perkInventory = GetComponent<PlayerPerkInventory>();

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
            playerHealth = 250f;
        }
    }

    public void SavePlayerLife()
    {
        if (perkInventory.has_Life_Savior_Perk)
        {
            perkInventory.has_Life_Savior_Perk = false;
            playerHealth = originalPlayerHealth;
            
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
