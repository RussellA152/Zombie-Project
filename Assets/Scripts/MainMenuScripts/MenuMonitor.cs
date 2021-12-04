using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuMonitor : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI downsizerText;
    [SerializeField] private TextMeshProUGUI controlsText;    //this is the text on the right monitor
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button returnButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        if (returnButton.gameObject != null)
            returnButton.gameObject.SetActive(false);

        if (controlsButton.gameObject != null)
            controlsButton.gameObject.SetActive(false);
        if (controlsText != null)
            controlsText.gameObject.SetActive(false);
        if(downsizerText != null)
            downsizerText.gameObject.SetActive(true);
        StartCoroutine(ChangeText());
    }

    private IEnumerator ChangeText()
    {
        yield return new WaitForSeconds(3.6f);
        if (downsizerText != null)
            downsizerText.gameObject.SetActive(false);
        if (controlsButton.gameObject != null)
            controlsButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }
}
