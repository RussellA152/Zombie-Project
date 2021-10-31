using System;
using UnityEngine;

public class BuyableEnding : MonoBehaviour
{
    public static BuyableEnding current;
    public bool conditions_met;

    private void Awake()
    {
        current = this;
    }
    private void Start()
    {
        conditions_met = false; 
    }
    public void CompleteLevel()
    {
        if (conditions_met)
        {
            Debug.Log("Go to victory screen here!");
        }
    }
}
