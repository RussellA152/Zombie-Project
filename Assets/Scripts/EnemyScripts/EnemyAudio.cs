using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public static EnemyAudio current;

    public GameObject enemyAudioSourceGameObject;

    private void Awake()
    {
        current = this;
        enemyAudioSourceGameObject = this.gameObject;
    }
    private void Start()
    {
        
    }
}
