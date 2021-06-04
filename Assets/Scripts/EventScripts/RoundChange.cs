using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoundChange : MonoBehaviour
{
    public static RoundChange roundChange;

    private void Awake()
    {
        roundChange = this;
    }

    public event Action onRoundChange;

    public void RoundChanging()
    {
        if(onRoundChange != null)
        {
            onRoundChange();
        }
    }
}
