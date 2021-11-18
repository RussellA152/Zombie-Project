using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI youDiedText;
    [SerializeField] private TextMeshProUGUI youLastedText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button retryButton;
    // Start is called before the first frame update
    void Start()
    {
        continueButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        youLastedText.gameObject.SetActive(false);
        youDiedText.gameObject.SetActive(true);
        youLastedText.text = "You Lasted \n" + RoundController.round + " Rounds!";
        StartCoroutine(ChangeText());
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator ChangeText()
    {
        yield return new WaitForSeconds(3.3f);
        youDiedText.gameObject.SetActive(false);
        youLastedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.3f);
        youLastedText.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
    }
}
