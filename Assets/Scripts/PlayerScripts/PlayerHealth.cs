using EZCameraShake;
using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public static float playerHealth;
    [SerializeField] float originalPlayerHealth;

    private float lastCallTime;

    //public bool has_Life_Savior_Perk;
    //public bool has_Health_Increase_Perk;

    private PlayerPerkInventory perkInventory;
    private PlayerMovement playerMovementAccessor;
    //public Coroutine RegenerateHealthCoroutine;

    public static bool is_attacked;
    private bool is_dead;
    private bool checked_for_death;
    public bool is_invincible;

    [SerializeField] private GameObject roundControllerObject;
    private RoundController roundControllerScriptAccessor;

    [SerializeField] private AudioClip life_save_sound;

    public float start_regen_timer;
    public float elapsed_regen_timer;

    private void Start()
    {

        perkInventory = GetComponent<PlayerPerkInventory>();
        playerMovementAccessor = GetComponent<PlayerMovement>();
        roundControllerScriptAccessor = roundControllerObject.GetComponent<RoundController>();


        //RegenerateHealthCoroutine = StartCoroutine(RegeneratePlayerHealth());

        //has_Health_Increase_Perk = false;
        //has_Life_Savior_Perk = false;

        originalPlayerHealth = 150f;

        playerHealth = originalPlayerHealth;

        is_dead = false;
        is_invincible = false;
    }
    private void Update()
    {
        elapsed_regen_timer = Time.time - start_regen_timer;

        if(elapsed_regen_timer >= 3f)
        {
            is_attacked = false;
        }
        
        if ((playerHealth < originalPlayerHealth) && !is_dead)
        {
            RegenerateHealth();
        }

        //we only want to check this, if the player is not dead because we only want to load to the death screen once
        if(!is_dead)
            playerDeath();

        if (is_dead && !checked_for_death)
        {
            roundControllerScriptAccessor.StopZombieSpawning();
            checked_for_death = true;
            Debug.Log("unsubscribed zombie spawns");
        }

    }

    public void IncreaseHealth()
    {
        if (perkInventory.has_Health_Increase_Perk)
        {
            originalPlayerHealth = 250f;
            //playerHealth = originalPlayerHealth;
        }
    }

    private void DecreaseHealth()
    {
        originalPlayerHealth = 150f;
        //playerHealth = originalPlayerHealth;
    }

    public void SavePlayerLife()
    {
        // these two functions basically reset the player to their original health and speed states before purchasing speed perks

        //starts coroutine for invincibilty timer (player cannot take damage for a few seconds)
        StartCoroutine(InvincibleTimer());
        playerHealth = 1f;
        InteractAudioSource.current.PlayInteractClip(life_save_sound, 0.5f);
        DecreaseHealth();
        if(perkInventory.has_Sprint_Speed_Perk == true)
            playerMovementAccessor.DecreaseSpeed();

            //if player was saved by life savior perk, remove all of their perks so they have to repurchase them
        perkInventory.has_Life_Savior_Perk = false;
        perkInventory.has_Reload_Speed_Perk = false;
        perkInventory.has_Sprint_Speed_Perk = false;
        perkInventory.has_Health_Increase_Perk = false; 
    }
    public void PlayerHasDied()
    {
        is_dead = true;
        MySceneHandler.current.ChangeScene("Death Screen");

        Debug.Log("You Died!");
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
            if (perkInventory.has_Life_Savior_Perk)
            {
                SavePlayerLife();
            }
            else if (!perkInventory.has_Life_Savior_Perk)
            {
                PlayerHasDied();
            }
            //lastCallTime = Time.time;
            
        }
    }

    private void RegenerateHealth()
    {
        if (!is_attacked)
        {
            playerHealth += 25f * Time.deltaTime;
            //Debug.Log("Regen health!");
        }
        else if(is_attacked)
        {
            return;
        }

        
    }
    public void ShakeCameraOnHit()
    {
        //when player is attacked, we call this function from the enemyAttacks script
        //allows player to see if they are hit in a visual aspect
        CameraShaker.Instance.ShakeOnce(10f, 5f, 0.1f, 0.5f);
    }
    IEnumerator InvincibleTimer()
    {
        is_invincible = true;
        yield return new WaitForSeconds(5f);
        is_invincible = false; 
    }

    /*
    public IEnumerator RegeneratePlayerHealth()
    {
        yield return new WaitForSeconds(1f);

        if (playerHealth < originalPlayerHealth)
        {
            playerHealth += .9f * Time.deltaTime;

        }
        else
        {
            yield return null;
        }
    }
    */

}
