using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUps : MonoBehaviour
{
    

    //this bool will prevent player from getting power-up more than once
    private bool hasPowerUp;

    public bool hasInstaKill;
    public bool hasDoublePoints;
    public bool hasSuperStrength;

    public bool gotMaxAmmo;

    private float powerUpCountDown;
    public float instaKillCountDown;
    public float doublePointsCountDown;
    public float superStrengthCountDown;

    public int id;

    

    private void Start()
    {
        PowerUpEvent.current.onPowerUpAcquire += GivePowerUp;

        hasPowerUp = false;
    }
    private void Update()
    {
        //Debug.Log(hasSuperStrength);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasPowerUp)
        {
            Debug.Log("Power-up Acquired");
            PowerUpEvent.current.PowerUpAcquirement(id);
            Debug.Log("power up event executed");

        }
    }

    void GivePowerUp(int id)
    {
        hasPowerUp = true;

        int powerUpAbilityChance = Random.Range(1, 5);

        //power up has a insta kill ability
        if(powerUpAbilityChance == 1)
        {
            hasInstaKill = true;
            powerUpCountDown = instaKillCountDown;
            StartCoroutine(PowerupCountDownRoutine());
            Debug.Log("Insta Kill!");
        }
        //power up has a max ammo ability
        else if(powerUpAbilityChance == 2)
        {
            gotMaxAmmo = true;
            Debug.Log("Max Ammo!");
        }
        //power up has double points ability
        else if(powerUpAbilityChance == 3)
        {
            hasDoublePoints = true;
            powerUpCountDown = doublePointsCountDown;
            StartCoroutine(PowerupCountDownRoutine());
            Debug.Log("Double Points!");
        }
        else if(powerUpAbilityChance == 4)
        {
            hasSuperStrength = true;
            powerUpCountDown = superStrengthCountDown;
            StartCoroutine(PowerupCountDownRoutine());
            Debug.Log("Super Strength");
        }


        
    }
    IEnumerator PowerupCountDownRoutine()
    {
        Debug.Log("Power up CountDown Begins");

        yield return new WaitForSeconds(powerUpCountDown);

        if (hasInstaKill)
        {
            hasInstaKill = false;
        }
        else if (hasDoublePoints)
        {
            hasDoublePoints = false;
        } 
        else if (hasSuperStrength)
        {
            hasSuperStrength = false;
        }
        Debug.Log("Power up CountDown Ended");

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        PowerUpEvent.current.onPowerUpAcquire -= GivePowerUp;
    }
}
    