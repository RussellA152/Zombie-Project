using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GivePlayerScoreEvent : MonoBehaviour
{
    public static GivePlayerScoreEvent current;

    private void Awake()
    {
        current = this;
    }

    public event Action GivePlayerScore;

    public void GiveScore()
    {
        if(GivePlayerScore != null)
        {
            GivePlayerScore();
        }
    }
}
