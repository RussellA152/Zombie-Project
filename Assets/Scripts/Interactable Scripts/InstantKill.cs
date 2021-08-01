using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantKill : MonoBehaviour
{
    public bool hasInstantKill;
    public float instantKillCountDown;
    public int id;

    public float deletionCountDown;
    private bool deletionCountDownHasStarted;

    // Start is called before the first frame update
    void Start()
    {
        
        hasInstantKill = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasInstantKill)
        {
            //we subscribe when we triggerEnter instead of at Start() so we don't activate duplicates at the same time
            PowerUpEvent.current.onPowerUpAcquire += GiveInstantKill;
            PowerUpEvent.current.PowerUpAcquirement(id);
            Debug.Log(" instant kill power up event executed");

        }
    }
    void GiveInstantKill(int id)
    {
        if(id == this.id && this.gameObject)
        {
            hasInstantKill = true;
            StartCoroutine(InstantKillTimer());
        }
        
    }
    IEnumerator InstantKillTimer()
    {
        yield return new WaitForSeconds(instantKillCountDown);
        hasInstantKill = false;
        Debug.Log("Instant kill is over");
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        PowerUpEvent.current.onPowerUpAcquire -= GiveInstantKill;
    }
}
