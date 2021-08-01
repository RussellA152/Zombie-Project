using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperStrength : MonoBehaviour
{
    public int id;
    public bool hasSuperStrength;
    public float superStrengthCountDown;

    public float deletionCountDown;
    private bool deletionCountDownHasStarted;

    // Start is called before the first frame update
    void Start()
    {
        
        hasSuperStrength = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasSuperStrength)
        {
            //we subscribe when we triggerEnter instead of at Start() so we don't activate duplicates at the same time
            PowerUpEvent.current.onPowerUpAcquire += GiveSuperStrength;
            PowerUpEvent.current.PowerUpAcquirement(id);    
            Debug.Log("Super Strength power up event executed");

        }
    }
    void GiveSuperStrength(int id)
    {
        if(id == this.id && this.gameObject)
        {
            hasSuperStrength = true;
            StartCoroutine(SuperStrengthTimer());
        }
    }
    IEnumerator SuperStrengthTimer()
    {
        yield return new WaitForSeconds(superStrengthCountDown);
        hasSuperStrength = false;
        Debug.Log("Super Strength is over");
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        PowerUpEvent.current.onPowerUpAcquire -= GiveSuperStrength;
    }
}