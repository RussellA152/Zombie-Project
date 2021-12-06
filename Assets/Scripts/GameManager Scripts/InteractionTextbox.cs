using TMPro;
using UnityEngine;

public class InteractionTextbox : MonoBehaviour
{
    public static InteractionTextbox current;

    public RectTransform textBox;
    public TextMeshProUGUI textboxDescription;


    private void Awake()
    {
        current = this;
    }
    private void Start()
    {
        //close the textbox initially on start of game
        CloseTextBox();
    }
    //this function takes in a string parameter from other scripts
    //the textbox will open and change the text to whatever text was passed into it 
    public void ChangeTextBoxDescription(string Text)
    {
        OpenTextBox();
        textboxDescription.text = Text;
    }

    //TextBox will open, so it is visible on player's hud
    public void OpenTextBox()
    {
        textBox.gameObject.SetActive(true);
    }
    //TextBox will close, so it is not visible on player's hud
    public void CloseTextBox()
    {
        textBox.gameObject.SetActive(false);
    }
}
