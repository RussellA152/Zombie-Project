using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuMonitor : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI downsizerText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    // Start is called before the first frame update
    void Start()
    {
        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        if(downsizerText != null)
            downsizerText.gameObject.SetActive(true);
        StartCoroutine(ChangeText());
    }

    private IEnumerator ChangeText()
    {
        yield return new WaitForSeconds(3.6f);
        if (downsizerText != null)
            downsizerText.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }
}
