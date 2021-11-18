using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI youEscapedText;
    [SerializeField] private TextMeshProUGUI youSurvivedText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button replayButton;
    // Start is called before the first frame update
    void Start()
    {
        continueButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        youSurvivedText.gameObject.SetActive(false);
        youEscapedText.gameObject.SetActive(true);
        youSurvivedText.text = "You Survived \n" + RoundController.round + " Rounds!";
        StartCoroutine(ChangeText());
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator ChangeText()
    {
        yield return new WaitForSeconds(3.3f);
        youEscapedText.gameObject.SetActive(false);
        youSurvivedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.3f);
        youSurvivedText.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(true);
    }
}
