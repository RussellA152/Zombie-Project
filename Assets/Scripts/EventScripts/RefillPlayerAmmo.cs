using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RefillPlayerAmmo : MonoBehaviour
{
    public static RefillPlayerAmmo current;
    private void Awake()
    {
        current = this;
    }

    public event Action RefillAmmo;

    public void AmmoRefiller()
    {
        if (RefillAmmo != null)
        {
            RefillAmmo();
        }
    }
}
