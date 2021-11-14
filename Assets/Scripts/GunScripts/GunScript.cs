using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using EZCameraShake;

public class GunScript : MonoBehaviour
{
    //all layers are able to be hit by bullets EXCEPT the "Ragdoll" layer which is meant for corpses so the player doesn't shoot them
    //private LayerMask bullet_applicable_layermasks = ~(1 << 11);
    [SerializeField] LayerMask bullet_applicable_layermasks;

    //these recoil variables only affect the view model weapons
    [SerializeField] private float recoilRotateX;
    [SerializeField] private float recoilPositionZ;
    [SerializeField] private float magnitudePower;
    [SerializeField] private float shakeFadeOutTime;

    private GameObject weaponHolder;

    [SerializeField] private bool isAutomatic;

    //values for damage, range, firerate, etc. of current weapon (each weapon should have different values)
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 100f;
    public float fireRate = 0.5f;

    [SerializeField] private float upgradeDamageMultiplier;


    public int original_mag_size;
    public int current_mag_size;
    public int ammoCapacity;
    public int original_ammoCapacity;
    public int bullets_fired;

    public float reloadSpeed;

    public int ammoPrice;

    private bool isReloading = false;
    public bool isSwapping;
    public bool ammoIsFull;
    public bool isUpgraded;

    //referencing our in-game camera (also our origin point for bullet raycasts)
    private Camera fpsCam;

    //reference to a particle System (like muzzle flashes)
    public ParticleSystem muzzleFlash;

    //shooting animation for gun
    public Animation gunAnimation;

    //reloading animation
    public Animator animator;

    //the impact particle from shooting at surfaces/enemies
    public GameObject impactEffect;
    [SerializeField] private GameObject bloodImpactEffect;

    public AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound1;  //remove mag sound
    [SerializeField] private AudioClip reloadSound2;  //load mag sound
    [SerializeField] private AudioClip reloadSound3;  //gun cocking sound
    [SerializeField] private float timeBetweenReloadSounds;  //this is the time between each of the three reload sounds (its so each gun can be slightly different)

    //public AudioClip reloadSound;
    private AudioSource gunAudio;
    private PlayerUI PlayerUIAccessor;
    private PlayerPerkInventory playerPerkAccessor;

    

    //reloading key
    [SerializeField] KeyCode reloadKey = KeyCode.R;

    //set to 0 by default to allow us to shoot at least once
    private float nextTimeToFire = 0f;


    private NavMeshAgent hitRB;
    private Target target;

