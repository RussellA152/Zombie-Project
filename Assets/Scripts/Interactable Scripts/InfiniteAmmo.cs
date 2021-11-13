using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteAmmo : MonoBehaviour
{
    [SerializeField] private float infiniteAmmoCountDown;
    public int id;

    [SerializeField] private float deletionCountDown;
    private bool deletionCountDownHasStarted;
    private bool hasPowerUp;

    private Coroutine deletionCountDownCoroutine;

    [SerializeField] private AudioClip powerUpRetrievedSound;
    [SerializeField] private AudioClip powerUpFinishedSound;

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
                PowerUpEvent.current.onPowerUpAcquire += GiveInfiniteAmmo;
                PowerUpEvent.current.PowerUpAcquirement(id);
                Debug.Log("Infinite Ammo power up event executed");
            }
        }
    }
    void GiveInfiniteAmmo(int id)
    {
        if (id == this.id && this.gameObject)
        {
            LeanTween.moveY(this.gameObject, 50, 7f);
            InteractAudioSource.current.PlayInteractClip(powerUpRetrievedSound, 0.5f);
            hasPowerUp = true;
            
            StartCoroutine(InfiniteAmmoTimer());
        }

    }
    IEnumerator InfiniteAmmoTimer()
    {
        //wait until the ongoing infinite ammo is over to trigger a new infinite ammo
        while (PowerUpEvent.current.hasInfiniteAmmo)
        {
            yield return new WaitUntil(() => PowerUpEvent.current.hasInfiniteAmmo == false);
        }
        PowerUpEvent.current.hasInfiniteAmmo = true;


        yield return new WaitForSeconds(infiniteAmmoCountDown);
        PowerUpEvent.current.hasInfiniteAmmo = false;
        Debug.Log("Infinite Ammo is over");
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
        PowerUpEvent.current.onPowerUpAcquire -= GiveInfiniteAmmo;
    }
}

