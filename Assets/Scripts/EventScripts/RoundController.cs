using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    public static int zombieCounter = 0;
    public GameObject zombie;
    public Transform spawnLocation;
    public int round;
    private int originalZombieCounter = 5;
    public int zombieIncrementor = 2;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("zombie counter " + zombieCounter);
        round = 0;
        RoundChange.roundChange.onRoundChange += RoundNumberChange;
    }
    private void Update()
    {
        Debug.Log(zombieCounter);
    }
    private void RoundNumberChange()
    {
        Debug.Log("zombie counter" + zombieCounter);
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
            zombieClone = Instantiate(zombie, spawnLocation.position, spawnLocation.rotation);
        }
    }
}
