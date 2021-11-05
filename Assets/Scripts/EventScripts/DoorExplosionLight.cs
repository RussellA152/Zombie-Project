using System.Collections;
using UnityEngine;

public class DoorExplosionLight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DisableThisLight()
    {
        StartCoroutine(DisableThisLightCoroutine());
    }
    IEnumerator DisableThisLightCoroutine()
    {
        yield return new WaitForSeconds(.63f);
        this.gameObject.SetActive(false);
    }
}
