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


    private void Start()
    {
        inTrigger = false;
        wantsToBuyDoor = false;
        doorWasOpened = false;
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
            if(PlayerScore.pScore >= doorPrice && wantsToBuyDoor && !doorWasOpened && !isEndingDoor)
            {
                Debug.Log("You opened Door #" + id);
                PlayerScore.pScore -= doorPrice;
                GameEvents.current.DoorwayTriggerEnter(id);
                doorWasOpened = true;

            }
            else if(PlayerScore.pScore >= doorPrice && wantsToBuyDoor && !doorWasOpened && isEndingDoor)
            {
                PlayerScore.pScore -= doorPrice;
                //instead of calling event, we will call our Buyable ending script functions
                BuyableEnding.current.conditions_met = true;
                BuyableEnding.current.CompleteLevel();
                doorWasOpened = true;
            }
        }    
    }
    //when player exits the Door Trigger, inTrigger is set to false
    private void OnTriggerExit(Collider other)
    {
        inTrigger = false; 
    }
}
