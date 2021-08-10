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

    [SerializeField] int perkPrice;

    private int perkType;

    [Header("Perk Types")]
    public bool is_Life_Savior_Perk;
    public bool is_Reload_Speed_Perk;
    public bool is_Sprint_Speed_Perk;
    public bool is_Health_Increase_Perk;

    [Header("Player Owned Perks")]
    public bool has_Life_Savior_Perk;
    public bool has_Reload_Speed_Perk;
    public bool has_Sprint_Speed_Perk;
    public bool has_Health_Increase_Perk;

    [Header("GameObject Accessors")]
    public GameObject player;
    //public GameObject weaponHolder;

    private PlayerHealth playerHealthAccessor;
    private PlayerMovement playerMovementAccessor;
    

    private void Awake()
    {
        current = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerHealthAccessor = player.GetComponent<PlayerHealth>();
        playerMovementAccessor = player.GetComponent<PlayerMovement>();



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
            inTrigger = true;
            Debug.Log("Entered perk machine trigger");

            if(playerHasPerk && playerHealthAccessor.has_Life_Savior_Perk == false)
            {
                playerHasPerk = false;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(wantsToBuyPerk && PlayerScore.pScore >= perkPrice && !playerHasPerk)
            {
                playerHasPerk = true;
                PlayerScore.pScore -= perkPrice;
                GivePerkType();
                
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inTrigger = false;
            Debug.Log("Exited perk machine trigger");
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
        switch (perkType)
        {
            case 1:
                //has_Health_Increase_Perk = true;
                playerHealthAccessor.has_Health_Increase_Perk = true;
                playerHealthAccessor.IncreaseHealth();
                Debug.Log("You have acquired health increase!");
                break;
            case 2:
                //has_Sprint_Speed_Perk = true;
                playerMovementAccessor.has_speed_increase_perk = true;
                playerMovementAccessor.IncreaseSpeed();
                Debug.Log("You have acquired sprint speed increase!");
                break;
            case 3:
                //has_Reload_Speed_Perk = true;
                Debug.Log("You have acquired reload speed increase!");
                break;
            case 4:
                has_Life_Savior_Perk = true;
                playerHealthAccessor.has_Life_Savior_Perk = true;
                Debug.Log("You have acquired life savior!");
                break;
        }
    }
    
}