    private void Start()
    {

        //Application.targetFrameRate = 15;
        //setting the "animator" to the Animator component of WeaponHolder (THIS WON'T WORK IF WE HAVE MULITPLE SCENES)
        GameObject player = GameObject.Find("Player");
        GameObject WeaponHolder = GameObject.Find("WeaponHolder");
        weaponHolder = WeaponHolder;
        GameObject PlayerHud = GameObject.Find("Player's HUD");
        animator = WeaponHolder.GetComponent<Animator>();
        PlayerUIAccessor = PlayerHud.GetComponent<PlayerUI>();
        playerPerkAccessor = player.GetComponent<PlayerPerkInventory>();

        //setting gunAudio to the audio source of the gun
        GameObject GunAudioObject = GameObject.Find("Gun Audio");
        gunAudio = GunAudioObject.GetComponent<AudioSource>();

        //setting fpsCam to the Main Camera
        fpsCam = Camera.main;
        

        current_mag_size = original_mag_size;
        bullets_fired = original_mag_size - current_mag_size;
        isUpgraded = false;

        //when gun is spawned in, update ammo UI
        PlayerUIAccessor.RetrieveAmmoInfo();
        PlayerUIAccessor.UpdatePlayerAmmoUI();

        

    }
    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);

        //animator.SetBool("Swapping", false);

        //when player swaps weapons, update ammo UI
        PlayerUIAccessor.RetrieveAmmoInfo();
        PlayerUIAccessor.UpdatePlayerAmmoUI();
    }
    private void OnDisable()
    {
        //animator.SetBool("Swapping", true);
    }

    // Update is called once per frame
    void Update()
    {
        

        if((current_mag_size == original_mag_size) && (ammoCapacity == original_ammoCapacity))
        {
            ammoIsFull = true;
        }
        else
        {
            ammoIsFull = false;
        }
        //Debug.Log(ammoIsFull);
        //checking if player is walking, playing walking animation 
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        //if player is completely out of ammo, they cannot reload or shoot 
        //If player has infinite ammo, then allow them to continue shooting
        if(ammoCapacity == 0 && current_mag_size == 0 && !PowerUpEvent.current.hasInfiniteAmmo)
        {
            return;
        }
        //you can't shoot if you're reloading
        if (isReloading || isSwapping)
        {
            return;
        }
        //if your magazine is empty, you must reload
        //if player has ifinite ammo, and their mag is empty, then don't let them reload
        if (current_mag_size <= 0f && !PowerUpEvent.current.hasInfiniteAmmo)
        {
            StartCoroutine(Reload());
            return;
        }
        //pressing the 'r' key will let you reload prematurely, but only if you actually have ammo to reload
        if(Input.GetKeyDown(reloadKey) && current_mag_size >= 0 && ammoCapacity != 0 && current_mag_size != original_mag_size)
        {
            StartCoroutine(Reload());
            return;
        }
        if (InputManager.IsInputEnabled)
        {
            //pressing left mouse will shoot
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && isAutomatic)
            {
                //higher firerate values allow player to shoot faster
                //essentially, we are setting nextTimeToFire equal to the sum of Time.time and 1/(firerate) 
                //if the Time.time is 5 seconds, and our nextTimeToFire is 5.25, we need to wait .25 seconds to shoot again
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
            if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && !isAutomatic)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }
            
    }
    void Shoot()
    {
        //plays muzzleFlash
        muzzleFlash.Play();
        //gunAnimation.Play();

        //plays shoot AudioClip
        gunAudio.PlayOneShot(shootSound, 0.5f);

        LeanTween.rotateLocal(this.gameObject, new Vector3(recoilRotateX,180,0), .05f);
        LeanTween.moveLocalZ(this.gameObject, recoilPositionZ, .05f);

        CameraShaker.Instance.ShakeOnce(magnitudePower, 2.5f, .05f, shakeFadeOutTime);

        //each time you fire, you lose 1 bullet, also your amount of bullets fired increments by 1
        //ONLY if the player doesn't have infinite ammo, otherwise they won't lose ammo
        if (!PowerUpEvent.current.hasInfiniteAmmo)
        {
            current_mag_size--;
            bullets_fired = original_mag_size - current_mag_size;
        }
        //update ammo UI each time player shoots;
        PlayerUIAccessor.UpdatePlayerAmmoUI();

        

        RaycastHit hit;
        
        //Raycast begins from position of main camera, and goes in the forward direction (forward from camera) (also IGNORES the "Ragdoll" layer)
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward,out hit, range,bullet_applicable_layermasks,QueryTriggerInteraction.Ignore))
        {
            //displays what/who we shoot at
            //Debug.Log(hit.transform.name);

            //allows us to access functions from the target script (if the enemy or object contains the "Target" script
            target = hit.transform.GetComponent<Target>();
            //only allow damage to things that we find the target component for
            if(target != null)
            {
                //checks if we have instant kill on
                if (PowerUpEvent.current.hasInstantKill)
                {
                    target.TakeDamage(target.maxHealthAmount);
                }
                //passing through our damage as an argument to do that much damage to the enemy (WITHOUT INSTANT KILL)
                else
                {
                    target.TakeDamage(damage);
                }
                
                
            }
            
                //checks if target has a rigidbody
            if(hit.rigidbody != null)
            {
                hitRB = hit.rigidbody.gameObject.GetComponent<NavMeshAgent>();
                //check if target has a NavMeshAgent
                if (hitRB != null)
                {
                    //Debug.Log("navmesh OFF");
                    //hitRB.enabled = false;

                    //setting object to not being kinematic so they can be affected by impact force
                    hit.rigidbody.isKinematic = false;

                    //applying force to object with gun
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                    StartCoroutine(KnockbackDelay(hit));
                    
                }
                else
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            //instantiate our impact effect at the point of bullet impact
            if(hit.rigidbody != null)
            {
                GameObject impactGameObject = Instantiate(bloodImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGameObject, 2f);
            }
            else
            {
                GameObject impactGameObject = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGameObject, 2f);
            }
            //GameObject impactGameObject = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(impactGameObject, 2f);
        }
    }

    //we call this function when we pick up a MaxAmmo powerup, or if we purchase ammo from the Ammo Crate
    public void MaxAmmo()
    {
        //set the current magazine size to the original mag size, and set the current ammo capacity to original_ammoCapacity
        //need to reset bullets_fired otherwise we receive extra ammo when we reload and buy ammo at the same time
        current_mag_size = original_mag_size;
        ammoCapacity = original_ammoCapacity;
        bullets_fired = 0;
        PlayerUIAccessor.UpdatePlayerAmmoUI();

    }

    public void UpgradeGun()
    {
        damage *= upgradeDamageMultiplier;
        original_mag_size *= 2;
        original_ammoCapacity *= 2;

        current_mag_size = original_mag_size;
        ammoCapacity = original_ammoCapacity;
        bullets_fired = 0;
        PlayerUIAccessor.UpdatePlayerAmmoUI();

        ammoPrice *= 2;
        Debug.Log("Weapon upgrade me!");
        isUpgraded = true;

    }

    IEnumerator KnockbackDelay(RaycastHit hit)
    {
        //Debug.Log("start");
        yield return new WaitForSeconds(.4f);
        if(hitRB != null && !target.is_dead)
        {
            //hitRB.enabled = true;
            //set rigidbody back to being kinematic so player cannot push the object
            hit.rigidbody.isKinematic = true;

            //Debug.Log("navmesh ON");
        }
        
        //Debug.Log("end");
    }

    //void knockbackoff(RaycastHit hit)
    //{
        //hit.rigidbody.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        //hit.rigidbody.isKinematic = true;
    //}
    
    IEnumerator Reload()
    {

        isReloading = true;

        Debug.Log("reloading");
        animator.SetBool("Reloading", true);

        //delay for reloading time
        if (playerPerkAccessor.has_Reload_Speed_Perk)
        {
            yield return new WaitForSeconds(0.2f);
            gunAudio.PlayOneShot(reloadSound1, 0.3f);
            yield return new WaitForSeconds(timeBetweenReloadSounds/2);
            gunAudio.PlayOneShot(reloadSound2, 0.3f);
            yield return new WaitForSeconds(timeBetweenReloadSounds/2);
            gunAudio.PlayOneShot(reloadSound3, 0.3f);

            yield return new WaitForSeconds((reloadSpeed/2) - .25f);
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            gunAudio.PlayOneShot(reloadSound1, 0.3f);
            yield return new WaitForSeconds(timeBetweenReloadSounds);
            gunAudio.PlayOneShot(reloadSound2, 0.3f);
            yield return new WaitForSeconds(timeBetweenReloadSounds);
            gunAudio.PlayOneShot(reloadSound3, 0.3f);
            yield return new WaitForSeconds(reloadSpeed - .25f);
        }
        
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);



        if (ammoCapacity >= bullets_fired && ammoCapacity != 0)
        {
            ammoCapacity -= bullets_fired;
            current_mag_size += bullets_fired;
        }
        else if(ammoCapacity < bullets_fired && ammoCapacity != 0)
        {
            current_mag_size += ammoCapacity;
            ammoCapacity = 0;
        }
        else if(ammoCapacity == 0)
        {
            current_mag_size = current_mag_size;
        }
        //current_mag_size = original_mag_size;
        bullets_fired = 0;

        isReloading = false;

        //when player reloads, update ammo UI
        PlayerUIAccessor.UpdatePlayerAmmoUI();
    }
}
