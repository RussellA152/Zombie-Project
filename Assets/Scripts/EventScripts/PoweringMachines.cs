using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweringMachines : MonoBehaviour
{
    public GameObject[] PerkMachines;
    PerkMachine perkMachineAccessor;

    // Start is called before the first frame update
    void Start()
    {
        perkMachineAccessor = GetComponent<PerkMachine>();

        PowerEvent.OnPowered += TurnOnMachinePower;
    }
    private void OnDisable()
    {
        PowerEvent.OnPowered -= TurnOnMachinePower;
    }
    void TurnOnMachinePower()
    {
        foreach(GameObject machine in PerkMachines)
        {
            PerkMachine.machineIsPowered = true;
        }
    }
}
