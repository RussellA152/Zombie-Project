
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;

    private void Start()
    {

    }
    public void TakeDamage(float amount)
    {
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
            Destroy(transform.parent.gameObject);
            
            //killing a zombie will decrement the zombie counter (MIGHT BE A DEPEDENCY ISSUE LATER)
            RoundController.zombieCounter -= 1;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
