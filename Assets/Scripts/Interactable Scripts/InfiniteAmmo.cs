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
                StartCoroutine(CloseTextboxBegin());
                PowerUpEvent.current.onPowerUpAcquire += GiveInfiniteAmmo;
                PowerUpEvent.current.PowerUpAcquirement(id);
                //Debug.Log("Infinite Ammo power up event executed");
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
        StartCoroutine(ChangeTextbox());
        PowerUpEvent.current.hasInfiniteAmmo = true;


        yield return new WaitForSeconds(infiniteAmmoCountDown);
        PowerUpEvent.current.hasInfiniteAmmo = false;
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

    IEnumerator CloseTextboxBegin()
    {
        InteractionTextbox.current.ChangeTextBoxDescription("Infinite Ammo!");
        yield return new WaitForSeconds(1f);
        InteractionTextbox.current.CloseTextBox();
    }
    IEnumerator ChangeTextbox()
    {

        yield return new WaitForSeconds(infiniteAmmoCountDown - 2f);
        InteractionTextbox.current.ChangeTextBoxDescription("Infinite Ammo Expired!");
        yield return new WaitForSeconds(1f);
        InteractionTextbox.current.CloseTextBox();

    }
}

