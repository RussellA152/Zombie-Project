using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    public float enemyDamage;   //determines how much damage this enemy will do to player
    //private bool canAttack;

    // Start is called before the first frame update
    void Start()
    {
        enemyDamage = 25f; //hard-coded for now, but should be a different value?
        //canAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InvokeRepeating("EnemyDealingDamage", .6f, 2f);
        }
    }
   
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CancelInvoke();
            Debug.Log("Attack invoke cancelled");
        }
    }
    private void EnemyDealingDamage()
    {
        PlayerHealth.playerHealth -= enemyDamage;
        Debug.Log("Attack!");
        //Debug.Log("Your Health: " + PlayerHealth.playerHealth);
    }
    private void OnDisable()
    {
        //This should prevent the zombie from being able to attack player when its dead
        CancelInvoke();
    }
}
