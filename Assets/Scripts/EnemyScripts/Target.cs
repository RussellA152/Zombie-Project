
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    public float health = 50f;

    public static float minSpeed = .8f;
    public static float maxSpeed = 1.1f;

    public static float difficultySpeedIncrease = 0f;
     

    private void Start()
    {
        //gameObject.GetComponent<NavMeshAgent>().speed = Random.Range(minSpeed, maxSpeed);
    }
    public void TakeDamage(float amount)
    {
        //amount is passed as an argument inside the GunScript.cs
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    public void RoundDifficultyIncrease()
    {
        //AS OF NOW, every 2 rounds, zombie health and movement speed increases
        if(RoundController.round % 2 == 0 && RoundController.round != 0)
        {
            Debug.Log("Round Difficulty Increased");
            health += 10f;
            difficultySpeedIncrease += .2f;
                
            
        }
        
    }
   
    void Die()
    {
        //destroys the zombie and its AI (everything)
        if(gameObject.CompareTag("Enemy"))
        {
            PlayerScore.pScore += 100f;
            Destroy(gameObject);
            
            //killing a zombie will decrement the zombie counter (MIGHT BE A DEPEDENCY ISSUE LATER)
            RoundController.zombieCounter -= 1;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
