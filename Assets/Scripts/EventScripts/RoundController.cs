using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoundController : MonoBehaviour
{
    public static int zombieCounter = 0;    //initally set to 0, this is variable represents how many zombies are currently alive
    public GameObject zombie;   //original prefab of the zombie
    public GameObject zombieDog;  //original prefab of the zombie 'dog' (smaller zombie essentially)
    private GameObject zombieClone; //zombieClones are the zombie that are spawned/instantiated
    private GameObject zombieDogClone;
    public Transform spawnLocation;

    public static int round;

    private int originalZombieCounter = 1; 
    public int zombieIncrementor = 2;   //zombie Counter increases by this amount for each round
    private int ZombieCountAtStartOfRound; //basically the zombie counter but without decrementation (this number doesn't undergo any subtraction like zombieCounter does)

    private int spawnIncrementor = 0; //incrementor for spawning zombies (INSIDE ZombieSpawns() function) (this increases each time a zombie has spawned)

    private int randomSpawnLocationSpot;

    public static int amountOfSpawnLocations;

    private bool zombieCanSpawn;

    private float zombieSpawnTime = 3f;
    private float zombieDogSpawnTime = 4f;

    //OUR MAIN LIST OF SPAWNING LOCATIONS / TRANSFORMS
    public List<Transform> RandomSpawnLocations;

    Target targetScript;

    // Start is called before the first frame update
    void Start()
    {
        
        round = 0;
        RoundChange.roundChange.onRoundChange += RoundNumberChange;
        amountOfSpawnLocations = 3;

    }
    private void Update()
    {
        
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


        //we check if the round is 5,10,15,20, etc. if so, then it will be a dedicated dog round
        if (round % 5 == 0 && round !=0)
        {
            //starts in 5 seconds, and invokes every 3 seconds
            //in 5 seconds, we spawn our first zombie, then every 3 seconds after that, we spawn each other zombie
            InvokeRepeating("ZombieDogSpawns", 5.0f, zombieDogSpawnTime);
            Debug.Log("Spawn Dogs!");
        }
        //if the round is not divisible by 5 (ex. 2,3,4,9), then it will be a normal zombie round
        else
        {
            InvokeRepeating("ZombieSpawns", 5.0f, zombieSpawnTime);
            Debug.Log("Spawn Zombies!");
        }
    }
    private void ZombieSpawns()
    {
        //since zombieCounter is constantly decreasing, we need to remember how many zombies we should be spawning at the start of each round, which is why we use ZombieCountAtStartOfRound
        //if we use spawnIncrementor < zombieCounter, we will spawn less zombies since zombieCounter is constantly decreasing due to player killing zombies
        if(spawnIncrementor < ZombieCountAtStartOfRound)
        {
            randomSpawnLocationSpot = Random.Range(0, amountOfSpawnLocations);
            //Debug.Log("random spawn: " + randomSpawnLocationSpot);
            //Debug.Log(randomSpawnLocationSpot);

            //RandomSpawnLocation's number of elements are increased through the DoorController event System, when opening doors, we add more elements to the RandomSpawnLcations list
            zombieClone = Instantiate(zombie, RandomSpawnLocations[randomSpawnLocationSpot].position, RandomSpawnLocations[randomSpawnLocationSpot].rotation);
            var zombieCloneData = zombieClone.GetComponent<Target>();

            //we get the Speed component from the zombieClone (SEE Target.cs) and randomize its speed, we also make sure to increase that speed by the difficultySpeedIncrease incrementor
            zombieClone.GetComponent<NavMeshAgent>().speed = Random.Range(zombieCloneData.minRandomSpeed + Target.difficultySpeedIncrease, zombieCloneData.maxRandomSpeed + Target.difficultySpeedIncrease);

            spawnIncrementor += 1;

        }
        
    }

    //spawning zombie dogs instead of normal zombies
    private void ZombieDogSpawns()
    {
        
        if(spawnIncrementor < ZombieCountAtStartOfRound)
        {
            randomSpawnLocationSpot = Random.Range(0, amountOfSpawnLocations);
            

            //RandomSpawnLocation's number of elements are increased through the DoorController event System, when opening doors, we add more elements to the RandomSpawnLcations list
            zombieDogClone = Instantiate(zombieDog, RandomSpawnLocations[randomSpawnLocationSpot].position, RandomSpawnLocations[randomSpawnLocationSpot].rotation);
            var zombieDogCloneData = zombieClone.GetComponent<Target>();

            //we get the Speed component from the zombieClone (SEE Target.cs) and randomize its speed, we also make sure to increase that speed by the difficultySpeedIncrease incrementor
            zombieDogClone.GetComponent<NavMeshAgent>().speed = Random.Range(zombieDogCloneData.minRandomSpeed + Target.difficultySpeedIncrease, zombieDogCloneData.maxRandomSpeed + Target.difficultySpeedIncrease);

            spawnIncrementor += 1;
        }
    }

    public void StopZombieSpawning()
    {
        //unsubscribes zombie spawning function from Round event system to stop zombie spawns
        RoundChange.roundChange.onRoundChange -= RoundNumberChange;
    }
}
