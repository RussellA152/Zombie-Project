using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    public static int zombieCounter = 0;
    public GameObject zombie;
    public Transform spawnLocation;
    public int round;
    private int originalZombieCounter = 1;
    public int zombieIncrementor = 2;

    public static int amountOfSpawnLocations;

    public List<Transform> RandomSpawnLocations;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("zombie counter " + zombieCounter);
        round = 0;
        RoundChange.roundChange.onRoundChange += RoundNumberChange;
        amountOfSpawnLocations = 3;
    }
    private void Update()
    {
        //Debug.Log(zombieCounter);
    }
    private void RoundNumberChange()
    {
        //Debug.Log("zombie counter" + zombieCounter);
        //if all zombies are dead, increment the round and increase zombie spawns
        if(zombieCounter == 0)
        {
            round += 1;
            zombieCounter = originalZombieCounter + zombieIncrementor;
            ZombieSpawns(zombieCounter);

        }
    }
    private void ZombieSpawns(int zombieCounter)
    {
        GameObject zombieClone;
        for (int i = 0; i < zombieCounter; i++)
        {
            int j = Random.Range(0, amountOfSpawnLocations);
            //changes the spawnLocation's position for each zombie instiantation so they have each have different spawn locations
            //RandomSpawnLocations[j].position = new Vector3(Random.Range(-50, -40), 1, Random.Range(-10, -5));
            Debug.Log(j);
            //spawnLocation.position = new Vector3(Random.Range(-50, -40), 1, Random.Range(-10, -5));

            zombieClone = Instantiate(zombie, RandomSpawnLocations[j].position, RandomSpawnLocations[j].rotation);
        }
    }
}
