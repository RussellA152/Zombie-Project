using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;

    

    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    float xRotation;
    float yRotation;


    // Start is called before the first frame update
    void Start()
    {
        

        //hides cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.IsInputEnabled)
        {
            MyInput();
        }
        

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    void MyInput()
    {
        //gets mouse input from user

        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        //allows rotation of camera
        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        //makes sure player has a minimum and maximum rotation of camera (cant look too high up or too low down)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
