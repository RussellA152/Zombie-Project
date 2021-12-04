using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class ControlsButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject return_button;
    [SerializeField] private Button button;

    [SerializeField] private GameObject control_text;
    [SerializeField] private GameObject main_camera;

    [SerializeField] private GameObject lamp_light_gameobject;

    [SerializeField] private Vector3 original_lamp_location;
    [SerializeField] private Vector3 controls_lamp_location;


    public void OnControlButtonClick()
    {
        LeanTween.rotate(main_camera, new Vector3(0, 30, 0), 1f);
        LeanTween.move(main_camera, new Vector3(-49.552f, -0.2f, -48.99f), 1f);
        LeanTween.move(lamp_light_gameobject, controls_lamp_location, 1f);
        StartCoroutine(ControlTextDelay());
    }

    IEnumerator ControlTextDelay()
    {
        yield return new WaitForSeconds(.5f);
        return_button.SetActive(true);
        button.interactable = true;
        control_text.SetActive(true);
    }
    IEnumerator ReturnTextDelay()
    {
        button.interactable = false;
        yield return new WaitForSeconds(1f);
        return_button.SetActive(false);
        control_text.SetActive(false);
    }

    public void OnReturnButtonClick()
    {
        LeanTween.rotate(main_camera, new Vector3(0, 0, 0), 1f);
        LeanTween.move(main_camera, new Vector3(-50f, -0.2f, -48.99f), 1f);
        LeanTween.move(lamp_light_gameobject, original_lamp_location, 1f);
        StartCoroutine(ReturnTextDelay());
    }
}
