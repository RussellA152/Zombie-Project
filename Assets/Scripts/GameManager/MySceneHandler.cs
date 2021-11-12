using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneHandler : MonoBehaviour
{
    public static MySceneHandler current;

    private void Awake()
    {

        if (current != this)
        {
            if (current != null)
            {
                Destroy(current.gameObject);
            }
            DontDestroyOnLoad(gameObject);
            current = this;
        }

    }

    public void ChangeScene(string scene)
    {
        //loads a new scene 
        StartCoroutine(LoadYourAsyncScene(scene));

    }
    public void QuiteGame()
    {
        //quits the game
        Application.Quit();
    }
    IEnumerator LoadYourAsyncScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        //wait until the scene is done loading...
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        //when scene is done loading, turn off input for a few se
        yield return new WaitForSeconds(InputManager.InputTurnOnTimer);
        InputManager.IsInputEnabled = true;

    }

}
