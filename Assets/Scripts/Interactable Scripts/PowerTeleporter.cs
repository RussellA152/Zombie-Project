using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerTeleporter : MonoBehaviour
{
    private bool inTrigger;
    private GameObject player;

    [SerializeField] private Transform superImprover_teleporter_position;

    [SerializeField] private bool buyable_ending_conditions_met;



    // Start is called before the first frame update
    void Start()
    {
        TeleporterEvent.current.onTeleport += TeleportPlayerToUpgradeRoom;

        //get reference for player from event system

    }

    // Update is called once per frame
    void Update()
    {
        //if player is inside this teleporter's trigger check for some input
        if (inTrigger)
        {
            //if teleporter is not linked then wants_to_link_Teleporter is true when player presses 'F'
            if(Input.GetKeyDown(KeyCode.F) && !TeleporterEvent.current.teleporters_are_linked && !TeleporterEvent.current.wants_to_link_teleporters && TeleporterEvent.current.teleporters_can_be_linked)
            {
                TeleporterEvent.current.wants_to_link_teleporters = true;
                TeleporterEvent.current.interactive_audio_source.PlayOneShot(TeleporterEvent.current.interact_successful_sound, 0.5f);
                Debug.Log("Player wants to link teleporters!");
            }
            //if teleporter is already linked and player presses 'F' then teleport player
            else if(Input.GetKeyDown(KeyCode.F) && TeleporterEvent.current.teleporters_are_linked)
            {
                //calls event for teleporting player
                Debug.Log("Teleport Player!");
                TeleporterEvent.current.interactive_audio_source.PlayOneShot(TeleporterEvent.current.teleport_successful_sound,0.5f);
                TeleporterEvent.current.TeleportPlayer();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (PowerEvent.powerIsTurnedOn)
            {
                inTrigger = true;
            }
            else
            {
                Debug.Log("You must turn on power!");
            }

            //TeleporterEvent.current.in_powerRoom_Teleporter_Trigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inTrigger = false;
            //TeleporterEvent.current.in_powerRoom_Teleporter_Trigger = false;
        }
    }
    void TeleportPlayerToUpgradeRoom()
    {
        player = TeleporterEvent.current.player;
        
        //player's position and rotation begins the superimprover_teleporter_position's position
        player.transform.position = new Vector3(superImprover_teleporter_position.transform.position.x, superImprover_teleporter_position.transform.position.y + 1f, superImprover_teleporter_position.transform.position.z);

        StartCoroutine(PlayerMovementDelay());
        //TeleporterEvent.current.player_orientation.transform.rotation = ChangeRotation(0f, 90f, 0f);

        //StartCoroutine(PLayerMovementDelay());

        //make these values false again so player has to relink them, not simply just reuse teleporter
        TeleporterEvent.current.wants_to_link_teleporters = false;
        TeleporterEvent.current.teleporters_are_linked = false;

        StartCoroutine(TeleportBackToStartingRoom());
    }
    IEnumerator TeleportBackToStartingRoom()
    {
        //after some time, teleport player back to starting room
        player = TeleporterEvent.current.player;
        yield return new WaitForSeconds(TeleporterEvent.current.upgradeRoomTimer);

        //if the player has not purchased the buyable ending door, then they will be teleported back, otherwise....
        //they will not be teleported back, so they can't miss the final explosion effect
        if (!BuyableEnding.current.conditions_met)
        {
            InteractAudioSource.current.PlayInteractClip(TeleporterEvent.current.teleport_return_sound, 0.5f);
            player.transform.position = new Vector3(TeleporterEvent.current.startingRoom_Teleporter.transform.position.x, TeleporterEvent.current.startingRoom_Teleporter.transform.position.y + 1f, TeleporterEvent.current.startingRoom_Teleporter.transform.position.z);
            StartCoroutine(PlayerMovementDelay());
        }
        

        //begin teleporter link cooldown
        StartCoroutine(LinkCoolDown());
    }
    IEnumerator LinkCoolDown()
    {
        //player has to wait to link teleporters again after being teleported back to starting room

        //Debug.Log("Teleporter cannot be linked!");
        TeleporterEvent.current.teleporters_can_be_linked = false;
        yield return new WaitForSeconds(TeleporterEvent.current.time_To_Link);
        //Debug.Log("Teleporter can be linked now!");
        TeleporterEvent.current.teleporters_can_be_linked = true;

    }

    IEnumerator PlayerMovementDelay()
    {
        var invincibility = player.gameObject.GetComponent<PlayerHealth>().is_invincible;
        //yield return new WaitForSeconds(1f);
        //TeleporterEvent.current.playerRB.constraints = RigidbodyConstraints.FreezePosition;
        //The player will not be allowed to move for a few seconds shortly after teleporting (this will hopefully prevent any falling through map bugs)
        //InputManager.IsInputEnabled = false;
        //while player is frozen, make them invincible for a second
        invincibility = true;
        yield return new WaitForSeconds(2f);
        
        //InputManager.IsInputEnabled = true;
        //TeleporterEvent.current.playerRB.constraints = RigidbodyConstraints.None;
        invincibility = false;

    }
}
