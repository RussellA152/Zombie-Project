using System.Collections;
//using System;
//using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAttacks : MonoBehaviour
{
    public float enemyDamage;   //determines how much damage this enemy will do to player
    //private bool canAttack;
    private PlayerHealth playerHealth;
    private Coroutine SetPlayerAttackedCoroutine;

    [SerializeField] private AudioClip[] zombieAttackSounds;
    private AudioSource zombieAudioSource;
    private NavMeshAgent  navmeshAgentAccessor;



    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();

        navmeshAgentAccessor = gameObject.GetComponent<NavMeshAgent>();

        zombieAudioSource = EnemyAudio.current.enemyAudioSourceGameObject.GetComponent<AudioSource>();


        //enemyDamage = 25f; //hard-coded for now, but should be a different value?
        //canAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(zombieAudioSource);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //when player is about to be attacked, the zombie's navmeshagent is stopped as to prevent player from being constantly pushed by the zombie (prevents pushing through walls)
            navmeshAgentAccessor.isStopped = true;
                
            InvokeRepeating("EnemyDealingDamage", .6f, 1f);
            
        }
    }
   
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CancelInvoke();
            //when player exits hitbox (not being attacked anymore), the zombie can start moving again
            navmeshAgentAccessor.isStopped = false;
            SetPlayerAttackedCoroutine = StartCoroutine(SetPlayerAttacked());
            
            //Debug.Log("Attack invoke cancelled");
        }
    }
    private void EnemyDealingDamage()
    {
        playerHealth.is_attacked = true;
        //stop player's health regeneration
        if (SetPlayerAttackedCoroutine != null)
        {
            StopCoroutine(SetPlayerAttackedCoroutine);
            Debug.Log("Stop Health Regen Coroutine!");
        }
        PlayerHealth.playerHealth -= enemyDamage;
        //random chance to play random sound for attack sound
        int randomAudioClip = Random.Range(0, zombieAttackSounds.Length);

        zombieAudioSource.PlayOneShot(zombieAttackSounds[randomAudioClip], 0.5f);

        
        

        //Debug.Log("Attack!");
        //Debug.Log("Your Health: " + PlayerHealth.playerHealth);
    }
    private void OnDisable()
    {
        //This should prevent the zombie from being able to attack player when its dead
        CancelInvoke();
    }
    IEnumerator SetPlayerAttacked()
    {
        Debug.Log("Wait for regeneration");
        yield return new WaitForSeconds(3f);
        playerHealth.is_attacked = false;
        Debug.Log("Can Regen!");
    }
}
