using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSway : MonoBehaviour
{
    
    //intensity of the swaying
    public float swayIntensity;

    //variable that determines how quick the gun returns to its original position
    public float swaySmooth;

    private Quaternion original_rotation;

    private void Start()
    {
        original_rotation = transform.localRotation;
        
    }
    private void Update()
    {
        UpdateSway();
    }

    private void UpdateSway()
    {
        //mouse controls for sway
        float t_x_mouse = Mathf.Clamp(Input.GetAxisRaw("Mouse X"), -1, 1);
        float t_y_mouse = Mathf.Clamp(Input.GetAxisRaw("Mouse Y"),-1,1);

        //calculates target rotation
        Quaternion t_x_adjustment = Quaternion.AngleAxis(-swayIntensity * t_x_mouse, Vector3.up);
        Quaternion t_y_adjustment = Quaternion.AngleAxis(-swayIntensity * t_y_mouse, Vector3.right);

        Quaternion target_rotation = original_rotation * t_x_adjustment * t_y_adjustment;

        
        
        //rotate towards target rotation
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, target_rotation, swaySmooth * Time.deltaTime);
    }

}
