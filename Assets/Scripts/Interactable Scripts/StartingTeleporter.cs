using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingTeleporter : MonoBehaviour
{
    private bool inTrigger;
    private GameObject player;


    private void Update()
    {
        if (inTrigger)
        {
            if (TeleporterEvent.current.wants_to_link_teleporters && Input.GetKeyDown(KeyCode.F) && !TeleporterEvent.current.teleporters_are_linked)
            {
                TeleporterEvent.current.teleporters_are_linked = true;
                TeleporterEvent.current.interactive_audio_source.PlayOneShot(TeleporterEvent.current.teleport_link_sound, 0.5f);
                Debug.Log("Teleporters are linked!");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
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
}
