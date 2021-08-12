using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerkInventory : MonoBehaviour
{
    [Header("Perks")]
    public bool has_Health_Increase_Perk;
    public bool has_Sprint_Speed_Perk;
    public bool has_Reload_Speed_Perk;
    public bool has_Life_Savior_Perk;


    // Start is called before the first frame update
    void Start()
    {
        has_Health_Increase_Perk = false;
        has_Life_Savior_Perk = false;
        has_Reload_Speed_Perk = false;
        has_Sprint_Speed_Perk = false;
    }

}
