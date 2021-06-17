
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    public float health = 50f;
    

    private void Start()
    {

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
            Debug.Log("HIIII");
            health += 10f;

            if(gameObject.GetComponent<NavMeshAgent>() != null)
            {
                gameObject.GetComponent<NavMeshAgent>().speed += .5f;
            }
        }
        
    }
   
    void Die()
    {
        //destroys the zombie and its AI (everything)
        if(gameObject.CompareTag("Enemy"))
        {
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
