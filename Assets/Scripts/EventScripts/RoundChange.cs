using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoundChange : MonoBehaviour
{
    public static RoundChange roundChange;

    public AudioClip round_starting_sound;
    public AudioClip round_ending_sound;

    private PlayerUI player_ui_accessor;
    [SerializeField] private GameObject pHud;
    private void Awake()
    {
        roundChange = this;
    }

    public event Action onRoundChange;
    private void Start()
    {
        pHud = GameObject.Find("Player's HUD");
        player_ui_accessor = pHud.GetComponent<PlayerUI>();
    }
    public void RoundChanging()
    {
        if(onRoundChange != null)
        {
            onRoundChange();
        }
    }
}
