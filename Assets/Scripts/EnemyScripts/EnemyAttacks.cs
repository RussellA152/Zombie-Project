using System.Collections;
using System;
using System.Collections.Generic;
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



    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();

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
        //random chance to play random sound for attack sound
        int randomAudioClip = Random.Range(0, zombieAttackSounds.Length);

        PlayerHealth.playerHealth -= enemyDamage;
        playerHealth.is_attacked = true;

        zombieAudioSource.PlayOneShot(zombieAttackSounds[randomAudioClip], 0.5f);
        
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
