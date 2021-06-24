using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoundController : MonoBehaviour
{
    public static int zombieCounter = 0;    //initally set to 0, this is variable represents how many zombies are currently alive
    public GameObject zombie;   //original prefab of the zombie
    private GameObject zombieClone; //zombieClones are the zombie that are spawned/instantiated
    public Transform spawnLocation;

    public static int round;

    private int originalZombieCounter = 1; 
    public int zombieIncrementor = 2;   //zombie Counter increases by this amount for each round
    private int ZombieCountAtStartOfRound;

    private int spawnIncrementor = 0; //incrementor for spawning zombies (INSIDE ZombieSpawns() function)

    private int randomSpawnLocationSpot;

    public static int amountOfSpawnLocations;

    private bool zombieCanSpawn;

    private float zombieSpawnTime = 3f;

    public List<Transform> RandomSpawnLocations;

    Target targetScript;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("zombie counter " + zombieCounter);
        //zombieCanSpawn = false;
        round = 0;
        RoundChange.roundChange.onRoundChange += RoundNumberChange;
        amountOfSpawnLocations = 3;

    }
    private void RoundNumberChange()
    {
        //if all zombies are dead, increment the round and increase zombie spawns (we check this inside the RoundActivator script)
        
        //prevents multiple spawning instances, allows us to have correct delay times between rounds and zombie spawns correctly (without this we get infinite invokes)
        CancelInvoke();

        //need to reset spawnIncrementor so we can spawn correct number of zombies
        spawnIncrementor = 0;
        round += 1;


        targetScript = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Target>();
        targetScript.RoundDifficultyIncrease();


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

        InvokeRepeating("ZombieSpawns", 5.0f, zombieSpawnTime);
    }
    private void ZombieSpawns()
    {
        //since zombieCounter is constantly decreasing, we need to remember how many zombies we should be spawning at the start of each round, which is why we use ZombieCountAtStartOfRound
        //if we use spawnIncrementor < zombieCounter, we will spawn less zombies since zombieCounter is constantly decreasing due to player killing zombies
        if(spawnIncrementor < ZombieCountAtStartOfRound)
        {
            //Debug.Log("spawn incrementor " + spawnIncrementor);
            //Debug.Log("zombie at start of round " + ZombieCountAtStartOfRound);
            //Debug.Log("i " + spawnIncrementor);
            //Debug.Log("zombie counter " + zombieCounter);
            randomSpawnLocationSpot = Random.Range(0, amountOfSpawnLocations);

            //Debug.Log(randomSpawnLocationSpot);

            zombieClone = Instantiate(zombie, RandomSpawnLocations[randomSpawnLocationSpot].position, RandomSpawnLocations[randomSpawnLocationSpot].rotation);

            //we get the Speed component from the zombieClone (SEE Target.cs) and randomize its speed, we also make sure to increase that speed by the difficultySpeedIncrease incrementor
            zombieClone.GetComponent<NavMeshAgent>().speed = Random.Range(Target.minSpeed + Target.difficultySpeedIncrease, Target.maxSpeed + Target.difficultySpeedIncrease);

            spawnIncrementor++;

        }
        
    }  
}
