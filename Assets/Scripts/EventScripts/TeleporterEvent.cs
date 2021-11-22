using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TeleporterEvent : MonoBehaviour
{
    public static TeleporterEvent current;

    public GameObject player;
    public GameObject player_orientation;

    public bool teleporters_are_linked;
    public bool wants_to_link_teleporters;

    //public bool powerRoom_teleporter_canLink;

    public bool teleporters_can_be_linked;

    //amount of time it takes for teleporter to be relinked
    public int time_To_Link;
    //timer inside upgrade room until player is teleported back to starting room
    public int upgradeRoomTimer;

    public Transform powerRoom_Teleporter;
    public Transform startingRoom_Teleporter;

    public AudioSource interactive_audio_source;

    public AudioClip teleport_link_sound;
    public AudioClip teleport_successful_sound;
    public AudioClip teleport_failed_sound;
    public AudioClip teleport_return_sound;
    public AudioClip interact_successful_sound;

    private bool canInteract;


    private void Awake()
    {
        current = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        teleporters_can_be_linked = true;
        teleporters_are_linked = false;
        wants_to_link_teleporters = false;


    }

    public event Action onTeleport;

    public void TeleportPlayer()
    {
        if(onTeleport != null)
        {
            onTeleport();
        }
    }
}
