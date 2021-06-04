using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int id;
    // Start is called before the first frame update
    void Start()
    {
        //subscribing to our event system
        GameEvents.current.onDoorwayTriggerEnter += OnDoorwayOpen;
    }
    private void OnDoorwayOpen(int id)
    {
        if(id == this.id)
        {
            Destroy(gameObject);

        }
        //opens door (destroys it for now)
    }
    private void OnDestroy()
    {
        //unsubscribes from event system when door is destroyed/ opened;
        GameEvents.current.onDoorwayTriggerEnter -= OnDoorwayOpen;
    }
}
