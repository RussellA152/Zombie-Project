using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Power-up Acquired");
            GivePowerUp();
        }
    }

    void GivePowerUp()
    {
        int powerUpAbilityChance = Random.Range(1, 4);

        //power up has a insta kill ability
        if(powerUpAbilityChance == 1)
        {
            Debug.Log("Insta Kill!");
        }
        //power up has a max ammo ability
        else if(powerUpAbilityChance == 2)
        {
            Debug.Log("Max Ammo!");
        }
        //power up has double points ability
        else if(powerUpAbilityChance == 3)
        {
            Debug.Log("Double Points!");
        }


        Destroy(gameObject, 0.5f);
    }
}
