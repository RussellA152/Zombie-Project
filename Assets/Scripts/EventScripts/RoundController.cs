using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoundController : MonoBehaviour
{
    public static int zombieCounter;    //initally set to 0, this is variable represents how many zombies are currently alive

    public GameObject zombie;   //original prefab of the zombie

    [SerializeField] private List<GameObject> ZombiePrefabs;   //list of zombie gameobject prefabs
    private int length_of_zombie_prefabs;   //the length of the list of zombie prefabs

    public GameObject zombieDog;  //original prefab of the zombie 'dog' (smaller zombie essentially)

    private GameObject zombieClone; //zombieClones are the zombie that are spawned/instantiated
    private GameObject zombieDogClone;

    public Transform spawnLocation;

    public static int round;

    public int zombieIncrementor;   //zombie Counter increases by this amount for each round
    private int ZombieCountAtStartOfRound; //basically the zombie counter but without decrementation (this number doesn't undergo any subtraction like zombieCounter does)

    private int spawnIncrementor = 0; //incrementor for spawning zombies (INSIDE ZombieSpawns() function) (this increases each time a zombie has spawned)

    private int randomSpawnLocationSpot;

    public static int amountOfSpawnLocations;

    private bool zombieCanSpawn;

    private float zombieSpawnTime = 3f;
    private float zombieDogSpawnTime = 4f;

    private PlayerUI player_ui_accessor;
    [SerializeField] private GameObject pHud;

    //OUR MAIN LIST OF SPAWNING LOCATIONS / TRANSFORMS
    public List<Transform> RandomSpawnLocations;

    Target targetScript;

    private AudioSource backGroundMusicAccessor;

    // Start is called before the first frame update
    void Start()
    {
        zombieCounter = 0;
        round = 0;
        RoundChange.roundChange.onRoundChange += RoundNumberChange;
        amountOfSpawnLocations = 3;
        length_of_zombie_prefabs = ZombiePrefabs.Count;

        pHud = GameObject.Find("Player's HUD");
        player_ui_accessor = pHud.GetComponent<PlayerUI>();

        backGroundMusicAccessor = BackgroundMusic.current.gameObject.GetComponent<AudioSource>();
        //round = 144;
        //round = 25;
    }
    private void Update()
    {
    }

    private void RoundNumberChange()
    {
        //if all zombies are dead, increment the round and increase zombie spawns (we check this inside the RoundActivator script)

        //if the round is not round 0, then dont play the round ending sound, only the round starting sound
        RoundActivator.round_must_increment = false;
        //if(round != 0)
        //{
          //  InteractAudioSource.current.PlayInteractClip(RoundChange.roundChange.round_ending_sound, 0.5f);
           // StartCoroutine(PlayRoundChangeSoundOnly());
        //}
        StartCoroutine(RoundChangeWithSoundCoroutine());
    }
    private void ZombieSpawns()
    {
        //since zombieCounter is constantly decreasing, we need to remember how many zombies we should be spawning at the start of each round, which is why we use ZombieCountAtStartOfRound
        //if we use spawnIncrementor < zombieCounter, we will spawn less zombies since zombieCounter is constantly decreasing due to player killing zombies
        if(spawnIncrementor < ZombieCountAtStartOfRound)
        {
            randomSpawnLocationSpot = Random.Range(0, amountOfSpawnLocations);

            //randomly chooses which zombie prefab to spawn in 
            int randomZombiePrefabNumber = Random.Range(0, length_of_zombie_prefabs - 1);
            zombie = ZombiePrefabs[randomZombiePrefabNumber];

            //RandomSpawnLocation's number of elements are increased through the DoorController event System, when opening doors, we add more elements to the RandomSpawnLcations list
            zombieClone = Instantiate(zombie, RandomSpawnLocations[randomSpawnLocationSpot].position, RandomSpawnLocations[randomSpawnLocationSpot].rotation);
            var zombieCloneData = zombieClone.GetComponent<Target>();

            //we get the Speed component from the zombieClone (SEE Target.cs) and randomize its speed, we also make sure to increase that speed by the difficultySpeedIncrease incrementor
            zombieClone.GetComponent<NavMeshAgent>().speed = Random.Range(zombieCloneData.minRandomSpeed + Target.difficultySpeedIncrease, zombieCloneData.maxRandomSpeed + Target.difficultySpeedIncrease);

            spawnIncrementor += 1;

            //Debug.Log("Spawned a zombie!");

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
            var zombieDogCloneData = zombieDogClone.GetComponent<Target>();

            //we get the Speed component from the zombieClone (SEE Target.cs) and randomize its speed, we also make sure to increase that speed by the difficultySpeedIncrease incrementor
            zombieDogClone.GetComponent<NavMeshAgent>().speed = Random.Range(zombieDogCloneData.minRandomSpeed + Target.difficultySpeedIncrease, zombieDogCloneData.maxRandomSpeed + Target.difficultySpeedIncrease);

            spawnIncrementor += 1;

            //Debug.Log("Spawned a zombie DOG!");
        }
    }

    private void ZombieAndDogSpawns()
    {
        if(spawnIncrementor < ZombieCountAtStartOfRound)
        {
            var zombieTypeSpawnChance = Random.Range(0, 7);

            // 4/7 chance to spawn a regular zombie 
            if(zombieTypeSpawnChance % 2 == 0)
            {
                randomSpawnLocationSpot = Random.Range(0, amountOfSpawnLocations);

                int randomZombiePrefabNumber = Random.Range(0, length_of_zombie_prefabs - 1);
                zombie = ZombiePrefabs[randomZombiePrefabNumber];

                //RandomSpawnLocation's number of elements are increased through the DoorController event System, when opening doors, we add more elements to the RandomSpawnLcations list
                zombieClone = Instantiate(zombie, RandomSpawnLocations[randomSpawnLocationSpot].position, RandomSpawnLocations[randomSpawnLocationSpot].rotation);
                var zombieCloneData = zombieClone.GetComponent<Target>();

                //we get the Speed component from the zombieClone (SEE Target.cs) and randomize its speed, we also make sure to increase that speed by the difficultySpeedIncrease incrementor
                zombieClone.GetComponent<NavMeshAgent>().speed = Random.Range(zombieCloneData.minRandomSpeed + Target.difficultySpeedIncrease, zombieCloneData.maxRandomSpeed + Target.difficultySpeedIncrease);

                spawnIncrementor += 1;
                //Debug.Log("Spawned a zombie! (SIMUTANEOUSLY)");
            }
            
            //smaller chance to spawn a zombie dog
            else
            {
                randomSpawnLocationSpot = Random.Range(0, amountOfSpawnLocations);


                //RandomSpawnLocation's number of elements are increased through the DoorController event System, when opening doors, we add more elements to the RandomSpawnLcations list
                zombieDogClone = Instantiate(zombieDog, RandomSpawnLocations[randomSpawnLocationSpot].position, RandomSpawnLocations[randomSpawnLocationSpot].rotation);
                var zombieDogCloneData = zombieDogClone.GetComponent<Target>();

                //we get the Speed component from the zombieClone (SEE Target.cs) and randomize its speed, we also make sure to increase that speed by the difficultySpeedIncrease incrementor
                zombieDogClone.GetComponent<NavMeshAgent>().speed = Random.Range(zombieDogCloneData.minRandomSpeed + Target.difficultySpeedIncrease, zombieDogCloneData.maxRandomSpeed + Target.difficultySpeedIncrease);

                spawnIncrementor += 1;

                //Debug.Log("Spawned a zombie DOG! (SIMUTANEOUSLY)");

            }



        }


    }
    IEnumerator RoundChangeWithSoundCoroutine()
    {
        Debug.Log("START ROUND CHANGE COROUTINE");
        if(round == 0)
        {
            backGroundMusicAccessor.Pause();
            yield return new WaitForSeconds(1f);
        }
        else
        {
            backGroundMusicAccessor.Pause();
            yield return new WaitForSeconds(0.5f);
        }
        

        if(round != 0)
        {
            //pauses current background music
            InteractAudioSource.current.PlayInteractClip(RoundChange.roundChange.round_ending_sound, 0.5f);
            yield return new WaitForSeconds(7f);
        }
        

        InteractAudioSource.current.PlayInteractClip(RoundChange.roundChange.round_starting_sound, 0.5f);
        player_ui_accessor.RoundChangeUIAnimation();
        
        yield return new WaitForSeconds(3.3f);

        //prevents multiple spawning instances, allows us to have correct delay times between rounds and zombie spawns correctly (without this we get infinite invokes)
        CancelInvoke();
        //StartCoroutine(PlayRoundChangeSound());

        //need to reset spawnIncrementor so we can spawn correct number of zombies
        spawnIncrementor = 0;
        round += 1;

        //first, access the original zombie's health to increase it
        for (int i = 0; i < length_of_zombie_prefabs; i++)
        {
            targetScript = ZombiePrefabs[i].GetComponent<Target>();
            targetScript.RoundDifficultyIncrease();
        }
        //we don't care where we call this because speed is static and we only need to increase it once for all zombies to be affected
        targetScript.IncreaseSpeed();

        //secondly, access the original zombie dog's health to increase it (by reassigning the targetscript)
        targetScript = zombieDog.GetComponent<Target>();
        targetScript.RoundDifficultyIncrease();


        //This should be hard-coded to make sure the zombie counter caps at 25 maximum zombies
        if (zombieIncrementor < 22)
        {
            zombieIncrementor = (round * 2) + 1;
        }
        //round 1 wil have 4 zombies, round 2 will have 6, round 3 will have 8, etc.
        zombieCounter = zombieIncrementor;
        //since zombieCounter is constantly decreasing, we need to remember how many zombies we should be spawning at the start of each round, which is why we use ZombieCountAtStartOfRound
        ZombieCountAtStartOfRound = zombieCounter;

        yield return new WaitForSeconds(.5f);
        //unpauses current background music
        backGroundMusicAccessor.UnPause();

        //if the round is 16 or higher and is not divisible by 5, spawn zombies and dogs simutaneously
        if (round >= 16 && round % 5 != 0 && round != 0)
        {
            InvokeRepeating("ZombieAndDogSpawns", 6.0f, zombieSpawnTime);
        }
        //we check if the round is 5,10,15,20, etc. if so, then it will be a dedicated dog round
        else if (round % 5 == 0 && round != 0)
        {
            //starts in 5 seconds, and invokes every 3 seconds
            //in 5 seconds, we spawn our first zombie, then every 3 seconds after that, we spawn each other zombie
            InvokeRepeating("ZombieDogSpawns", 6.0f, zombieDogSpawnTime);
            // Debug.Log("Spawn Dogs!");
        }
        //if the round is not divisible by 5 (ex. 2,3,4,9), then it will be a normal zombie round
        else
        {
            InvokeRepeating("ZombieSpawns", 6.0f, zombieSpawnTime);
            //Debug.Log("Spawn Zombies!");
        }
        RoundActivator.round_must_increment = true;
    }
        public void StopZombieSpawning()
    {
        //unsubscribes zombie spawning function from Round event system to stop zombie spawns
        RoundChange.roundChange.onRoundChange -= RoundNumberChange;
    }
}
