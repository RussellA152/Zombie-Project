using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneHandler : MonoBehaviour
{
    public static MySceneHandler current;

    private void Awake()
    {
        current = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void ChangeScene(string scene)
    {
        //loads a new scene 
        SceneManager.LoadScene(scene);
        PlayerInputOn();

    }
    public void QuiteGame()
    {
        //quits the game
        Application.Quit();
    }

    public void PlayerInputOn()
    {
        InputManager.IsInputEnabled = false;
        StartCoroutine(InputTurnOn());
    }
    IEnumerator InputTurnOn()
    {
        yield return new WaitForSeconds(InputManager.InputTurnOnTimer);
        InputManager.IsInputEnabled = true;
    }
}
