using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePoints : MonoBehaviour
{
    public bool hasDoublePoints;
    public float doublePointsCountDown;
    public int id;
    public float deletionCountDown;
    private bool deletionCountDownHasStarted;

    // Start is called before the first frame update
    void Start()
    {
        
        hasDoublePoints = false;
        StartCoroutine(DeletePowerUpTimer());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasDoublePoints)
        {
            if (deletionCountDownHasStarted)
                StopCoroutine(nameof(DeletePowerUpTimer));

            //we subscribe when we triggerEnter instead of at Start() so we don't activate duplicates at the same time

            PowerUpEvent.current.onPowerUpAcquire += GiveDoublePoints;
            PowerUpEvent.current.PowerUpAcquirement(id);
            Debug.Log("Double Points power up event executed");

        }
    }
    void GiveDoublePoints(int id)
    {
        if(id == this.id && this.gameObject)
        {
            hasDoublePoints = true;
            StartCoroutine(DoublePointsTimer());
        }
        
    }
    IEnumerator DoublePointsTimer()
    {
        yield return new WaitForSeconds(doublePointsCountDown);
        hasDoublePoints = false;
        Debug.Log("Double points is over");
        Destroy(gameObject);
    }
    IEnumerator DeletePowerUpTimer()
    {
        deletionCountDownHasStarted = true;
        yield return new WaitForSeconds(deletionCountDown);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        PowerUpEvent.current.onPowerUpAcquire -= GiveDoublePoints;
    }
}