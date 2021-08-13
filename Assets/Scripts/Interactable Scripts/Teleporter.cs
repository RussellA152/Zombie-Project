using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    private GameObject player;

    [SerializeField] private Transform powerRoom_teleporter_position;
    [SerializeField] private Transform startingRoom_teleporter_position;

    [SerializeField] private bool is_PowerRoom_Teleporter;
    [SerializeField] private bool is_StartingRoom_Teleporter;

    //private bool teleporter_Is_Linked;


    private void Start()
    {
        player = GameObject.Find("Player");
        //teleporter_Is_Linked = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && is_PowerRoom_Teleporter)
        {
            player.transform.position = startingRoom_teleporter_position.position;
        }
    }
}
