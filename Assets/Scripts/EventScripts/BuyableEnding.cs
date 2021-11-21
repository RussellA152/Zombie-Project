using System.Collections;
using UnityEngine;

public class BuyableEnding : MonoBehaviour
{
    public static BuyableEnding current;
    public bool conditions_met;

    private void Awake()
    {
        current = this;
    }
    private void Start()
    {
        conditions_met = false; 
    }
    public void CompleteLevel()
    {
        if (conditions_met)
        {
            //MySceneHandler.current.ChangeScene("Victory Screen");
            StartCoroutine(CompletionDelay());
            Debug.Log("Go to victory screen here!");
        }
    }
    IEnumerator CompletionDelay()
    {
        //this is a small delay for better scene transitioning
        yield return new WaitForSeconds(2f);
        MySceneHandler.current.ChangeScene("Victory Screen");
    }
}
