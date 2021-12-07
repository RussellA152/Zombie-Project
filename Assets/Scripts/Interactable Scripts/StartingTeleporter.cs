
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
                InteractionTextbox.current.ChangeTextBoxDescription("Teleporters were linked!");
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
                if (!TeleporterEvent.current.teleporters_are_linked && TeleporterEvent.current.wants_to_link_teleporters)
                    InteractionTextbox.current.ChangeTextBoxDescription("Press 'F' to link teleporters.");
                else if(TeleporterEvent.current.teleporters_are_linked)
                    InteractionTextbox.current.ChangeTextBoxDescription("This teleporter is already linked.");
                else if (TeleporterEvent.current.teleporters_can_be_linked)
                    InteractionTextbox.current.ChangeTextBoxDescription("Teleporters on cooldown.");
            }
            else
            {
                InteractionTextbox.current.ChangeTextBoxDescription("The power must be turned on!");
            }
            
            //TeleporterEvent.current.in_powerRoom_Teleporter_Trigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inTrigger = false;
            InteractionTextbox.current.CloseTextBox();
            //TeleporterEvent.current.in_powerRoom_Teleporter_Trigger = false;
        }
    }
}
