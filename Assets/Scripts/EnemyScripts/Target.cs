using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    public float health = 50f;

    public static float minSpeed = .8f;
    public static float maxSpeed = 1.1f;

    public static float difficultySpeedIncrease = 0f;

    private NavMeshAgent navMesh;
    private Rigidbody target_rigidbody;
    private EnemyAttacks enemyAttacks_access;
     

    private void Start()
    {
        //gameObject.GetComponent<NavMeshAgent>().speed = Random.Range(minSpeed, maxSpeed);
        navMesh = gameObject.GetComponent<NavMeshAgent>();
        target_rigidbody = GetComponent<Rigidbody>();
        enemyAttacks_access = GetComponent<EnemyAttacks>();
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
            //when enemt dies, give player score
            PlayerScore.pScore += 100f;

            enemyAttacks_access.enabled = false;

            //turn off zombie's navmesh, then turn off its constraints to make "ragdoll" effects
            navMesh.enabled = false;
            target_rigidbody.constraints = RigidbodyConstraints.None;

            //after a few seconds, destroy zombie's body
            Destroy(gameObject, 2.5f);
            
            //killing a zombie will decrement the zombie counter (MIGHT BE A DEPEDENCY ISSUE LATER)
            RoundController.zombieCounter -= 1;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //IEnumerator DeathAnimationDelay()
    //{
        //yield return new WaitForSeconds(2.5f);
        //Destroy(gameObject);
    //}
}
