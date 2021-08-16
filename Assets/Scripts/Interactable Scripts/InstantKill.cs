using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantKill : MonoBehaviour
{
    [SerializeField] private float instantKillCountDown;
    public int id;

    [SerializeField] private float deletionCountDown;
    private bool deletionCountDownHasStarted;
    private bool hasPowerUp;

    private Coroutine deletionCountDownCoroutine;

    // Start is called before the first frame update
    void Start()
    {

        hasPowerUp = false;
        deletionCountDownCoroutine = StartCoroutine(DeletePowerUpTimer());

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!hasPowerUp)
            {
                StopCoroutine(deletionCountDownCoroutine);
                PowerUpEvent.current.onPowerUpAcquire += GiveInstantKill;
                PowerUpEvent.current.PowerUpAcquirement(id);
                Debug.Log(" instant kill power up event executed");
            }
        }

        /*

        if (other.gameObject.CompareTag("Player") && !PowerUpEvent.current.hasInstantKill)
        {
            //we subscribe when we triggerEnter instead of at Start() so we don't activate duplicates at the same time
            PowerUpEvent.current.onPowerUpAcquire += GiveInstantKill;
            PowerUpEvent.current.PowerUpAcquirement(id);
            Debug.Log(" instant kill power up event executed");

        }
        */
    }
    void GiveInstantKill(int id)
    {
        if(id == this.id && this.gameObject)
        {
            hasPowerUp = true;
            //PowerUpEvent.current.hasInstantKill = true;
            StartCoroutine(InstantKillTimer());
        }
        
    }
    IEnumerator InstantKillTimer()
    {
        //wait until the ongoing instant kill is over to trigger a new instant kill
        while (PowerUpEvent.current.hasInstantKill)
        {
            yield return new WaitUntil(() => PowerUpEvent.current.hasInstantKill == false);
        }
        PowerUpEvent.current.hasInstantKill = true;


        yield return new WaitForSeconds(instantKillCountDown);
        PowerUpEvent.current.hasInstantKill = false;
        Debug.Log("Instant kill is over");
        Destroy(gameObject);
    }

    IEnumerator DeletePowerUpTimer()
    {
        yield return new WaitForSeconds(deletionCountDown);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        PowerUpEvent.current.onPowerUpAcquire -= GiveInstantKill;
    }
}
