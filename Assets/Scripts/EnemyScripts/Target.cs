
using UnityEngine;

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
