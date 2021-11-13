using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePoints : MonoBehaviour
{
    public float doublePointsCountDown;
    public int id;

    [SerializeField] private float deletionCountDown;
    private bool deletionCountDownHasStarted;

    //bool that prevents player from grabbing same powerup twice
    private bool hasPowerUp;

    private Coroutine deletionCountDownCoroutine;

    [SerializeField] private AudioClip powerUpRetrievedSound;
    [SerializeField] private AudioClip powerUpFinishedSound;

    // Start is called before the first frame update
    void Start()
    {

        hasPowerUp = false;
        deletionCountDownCoroutine = StartCoroutine(DeletePowerUpTimer());
        //StartCoroutine(deletionCountDownCoroutine);
        //Debug.Log("Start deletion coroutine");
    }
    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!hasPowerUp)
            {
                StopCoroutine(deletionCountDownCoroutine);
                //Debug.Log("stop deletion coroutine");

                //we subscribe when we triggerEnter instead of at Start() so we don't activate duplicates at the same time

                PowerUpEvent.current.onPowerUpAcquire += GiveDoublePoints;
                PowerUpEvent.current.PowerUpAcquirement(id);
                Debug.Log("Double Points power up event executed " + this.name);
            }
            /*
            else if (PowerUpEvent.current.hasDoublePoints && !hasPowerUp)
            {
                StopCoroutine(deletionCountDownCoroutine);
                Debug.Log("stop deletion coroutine");

                //we subscribe when we triggerEnter instead of at Start() so we don't activate duplicates at the same time

                PowerUpEvent.current.onPowerUpAcquire += GiveDoublePoints;
                PowerUpEvent.current.PowerUpAcquirement(id);
                Debug.Log("Double Points power up event executed DUPE");
            }
            */
        }
            /*
        }
        if (other.gameObject.CompareTag("Player") && !PowerUpEvent.current.hasDoublePoints)
        {
            StopCoroutine(deletionCountDownCoroutine);
            Debug.Log("stop deletion coroutine");

            //we subscribe when we triggerEnter instead of at Start() so we don't activate duplicates at the same time
            
            PowerUpEvent.current.onPowerUpAcquire += GiveDoublePoints;
            PowerUpEvent.current.PowerUpAcquirement(id);
            Debug.Log("Double Points power up event executed");

        }
            */
    }
    void GiveDoublePoints(int id)
    {
        if(id == this.id && this.gameObject)
        {
            LeanTween.moveY(this.gameObject, 50, 7f);
            InteractAudioSource.current.PlayInteractClip(powerUpRetrievedSound, 0.5f);
            hasPowerUp = true;
            StartCoroutine(DoublePointsTimer());
        }
        
    }
    IEnumerator DoublePointsTimer()
    {
        while (PowerUpEvent.current.hasDoublePoints)
        {
            Debug.Log("DP WAIT UNTIL " + this.name);
            
            yield return new WaitUntil(() => PowerUpEvent.current.hasDoublePoints == false);

            Debug.Log("DP DONE WAIT " + this.name);
        }
        PowerUpEvent.current.hasDoublePoints = true;
        Debug.Log("STARTED DP TIMER " + this.name);

        yield return new WaitForSeconds(doublePointsCountDown);
        PowerUpEvent.current.hasDoublePoints = false;
        Debug.Log("Double points is over" + this.name);
        Destroy(gameObject);
    }
    IEnumerator DeletePowerUpTimer()
    {
        yield return new WaitForSeconds(deletionCountDown);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        //only play sound if player obtained the powerup
        if (hasPowerUp)
            InteractAudioSource.current.PlayInteractClip(powerUpFinishedSound, 0.5f);
        PowerUpEvent.current.onPowerUpAcquire -= GiveDoublePoints;
    }
}