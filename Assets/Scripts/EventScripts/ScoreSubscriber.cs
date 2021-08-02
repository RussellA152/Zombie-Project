using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSubscriber : MonoBehaviour
{
    public GameObject zombiePrefab;
    private Target TargetAccess;

    public GameObject DoublePointsPowerup;
    private DoublePoints DoublePointsAccess;

    private float scoreAmount;

    // Start is called before the first frame update
    void Start()
    {
        GivePlayerScoreEvent.current.GivePlayerScore += EarnScore;
        TargetAccess = zombiePrefab.GetComponent<Target>();

        scoreAmount = TargetAccess.pointsForDeath;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(PowerUpEvent.current.hasDoublePoints);
    }
    public void EarnScore()
    {
        if (PowerUpEvent.current.hasDoublePoints)
        {
            PlayerScore.pScore = PlayerScore.pScore +  (scoreAmount * 2f);
            Debug.Log("Give double points");
        }
        else
        {
            PlayerScore.pScore += scoreAmount;
            Debug.Log("Give normal points");
        }
    }
}
