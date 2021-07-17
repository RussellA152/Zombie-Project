
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    //by default, 0 is the first weapon (Handgun)
    public int selectedWeapon = 0;

    //time it takes before you can swap weapons again
    private float swapDelay = 0.5f;
    private float nextTimeToSwap = 0f;

    private bool canSwap;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        //we set our "previousSelectedWeapon to our current weapon in hand
        int previousSelectedWeapon = selectedWeapon;

        //pressing the '1' key will equip first gun, '2' for second gun, '3' for third gun
        if ((Input.GetKeyDown(KeyCode.Alpha1) == true) && Time.time >= nextTimeToSwap)
        {
            selectedWeapon = 0;
            nextTimeToSwap = Time.time + swapDelay;
        }
        if ((Input.GetKeyDown(KeyCode.Alpha2) == true) && Time.time >= nextTimeToSwap)
        {
            selectedWeapon = 1;
            nextTimeToSwap = Time.time + swapDelay;
        }
        if ((Input.GetKeyDown(KeyCode.Alpha3) == true) && Time.time >= nextTimeToSwap)
        {
            selectedWeapon = 2;
            nextTimeToSwap = Time.time + swapDelay;
        }

        //scrolling up for swapping guns (goes to next weapon)  
        //we also check if we're able to swap weapons (weapon swap cooldown has settled)
        if ((Input.GetAxis("Mouse ScrollWheel") > 0f) && Time.time >= nextTimeToSwap)
        {
            nextTimeToSwap = Time.time + swapDelay;
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
        if ((Input.GetAxis("Mouse ScrollWheel") < 0f) && Time.time >= nextTimeToSwap)
        {
            nextTimeToSwap = Time.time + swapDelay;
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
            SelectWeapon();
        }

    }
    void SelectWeapon()
    {
        //we iterate through each child's index/position in the WeaponHolder, if the index equals our currently selected weapon, equip it, otherwise its not equipped
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
