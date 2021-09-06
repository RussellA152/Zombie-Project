using System.Collections;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    //by default, 0 is the first weapon (Handgun)
    public int selectedWeapon = 0;
    public Transform equippedWeapon;

    //time it takes before you can swap weapons again
    //private float swapDelay = 0.2f;
    //private float nextTimeToSwap = 0f;

    [SerializeField] private float swapTime;

    private bool canSwap;

    public int maxWeaponInventorySize;
    public int currentWeaponInventorySize;

    public int previousSelectedWeapon;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
        maxWeaponInventorySize = 2;
        currentWeaponInventorySize = transform.childCount;
        //Debug.Log(currentWeaponInventorySize);
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(equippedWeapon);

        //we set our "previousSelectedWeapon to our current weapon in hand
        previousSelectedWeapon = selectedWeapon;

        //pressing the '1' key will equip first gun, '2' for second gun, '3' for third gun
        if ((Input.GetKeyDown(KeyCode.Alpha1) == true))
        {
            selectedWeapon = 0;
            //nextTimeToSwap = Time.time + swapDelay;
        }
        if ((Input.GetKeyDown(KeyCode.Alpha2) == true) && currentWeaponInventorySize > 1)
        {
            selectedWeapon = 1;
            //nextTimeToSwap = Time.time + swapDelay;
        }

        /*
        if ((Input.GetKeyDown(KeyCode.Alpha3) == true) && Time.time >= nextTimeToSwap)
        {
            selectedWeapon = 2;
            nextTimeToSwap = Time.time + swapDelay;
        }
        */

        //scrolling up for swapping guns (goes to next weapon)  
        //we also check if we're able to swap weapons (weapon swap cooldown has settled)
        if ((Input.GetAxis("Mouse ScrollWheel") > 0f))
        {
            //nextTimeToSwap = Time.time + swapDelay;
            //if we try to scroll past the amount of weapons we have, the index will reset
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon--;


            }
            

        }
        //scrolling down for swapping guns (goes to previous weapon)
        //we also check if we're able to swap weapons (weapon swap cooldown has settled)
        if ((Input.GetAxis("Mouse ScrollWheel") < 0f))
        {
            //nextTimeToSwap = Time.time + swapDelay;
            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;

            }

        }
        //after swapping weapons we check if our previousSelectedWeapon does NOT equal our current weapon (we changed weapons)
        if (previousSelectedWeapon != selectedWeapon)
        {
            //if we changed weapons, then call the SelectWeapon function to set new guns active, otherwise do nothing
            StartCoroutine(WeaponSwapDelay());
            

        }

    }
    public void SelectWeapon()
    {
        //we iterate through each child's index/position in the WeaponHolder, if the index equals our currently selected weapon, equip it, otherwise its not equipped
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                equippedWeapon = weapon;
            }
                

            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

    IEnumerator WeaponSwapDelay()
    {
        animator.SetBool("Swapping", true);
        yield return new WaitForSeconds(swapTime -.25f);
        SelectWeapon();
        animator.SetBool("Swapping", false);
        yield return new WaitForSeconds(.25f);

    }

}
