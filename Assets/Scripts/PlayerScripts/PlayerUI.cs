using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public string healthText;

    public Text healthElement;

    public Text ammoElement;

    public Text pointElement;

    public Text roundElement;

    public Text zombieCounterElement;

    private float nextActionTime = 0.0f;
    private float period = 0.3f;

    // Start is called before the first frame update
    void Start()
    {

        healthElement.text = healthText + PlayerHealth.playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextActionTime)
        {
            //updates player's health and score a few times per second (fast, but not every frame)
            //updates the round and zombie counter
            UpdatePlayerHealthUI();
            UpdatePlayerPointsUI();
            UpdateRoundUI();
            UpdateZombieCounterUI();

            nextActionTime = Time.time + period;
        }

    }
    void UpdatePlayerHealthUI()
    {
        healthElement.text = healthText + PlayerHealth.playerHealth;
        //Debug.Log("Player health UI updated");
    }
    void UpdatePlayerPointsUI()
    {
        pointElement.text = "Points: " + PlayerScore.pScore;
    }
    void UpdatePlayerAmmoUI()
    {
        //ammoElement.text = 
    }
    void UpdateRoundUI()
    {
        roundElement.text = "Round: " + RoundController.round;
    }
    void UpdateZombieCounterUI()
    {
        zombieCounterElement.text = "Zombies Left: " + RoundController.zombieCounter;
    }
}
