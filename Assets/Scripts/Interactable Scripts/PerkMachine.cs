using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkMachine : MonoBehaviour
{
    public static PerkMachine current;

    public static bool machineIsPowered;
    private bool inTrigger;
    private bool wantsToBuyPerk;

    private bool playerHasPerk;
    private bool canInteract;

    [SerializeField] int perkPrice;

    private int perkType;

    [Header("Perk Types")]
    public bool is_Life_Savior_Perk;
    public bool is_Reload_Speed_Perk;
    public bool is_Sprint_Speed_Perk;
    public bool is_Health_Increase_Perk;

    [Header("Player Owned Perks")]
    public bool purchased_Life_Savior_Perk;
    public bool purchased_Reload_Speed_Perk;
    public bool purchased_Sprint_Speed_Perk;
    public bool purchased_Health_Increase_Perk;

    [Header("GameObject Accessors")]
    public GameObject player;
    [SerializeField] private ParticleSystem player_first_power_particle_effect;
    [SerializeField] private ParticleSystem player_second_power_particle_effect;
    //[SerializeField] private ParticleSystem player_second_power_particle_effect;
    //public GameObject weaponHolder;

    private PlayerHealth playerHealthAccessor;
    private PlayerMovement playerMovementAccessor;
    private PlayerPerkInventory playerPerkInventory;

    [SerializeField] private AudioClip soda_open_sound;
    [SerializeField] private AudioClip soda_buff_sound;

    [SerializeField] private AudioClip purchase_successful_sound;
    [SerializeField] private AudioClip purchase_failed_sound;

    [SerializeField] private string perk_name;
    [SerializeField] private string perk_description;



    private void Awake()
    {
        current = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerHealthAccessor = player.GetComponent<PlayerHealth>();
        playerMovementAccessor = player.GetComponent<PlayerMovement>();
        playerPerkInventory = player.GetComponent<PlayerPerkInventory>();


        canInteract = true;
        machineIsPowered = false;
        wantsToBuyPerk = false;
        playerHasPerk = false;

        CheckPerkType();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("PerkType " + perkType);
        if (machineIsPowered)
        {
            if (Input.GetKey(KeyCode.F) && inTrigger)
            {
                wantsToBuyPerk = true;
            }
            else if (!inTrigger || Input.GetKey(KeyCode.F) == false)
            {
                wantsToBuyPerk = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!playerHasPerk && machineIsPowered)
                InteractionTextbox.current.ChangeTextBoxDescription("Press 'F' to purchase: " + perk_name + "(" + perk_description + ") " + "($" + perkPrice + ")");
            else if (playerHasPerk && machineIsPowered)
                InteractionTextbox.current.ChangeTextBoxDescription("You already have this perk.");
            else if (!machineIsPowered)
                InteractionTextbox.current.ChangeTextBoxDescription("The power must be turned on!");

            inTrigger = true;
            Debug.Log("Entered perk machine trigger");

            //we check if player had ever purchased the life savior perk and if they had lost it (died, and thus losing the life savior perk) and is now trying to repurchase the perk
            if(playerHasPerk && purchased_Life_Savior_Perk == true && !playerPerkInventory.has_Life_Savior_Perk)
            {
                purchased_Life_Savior_Perk = false;
                playerHasPerk = false;
            }
            if (playerHasPerk && purchased_Health_Increase_Perk == true && !playerPerkInventory.has_Health_Increase_Perk)
            {
                purchased_Health_Increase_Perk = false;
                playerHasPerk = false;
            }
            if(playerHasPerk && purchased_Reload_Speed_Perk == true && !playerPerkInventory.has_Reload_Speed_Perk)
            {
                purchased_Reload_Speed_Perk= false;
                playerHasPerk = false;
            }
            if(playerHasPerk && purchased_Sprint_Speed_Perk == true && !playerPerkInventory.has_Sprint_Speed_Perk)
            {
                purchased_Sprint_Speed_Perk = false;
                playerHasPerk = false;
            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(wantsToBuyPerk && !playerHasPerk && canInteract)
            {
                if (PlayerScore.pScore >= perkPrice)
                {
                    InteractAudioSource.current.PlayInteractClip(purchase_successful_sound, 0.5f);
                    playerHasPerk = true;
                    PlayerScore.pScore -= perkPrice;
                    GivePerkType();

                    StartCoroutine(InteractionDelay());
                }
                else
                {
                    InteractAudioSource.current.PlayInteractClip(purchase_failed_sound, 0.5f);
                    StartCoroutine(InteractionDelay());
                }
                
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inTrigger = false;
            InteractionTextbox.current.CloseTextBox();
        }
    }

    //This function is called at start to check which perktype the machine is, it then sets perkType to a specific number
    private void CheckPerkType()
    {
        if (is_Health_Increase_Perk)
            perkType = 1;
        else if (is_Sprint_Speed_Perk)
            perkType = 2;
        else if (is_Reload_Speed_Perk)
            perkType = 3;
        else if (is_Life_Savior_Perk)
            perkType = 4;
    }


    //This function checks what the perktype is, it then gives player the perk
    private void GivePerkType()
    {
        //plays a soda can opening sound when purchasing perk
        InteractAudioSource.current.PlayInteractClip(soda_open_sound, 0.5f);
        
        

        switch (perkType)
        {

            case 1:
                
                playerPerkInventory.has_Health_Increase_Perk = true;
                purchased_Health_Increase_Perk = true;
                playerHealthAccessor.IncreaseHealth();
                //since we want to enter bytes for color values, we need to use new color32 instead of new color()
                var color1 = new Color32(200, 30, 0, 255);
                StartCoroutine(BuffSoundDelay(color1));
                Debug.Log("You have acquired health increase!");
                break;
            case 2:
                
                playerPerkInventory.has_Sprint_Speed_Perk = true;
                purchased_Sprint_Speed_Perk = true;
                playerMovementAccessor.IncreaseSpeed();
                var color2 = new Color32(69, 97, 255, 255);
                StartCoroutine(BuffSoundDelay(color2));
                Debug.Log("You have acquired sprint speed increase!");
                break;
            case 3:
                playerPerkInventory.has_Reload_Speed_Perk = true;
                purchased_Reload_Speed_Perk = true;
                var color3 = new Color32(78, 200, 42, 255);
                StartCoroutine(BuffSoundDelay(color3));
                Debug.Log("You have acquired reload speed increase!");
                break;
            case 4:
                playerPerkInventory.has_Life_Savior_Perk = true;
                purchased_Life_Savior_Perk = true;
                var color4 = new Color32(227, 229, 120, 255);
                StartCoroutine(BuffSoundDelay(color4));
                Debug.Log("You have acquired life savior!");
                break;
        }
    }

    IEnumerator InteractionDelay()
    {
        canInteract = false;
        yield return new WaitForSeconds(1f);
        canInteract = true;
    }
    IEnumerator BuffSoundDelay(Color color)
    {
        var main = player_first_power_particle_effect.main;
        var main2 = player_second_power_particle_effect.main;
        yield return new WaitForSeconds(1f);
        InteractAudioSource.current.PlayInteractClip(soda_buff_sound, 0.5f);
        main.startColor = color;
        main2.startColor = color;
        player_first_power_particle_effect.gameObject.SetActive(true);
        player_first_power_particle_effect.Play();
    }

}
