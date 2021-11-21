using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneHandler : MonoBehaviour
{
    public static MySceneHandler current;

    public GameObject animator_object;
    public Animator animator;
   
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
    private void Start()
    {
        //finds "Fade Animator" in the scene
        FindFadeAnimator();
    }

    public void ChangeScene(string scene)
    {
        //loads a new scene 
        StartCoroutine(FadeOutToLoad(scene));

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
        FindFadeAnimator();
        //when scene is done loading, turn off input for a few se
        InputManager.IsInputEnabled = false;
        yield return new WaitForSeconds(InputManager.InputTurnOnTimer);
        InputManager.IsInputEnabled = true;

       

    }
    IEnumerator FadeOutToLoad(string scene)
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2f);
        StartCoroutine(LoadYourAsyncScene(scene));

    }
    void FindFadeAnimator()
    {
        if (GameObject.Find("Fade Animator") != null)
            animator_object = GameObject.Find("Fade Animator");
        if (animator_object != null)
            animator = animator_object.GetComponent<Animator>();
    }

}
