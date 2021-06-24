using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth;
    private bool canTakeDamage;
    private bool wasAttacked;
    public float damageCooldown = 2f;


    private void Start()
    {
        canTakeDamage = true;
        playerHealth = 150f;
    }
    private void Update()
    {
        //Debug.Log(playerHealth);
        playerDeath();
    }
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
    private void playerDeath()
    {
        if(playerHealth <= 0f)
        {
            Debug.Log("You Died");
        }
    }

}
