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
        PowerUpEvent.current.onPowerUpAcquire -= GiveInfiniteAmmo;
    }
}

