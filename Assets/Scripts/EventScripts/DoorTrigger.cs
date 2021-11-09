using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public int id;
    public int doorPrice;
    private bool inTrigger;
    private bool wantsToBuyDoor;
    private bool doorWasOpened;

    public bool isEndingDoor;    //this bool represents whether this door, is the final door to end the level
    [SerializeField] private AudioSource interactive_audio_source;

    [SerializeField] private AudioClip purchase_successful_sound;
    [SerializeField] private AudioClip purchase_failed_sound;


    private void Start()
    {
        inTrigger = false;
        wantsToBuyDoor = false;
        doorWasOpened = false;

        interactive_audio_source = this.transform.GetComponentInParent<DoorController>().interactive_audioSource;
    }
    private void Update()
    {
        //if the player is inside the trigger hitbox of said Door Trigger, AND they are holding the 'F' key, then wantsToBuyDoor is set to true, otherwise it wil be set to false
        if(inTrigger && Input.GetKey(KeyCode.F))
        {
            wantsToBuyDoor = true;
        }
        else if (!inTrigger || Input.GetKey(KeyCode.F) == false)
        {
            wantsToBuyDoor = false;
        }
    }
    //there are hitboxes infront of each side of a buyable door called Door Trigger (s), if player enters the trigger hitbox of said Door Trigger, our event is called

    //when player enters the Door Trigger, inTrigger is set to true
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inTrigger = true;
        }
    }
    //while the player is inside the Door Trigger, we check if wantsToBuyDoor is true, and if they have the suffiicent points to buy the door,
    //if they pass the check, we execute the event and open the door, otherwise the door stays closed
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            //Debug.Log("Hold 'f' to open Door [Cost: " + doorPrice);
            if(wantsToBuyDoor && !doorWasOpened && !isEndingDoor)
            {
                if(PlayerScore.pScore >= doorPrice)
                {
                    PlayerScore.pScore -= doorPrice;
                    GameEvents.current.DoorwayTriggerEnter(id);
                    interactive_audio_source.PlayOneShot(purchase_successful_sound,0.5f);
                    doorWasOpened = true;
                }
                else
                {
                    interactive_audio_source.PlayOneShot(purchase_failed_sound,0.5f);
                }
                

            }
            else if(wantsToBuyDoor && !doorWasOpened && isEndingDoor)
            {
                if(PlayerScore.pScore >= doorPrice)
                {
                    PlayerScore.pScore -= doorPrice;
                    interactive_audio_source.PlayOneShot(purchase_successful_sound,0.5f);
                    //instead of calling event, we will call our Buyable ending script functions
                    GameEvents.current.DoorwayTriggerEnter(id);
                    BuyableEnding.current.conditions_met = true;
                    BuyableEnding.current.CompleteLevel();
                    doorWasOpened = true;
                }
                else
                {
                    interactive_audio_source.PlayOneShot(purchase_failed_sound,0.5f);
                }

                
            }
        }    
    }
    //when player exits the Door Trigger, inTrigger is set to false
    private void OnTriggerExit(Collider other)
    {
        inTrigger = false; 
    }
}
