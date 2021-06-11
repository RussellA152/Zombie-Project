using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    public static int zombieCounter = 0;    //initally set to 0, this is variable represents how many zombies are currently alive
    public GameObject zombie;   //original prefab of the zombie
    private GameObject zombieClone; //zombieClones are the zombie that are spawned/instantiated
    public Transform spawnLocation;
    public int round;

    private int originalZombieCounter = 1; 
    public int zombieIncrementor = 2;   //zombie Counter increases by this amount for each round
    private int ZombieCountAtStartOfRound;

    private int spawnIncrementor = 0; //incrementor for spawning zombies (INSIDE ZombieSpawns() function)

    private int randomSpawnLocationSpot;

    public static int amountOfSpawnLocations;

    private bool zombieCanSpawn;

    public List<Transform> RandomSpawnLocations;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("zombie counter " + zombieCounter);
        //zombieCanSpawn = false;
        round = 0;
        RoundChange.roundChange.onRoundChange += RoundNumberChange;
        amountOfSpawnLocations = 3;
    }
    private void Update()
    {
        //Debug.Log(zombieCounter);
        //Debug.Log(zombieCanSpawn);
        Debug.Log(round);
    }
    private void RoundNumberChange()
    {
        
        //if all zombies are dead, increment the round and increase zombie spawns
        if(zombieCounter == 0)
        {
            //need to reset spawnIncrementor so we can spawn correct number of zombies
            spawnIncrementor = 0;
            round += 1;
            //round 1 wil have 3 zombies, round 2 will have 5, round 3 will have 7, etc.
            zombieCounter = originalZombieCounter + zombieIncrementor;
            //since zombieCounter is constantly decreasing, we need to remember how many zombies we should be spawning at the start of each round, which is why we use ZombieCountAtStartOfRound
            ZombieCountAtStartOfRound = zombieCounter;
            if (zombieIncrementor < 23)
            {
                zombieIncrementor += 2;
            }
            //ZombieSpawns(zombieCounter);

            //starts in 2 seconds, and invokes every 3 seconds
            //in 2 seconds, we spawn our first zombie, then every 3 seconds after that, we spawn each other zombie
            InvokeRepeating("ZombieSpawns", 5.0f, 3.0f);

        }
    }
    private void ZombieSpawns()
    {
        //since zombieCounter is constantly decreasing, we need to remember how many zombies we should be spawning at the start of each round, which is why we use ZombieCountAtStartOfRound
        //if we use spawnIncrementor < zombieCounter, we will spawn less zombies since zombieCounter is constantly decreasing due to player killing zombies
        if(spawnIncrementor < ZombieCountAtStartOfRound)
        {
            //Debug.Log("i " + spawnIncrementor);
            //Debug.Log("zombie counter " + zombieCounter);
            randomSpawnLocationSpot = Random.Range(0, amountOfSpawnLocations);

            //Debug.Log(randomSpawnLocationSpot);

            zombieClone = Instantiate(zombie, RandomSpawnLocations[randomSpawnLocationSpot].position, RandomSpawnLocations[randomSpawnLocationSpot].rotation);
            spawnIncrementor++;

        }
        /*
        
        for (int i = 0; i < zombieCounter; i++)
        {
            Debug.Log("i"+i);
            
            StartCoroutine(ZombieSpawnDelay());
            if (zombieCanSpawn)
            {
                randomSpawnLocationSpot = Random.Range(0, amountOfSpawnLocations);

                Debug.Log(randomSpawnLocationSpot);

                zombieClone = Instantiate(zombie, RandomSpawnLocations[randomSpawnLocationSpot].position, RandomSpawnLocations[randomSpawnLocationSpot].rotation);
                
            }
            //zombieCanSpawn = false;

            /*
            randomSpawnLocationSpot = Random.Range(0, amountOfSpawnLocations);
            //changes the spawnLocation's position for each zombie instiantation so they have each have different spawn locations
            //RandomSpawnLocations[j].position = new Vector3(Random.Range(-50, -40), 1, Random.Range(-10, -5));

            Debug.Log(randomSpawnLocationSpot);

            //spawnLocation.position = new Vector3(Random.Range(-50, -40), 1, Random.Range(-10, -5));

            StartCoroutine(ZombieSpawnDelay());  
            
            //zombieClone = Instantiate(zombie, RandomSpawnLocations[j].position, RandomSpawnLocations[j].rotation);
            */
        //}
    }
    /*
    IEnumerator ZombieSpawnDelay()
    {
        zombieCanSpawn = false;
        yield return new WaitForSeconds(5f);
        zombieCanSpawn = true;
    }
    */
}
