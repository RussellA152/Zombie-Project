using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreePoints : MonoBehaviour
{
    private bool gotFreePoints;

    public int id;

    [SerializeField] private float deletionCountDown;
    [SerializeField] private float amount_of_points_given;
    private bool deletionCountDownHasStarted;

    private Coroutine deletionCountDownCoroutine;

    [SerializeField] private AudioClip powerUpRetrievedSound;

    // Start is called before the first frame update
    void Start()
    {
        gotFreePoints = false;

        deletionCountDownCoroutine = StartCoroutine(DeletePowerUpTimer());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !gotFreePoints)
        {
            //we subscribe when we triggerEnter instead of at Start() so we don't activate duplicates at the same time
            PowerUpEvent.current.onPowerUpAcquire += FreePointsSubscriber;
            PowerUpEvent.current.PowerUpAcquirement(id);
            Debug.Log("Free Points power up event executed");

        }
    }
    void FreePointsSubscriber(int id)
    {
        if (id == this.id && this.gameObject)
        {
            InteractAudioSource.current.PlayInteractClip(powerUpRetrievedSound, 0.5f);
            FreePointsGiver();
            gotFreePoints = true;
            Destroy(gameObject);
        }
    }

    void FreePointsGiver()
    {
        PlayerScore.pScore += 1000f;
    }

    //makes free points power up delete after some time
    IEnumerator DeletePowerUpTimer()
    {
        yield return new WaitForSeconds(deletionCountDown);
        Destroy(gameObject);

    }
    private void OnDestroy()
    {
        //not sure if we need to stop coroutine since it gets destroyed but this is here just in case
        StopCoroutine(deletionCountDownCoroutine);
        PowerUpEvent.current.onPowerUpAcquire -= FreePointsSubscriber;
    }
}
