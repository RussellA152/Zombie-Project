using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public static float pScore;
    private float money_regen_rate;
    // Start is called before the first frame update
    void Start()
    {
        money_regen_rate = 1.3f;
        pScore = 11200f;
    }
    private void Update()
    {
        //player's money will grow/regen overtime as to prevent player from being stuck without ammo and money
        RegenMoney();
    }
    void RegenMoney()
    {
        if(InputManager.IsInputEnabled)
            pScore += money_regen_rate * Time.deltaTime;
        
    }
}
