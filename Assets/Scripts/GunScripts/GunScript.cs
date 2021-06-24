using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;

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

    //reloading animation
    public Animator animator;

    //the impact particle from shooting at surfaces/enemies
    public GameObject impactEffect;

    public AudioSource shootSound;

    //reloading key
    [SerializeField] KeyCode reloadKey = KeyCode.R;

    //set to 0 by default to allow us to shoot at least once
    private float nextTimeToFire = 0f;

    private void Start()
    {
        current_mag_size = original_mag_size;
        bullets_fired = original_mag_size - current_mag_size;
    }
    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update()
    {
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
        shootSound.Play();
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
                if(hit.rigidbody.gameObject.GetComponent<NavMeshAgent>() != null)
                {
                    hit.rigidbody.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                    hit.rigidbody.isKinematic = false;
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                    StartCoroutine(KnockbackDelay(hit));
                    
                }
                else
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            //instantiate our impact effect at the point of bullet impact
            GameObject impactGameObject = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGameObject, 2f);
        }
    }

    IEnumerator KnockbackDelay(RaycastHit hit)
    {
        //Debug.Log("start");
        yield return new WaitForSeconds(.4f);
        hit.rigidbody.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        hit.rigidbody.isKinematic = true;
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
        yield return new WaitForSeconds(reloadSpeed -.25f);
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
    }
}
