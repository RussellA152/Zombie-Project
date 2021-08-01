using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponInventory : MonoBehaviour
{
    // Start is called before the first frame update

    //an array of guns that the player current has
    //public GameObject[] currentWeapons;

    public List<GameObject> currentWeaponsList;

    void Start()
    {
        
        UpdatePlayerWeaponInventory();

        currentWeaponsList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Weapon"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //Updates the player's inventory
    public void UpdatePlayerWeaponInventory()
    {
        //currentWeapons = GameObject.FindGameObjectsWithTag("Weapon");
    }
}
