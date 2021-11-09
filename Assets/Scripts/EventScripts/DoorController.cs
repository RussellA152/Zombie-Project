using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class DoorController : MonoBehaviour
{
    public int id;
    //public List<Transform> DoorSpecificSpawnLocations;
    public List<Transform> DoorSpecificSpawnLocations;

    //the MAIN list of spawning locations
    public List<Transform> RandomSpawnLocations { get; private set; }

    private int amountOfSpawnLocations;

    public GameObject roundChangerObject;

    [SerializeField] private GameObject doorBomb;    //this is the bomb attached in front of a non-rotatable door
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private Light explosionLight;
    [SerializeField] private float timeUntilDoorDisables;

    [SerializeField] private float rotationValueY;  //this value specifies where the door's y component will rotate to

    [SerializeField] private bool door_is_rotatable;
    [SerializeField] public AudioSource interactive_audioSource;
    [SerializeField] private AudioClip doorOpenSound;

    [SerializeField] private AudioClip detonateStartExplosiveSound;
    [SerializeField] private AudioClip detonateTickingExplosiveSound;



    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 15;
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

        //opens door
        if (id == this.id)
        {

            if (door_is_rotatable)
            {
                LeanTween.rotateLocal(this.gameObject, new Vector3(0, rotationValueY, 0), .5f);
                interactive_audioSource.PlayOneShot(doorOpenSound,1f);
            }

            else
            {
                //door will explode instead of rotate
                StartCoroutine(DoorExplosion());
                
            }
                

            //disabls the nav mesh obstacle on the door so zombies can walk the new door path
            //GetComponent<NavMeshObstacle>().enabled = false;
            GameEvents.current.onDoorwayTriggerEnter -= OnDoorwayOpen;


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
    IEnumerator DoorExplosion()
    {
        //play detonate sound
        doorBomb.SetActive(true);
        interactive_audioSource.PlayOneShot(detonateStartExplosiveSound, 1f);
        yield return new WaitForSeconds(.7f);
        interactive_audioSource.PlayOneShot(detonateTickingExplosiveSound,1f);
        yield return new WaitForSeconds(1.35f);
        interactive_audioSource.PlayOneShot(doorOpenSound, 1f);
        CameraShaker.Instance.ShakeOnce(22f, 5f, 0.1f, 2.5f);
        //the particle system will renable its script to auto delete itself after animation is done playing
        explosionParticles.gameObject.GetComponent<CFX_AutoDestructShuriken>().enabled = true;
        doorBomb.SetActive(false);
        explosionParticles.Play();
        explosionLight.gameObject.SetActive(true);
        //disables explosion light without worrying about when this door will be destoryed
        explosionLight.GetComponent<DoorExplosionLight>().DisableThisLight();
        yield return new WaitForSeconds(timeUntilDoorDisables);
        Destroy(gameObject);


    }
    private void OnDestroy()
    {
        //unsubscribes from event system when door is destroyed/ opened;

        //UPDATES NAVMESH, but causes a game-freeze for a few seconds
        //NavMeshBuilder.BuildNavMesh();

        GameEvents.current.onDoorwayTriggerEnter -= OnDoorwayOpen;
        
    }
}
