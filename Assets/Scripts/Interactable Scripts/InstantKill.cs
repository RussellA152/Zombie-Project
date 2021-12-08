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
    [SerializeField] private AudioClip powerUpRetrievedSound;
    [SerializeField] private AudioClip powerUpFinishedSound;

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
                StartCoroutine(ChangeTextbox());
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
            LeanTween.moveY(this.gameObject, 50, 7f);
            hasPowerUp = true;
            InteractAudioSource.current.PlayInteractClip(powerUpRetrievedSound, 0.5f);
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
        {
            InteractAudioSource.current.PlayInteractClip(powerUpFinishedSound, 0.5f);
        }
            
        PowerUpEvent.current.onPowerUpAcquire -= GiveInstantKill;
    }

    IEnumerator ChangeTextbox()
    {
        InteractionTextbox.current.ChangeTextBoxDescription("Instant Discharge!");
        yield return new WaitForSeconds(1f);
        InteractionTextbox.current.CloseTextBox();

    }
}
