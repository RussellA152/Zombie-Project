using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteraction : MonoBehaviour
{
    public Button this_button;
    private float buttonReactivateDelay = 4f;

    private void Start()
    {
        this_button.interactable = true;

    }

    //this function is assigned as the OnClick listener from the inspector
    public void WhenClicked()
    {
        this_button.interactable = false;

        //when player clicks on a menu button, they will have to wait a few seconds until they can click the button again
        StartCoroutine(EnableButtonAfterDelay(this_button, buttonReactivateDelay));
    }

    IEnumerator EnableButtonAfterDelay(Button button, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        button.interactable = true;
    }



}
