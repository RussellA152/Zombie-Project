using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public int zombieCounter = 0;
    public GameObject zombie;
    public Transform spawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        GameObject zombieClone;
        for(int i = 0; i < zombieCounter; i++)
        {
            zombieClone = Instantiate(zombie, spawnLocation.position, spawnLocation.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
