using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundActivator : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        RoundChange.roundChange.RoundChanging();
    }
}
