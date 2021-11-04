using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public static float pScore;
    // Start is called before the first frame update
    void Start()
    {
        pScore = 20000f;
    }
    private void Update()
    {
        //Debug.Log("My Score: " + pScore);
    }
}
