using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth.playerHealth = 0;
            other.gameObject.GetComponent<PlayerHealth>().PlayerHasDied();

        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Target>().Die();
        }
    }
}
