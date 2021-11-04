using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI youDiedText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button retryButton;
    // Start is called before the first frame update
    void Start()
    {
        continueButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        youDiedText.gameObject.SetActive(true);
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
        continueButton.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
    }
}
