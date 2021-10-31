using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamagedSound : MonoBehaviour
{
    public static EnemyDamagedSound current;

    public GameObject enemyDamagedAudioSourceGameObject;

    private void Awake()
    {
        current = this;
        enemyDamagedAudioSourceGameObject = this.gameObject;
    }
    private void Start()
    {

    }
}
