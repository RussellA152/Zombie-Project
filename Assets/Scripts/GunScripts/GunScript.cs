using System.Collections;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    //values for damage, range, firerate, etc. of current weapon (each weapon should have different values)
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 100f;
    public float fireRate = 0.5f;


    public int original_mag_size;
    public int current_mag_size;
    public int ammoCapacity;
    public int bullets_fired;

    public float reloadSpeed;
    private bool isReloading = false;

    //referencing our in-game camera
    public Camera fpsCam;

    //reference to a particle System (like muzzle flashes)
    public ParticleSystem muzzleFlash;

    //shooting animation for gun
    public Animation gunAnimation;

    //the impact particle from shooting at surfaces/enemies
    public GameObject impactEffect;

    //reloading key
    [SerializeField] KeyCode reloadKey = KeyCode.R;

    //set to 0 by default to allow us to shoot at least once
    private float nextTimeToFire = 0f;

    private void Start()
    {
        current_mag_size = original_mag_size;
        bullets_fired = original_mag_size - current_mag_size;
    }

    // Update is called once per frame
    void Update()
    {
        //if player is completely out of ammo, they cannot reload or shoot 
        if(ammoCapacity == 0 && current_mag_size == 0)
        {
            return;
        }
        //you can't shoot if you're reloading
        if (isReloading)
        {
            return;
        }
        //if your magazine is empty, you must reload
        if (current_mag_size <= 0f)
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
        //pressing left mouse will shoot
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            //higher firerate values allow player to shoot faster
            //essentially, we are setting nextTimeToFire equal to the sum of Time.time and 1/(firerate) 
            //if the Time.time is 5 seconds, and our nextTimeToFire is 5.25, we need to wait .25 seconds to shoot again
            nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
        }
    }
    void Shoot()
    {
        //plays muzzleFlash
        //muzzleFlash.Play();
        //gunAnimation.Play();

        //each time you fire, you lose 1 bullet, also your amount of bullets fired increments by 1
        current_mag_size--;
        bullets_fired = original_mag_size - current_mag_size;

        RaycastHit hit;

        //Raycast begins from position of main camera, and goes in the forward direction (forward from camera)
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward,out hit, range))
        {
            //displays what/who we shoot at
            //Debug.Log(hit.transform.name);

            //allows us to access functions from the target script (if the enemy or object contains the "Target" script
            Target target = hit.transform.GetComponent<Target>();
            //only allow damage to things that we find the target component for
            if(target != null)
            {
                //passing through our damage as an argument to do that much damage to the enemy
                target.TakeDamage(damage);
            }

            
                //checks if target has a rigidbody
            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            //instantiate our impact effect at the point of bullet impact
            GameObject impactGameObject = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGameObject, 2f);
        }
    }
    
    IEnumerator Reload()
    {

        isReloading = true;

        Debug.Log("reloading");

        //delay for reloading time
        yield return new WaitForSeconds(reloadSpeed);

        
        if(ammoCapacity >= bullets_fired && ammoCapacity != 0)
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
    }
}
