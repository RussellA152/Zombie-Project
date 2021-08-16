using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    public float enemyDamage;   //determines how much damage this enemy will do to player
    //private bool canAttack;
    private PlayerHealth playerHealth;
    private Coroutine SetPlayerAttackedCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        

        //enemyDamage = 25f; //hard-coded for now, but should be a different value?
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
            if (SetPlayerAttackedCoroutine != null)
            {
                StopCoroutine(SetPlayerAttackedCoroutine);
                Debug.Log("Stop Coroutine!");
            }
                
            InvokeRepeating("EnemyDealingDamage", .6f, 1f);
            
        }
    }
   
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CancelInvoke();
            SetPlayerAttackedCoroutine = StartCoroutine(SetPlayerAttacked());
            Debug.Log("Attack invoke cancelled");
        }
    }
    private void EnemyDealingDamage()
    {

        PlayerHealth.playerHealth -= enemyDamage;
        playerHealth.is_attacked = true;
        
        Debug.Log("Attack!");
        //Debug.Log("Your Health: " + PlayerHealth.playerHealth);
    }
    private void OnDisable()
    {
        //This should prevent the zombie from being able to attack player when its dead
        CancelInvoke();
    }
    IEnumerator SetPlayerAttacked()
    {
        Debug.Log("Attack Coroutine!");
        yield return new WaitForSeconds(3f);
        playerHealth.is_attacked = false;
    }
}
