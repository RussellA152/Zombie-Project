using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    //The idea with the teleporter is to link the power room teleporter with the starting room teleporter, then when used, 
    //teleport the player from the power room to the superimprover room, then after some time, teleport the player back to the starting room (with a cooldown for linkage)

    private GameObject player;

    //these two variables are static so that both scripts have the same value
    private static bool teleporter_Is_Linked;
    private static bool wants_To_Link_Teleporters;

    //these bools are used to check if the player is inside the teleporter's Trigger colliders
    private bool in_startingRoom_Teleporter_Trigger;
    private bool in_powerRoom_Teleporter_Trigger;
    private bool powerRoom_teleporter_canLink;

    [Header("Teleport Positions")]
    [SerializeField] private Transform powerRoom_teleporter_position;
    [SerializeField] private Transform startingRoom_teleporter_position;
    [SerializeField] private Transform superImprover_teleporter_position;

    [Header("Teleporter Types")]
    [SerializeField] private bool is_PowerRoom_Teleporter;
    [SerializeField] private bool is_StartingRoom_Teleporter;


    [Header("Teleport Cooldowns")]
    //amount of time it takes for teleporter to be relinked
    [SerializeField] private int time_To_Link;
    [SerializeField] private int upgradeRoomTimer;



    //private bool teleporter_Is_Linked;


    private void Start()
    {
        player = GameObject.Find("Player");
        //time_To_Link = 0;
        powerRoom_teleporter_canLink = true;
        teleporter_Is_Linked = false;
        wants_To_Link_Teleporters = false;

        in_startingRoom_Teleporter_Trigger = false;
        in_powerRoom_Teleporter_Trigger = false;

        time_To_Link = 30;
        upgradeRoomTimer = 20;

    }
    private void Update()
    {
        //Debug.Log(wants_To_Link_Teleporters);

        //checking if player wants to link the teleporters
        if (in_powerRoom_Teleporter_Trigger)
        {
            if(Input.GetKeyDown(KeyCode.F) && !teleporter_Is_Linked && !wants_To_Link_Teleporters && powerRoom_teleporter_canLink)
            {
                wants_To_Link_Teleporters = true;
                //Debug.Log("player wants to link teleporter!");
            }
            //if the teleporter is already linked, then check if player wants to teleport to upgrade room, if so, teleport them to upgrade room position
            else if (teleporter_Is_Linked && Input.GetKeyDown(KeyCode.F))
            {
                TeleportToUpgradeRoom();
                
            }

        }
        //if player has linked powerRoom teleporter, then check if they have linked the startingRoom teleporter, if they have then set teleporter_Is_Linked to true
        if (in_startingRoom_Teleporter_Trigger)
        {
            if (wants_To_Link_Teleporters && Input.GetKeyDown(KeyCode.F) && !teleporter_Is_Linked)
            {
                teleporter_Is_Linked = true;
                //Debug.Log("teleporter is linked!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (is_PowerRoom_Teleporter)
            {
                in_powerRoom_Teleporter_Trigger = true;
                //player.transform.position = superImprover_teleporter_position.transform.position;
            }
            if (is_StartingRoom_Teleporter)
            {
                in_startingRoom_Teleporter_Trigger = true;
                //Debug.Log("Press 'F' to link teleporter");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (is_PowerRoom_Teleporter)
            {
                in_powerRoom_Teleporter_Trigger = false;
            }
            else if (is_StartingRoom_Teleporter)
            {
                in_startingRoom_Teleporter_Trigger = false;
            }

        }
    }
    void TeleportToUpgradeRoom()
    {
        player.transform.position = superImprover_teleporter_position.transform.position;

        //make these values false again so player has to relink them, not simply just reuse teleporter
        wants_To_Link_Teleporters = false;
        teleporter_Is_Linked = false;
        StartCoroutine(TeleportBackToStartingRoom());
        //Debug.Log("player was teleported!");
    }

    IEnumerator TeleportBackToStartingRoom()
    {
        //after some time, teleport player back to starting room
        yield return new WaitForSeconds(upgradeRoomTimer);
        player.transform.position = startingRoom_teleporter_position.transform.position;
        StartCoroutine(LinkCoolDown());
    }
    IEnumerator LinkCoolDown()
    {
        //player has to wait to link teleporters again after being teleported back to starting room

        //Debug.Log("Teleporter cannot be linked!");
        powerRoom_teleporter_canLink = false;
        yield return new WaitForSeconds(time_To_Link);
        //Debug.Log("Teleporter can be linked now!");
        powerRoom_teleporter_canLink = true;

    }
}
