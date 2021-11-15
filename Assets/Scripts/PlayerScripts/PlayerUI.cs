using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI zombieCounterText;

    [SerializeField] private GameObject roundTextDisplay;

    [SerializeField] private GameObject crosshair;


    //public string healthText;

    //public Text healthElement;

    //public Text ammoElement;

    //public Text pointElement;

    //public Text roundElement;

    //public Text zombieCounterElement;

    private GameObject weaponHolder;
    private GunScript gunScriptAccessor;
    private WeaponSwitching weaponSwitchAccessor;

    private float nextActionTime = 0.0f;
    private float period = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        weaponHolder = GameObject.Find("WeaponHolder");
        gunScriptAccessor = weaponHolder.GetComponentInChildren<GunScript>();
        //weaponSwitchAccessor = weaponHolder.GetComponent<WeaponSwitching>();

        //healthElement.text = "Health: " + PlayerHealth.playerHealth;
        UpdatePlayerAmmoUI();

    }

    // Update is called once per frame
    void Update()
    {
        //if(weaponSwitchAccessor.previousSelectedWeapon != weaponSwitchAccessor.selectedWeapon)
        //gunScriptAccessor = weaponHolder.GetComponentInChildren<GunScript>();

        //should we update this every frame?
        //UpdatePlayerAmmoUI();

        UpdatePlayerHealthUI();
        if (Time.time > nextActionTime)
        {
            //updates player's health and score a few times per second (fast, but not every frame)
            //updates the round and zombie counter
            
            UpdatePlayerPointsUI();
            UpdateRoundUI();
            UpdateZombieCounterUI();

            nextActionTime = Time.time + period;
        }

    }
    void UpdatePlayerHealthUI()
    {
        healthText.text = "Health: " + Mathf.Round(PlayerHealth.playerHealth);
        //healthElement.text = healthText + PlayerHealth.playerHealth;
        //Debug.Log("Player health UI updated");
    }
    void UpdatePlayerPointsUI()
    {
        scoreText.text = "$ " + Mathf.Round(PlayerScore.pScore);
        //pointElement.text = "Points: " + PlayerScore.pScore;
    }
    public void UpdatePlayerAmmoUI()
    {

        ammoText.text = gunScriptAccessor.current_mag_size + " | " + gunScriptAccessor.ammoCapacity;
        //ammoElement.text = gunScriptAccessor.current_mag_size + " / " + gunScriptAccessor.ammoCapacity;
    }
    void UpdateRoundUI()
    {
        roundText.text = "" + RoundController.round;
        //roundElement.text = "Round: " + RoundController.round;
    }
    void UpdateZombieCounterUI()
    {
        zombieCounterText.text = "Zombies Left: " + RoundController.zombieCounter;
       // zombieCounterElement.text = "Zombies Left: " + RoundController.zombieCounter;
    }

    public void RetrieveAmmoInfo()
    {
        gunScriptAccessor = weaponHolder.GetComponentInChildren<GunScript>();
    }

    public void RoundChangeUIAnimation()
    {
        //moves round text to the center of the screen
        roundText.rectTransform.LeanMoveLocal(crosshair.transform.localPosition,3.5f);
        StartCoroutine(RoundChangeBack());
    }
    IEnumerator RoundChangeBack()
    {
        yield return new WaitForSeconds(6.5f);

        roundText.rectTransform.LeanMove(new Vector3(0f, -25f, 0f), 1.5f);

    }
}
