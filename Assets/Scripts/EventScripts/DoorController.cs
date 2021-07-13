using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;

public class DoorController : MonoBehaviour
{
    public int id;
    //public List<Transform> DoorSpecificSpawnLocations;
    public List<Transform> DoorSpecificSpawnLocations;

    //the MAIN list of spawning locations
    public List<Transform> RandomSpawnLocations { get; private set; }

    private int amountOfSpawnLocations;

    public GameObject roundChangerObject;


    // Start is called before the first frame update
    void Start()
    {
        //subscribing to our event system (s)
        GameEvents.current.onDoorwayTriggerEnter += OnDoorwayOpen;

        //RandomSpawnLocations is the MAIN list that contains the locations of each spawn location 
        RandomSpawnLocations = roundChangerObject.GetComponent<RoundController>().RandomSpawnLocations;
        
    }
    private void OnDoorwayOpen(int id)
    {
        //might be redundant
        if (id == this.id)
        {
            
            DoorZombieSpawning();
        }

        //opens door (destroys it for now)
        if (id == this.id)
        {
            
            Destroy(gameObject);
            

        }
        
        

    }
    private void DoorZombieSpawning()
    {
        //we have a list of spawn locations for each individual door, and we add those specific door spawn locations to our main spawn locations list inside the 'RoundController' script
        for(int i = 0; i < DoorSpecificSpawnLocations.Count; i++)
        {
            var spawnerLocation = DoorSpecificSpawnLocations[i];

            //we check if that spawn location is NOT already in our spawn locations list, if it isn't, then we add that element to our spawn list (and increase the number of spawn locations)
            if (!RandomSpawnLocations.Contains(spawnerLocation))
            {
                RandomSpawnLocations.Add(spawnerLocation);
                RoundController.amountOfSpawnLocations++;
            }
            //RandomSpawnLocations.Add(spawnerLocation);
            //RoundController.amountOfSpawnLocations++;
            
        }
    }
    private void OnDestroy()
    {
        //unsubscribes from event system when door is destroyed/ opened;

        //UPDATES NAVMESH, but causes a game-freeze for a few seconds
        //NavMeshBuilder.BuildNavMesh();

        GameEvents.current.onDoorwayTriggerEnter -= OnDoorwayOpen;
        
    }
}
