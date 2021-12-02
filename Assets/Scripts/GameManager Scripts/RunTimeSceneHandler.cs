
using UnityEngine;

public class RunTimeSceneHandler : MonoBehaviour
{
    //This script allows the scenemanager to be dragged into onClick() properties during runtime
    //I cannot drag in dontdestroyonload objects in the editor, so this script will allow me to do this in runtime instead

    [SerializeField] GameObject object_to_add;
    [SerializeField] string scene_name;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        if(MySceneHandler.current.gameObject != null)
            object_to_add = MySceneHandler.current.gameObject;
        else
        {
            object_to_add = GameObject.Find("MySceneHandler");
        }

    }

    // Update is called once per frame
    public void CallSceneManager()
    {
        MySceneHandler.current.ChangeScene(scene_name);

    }
    public void QuitGameRunTime()
    {
        MySceneHandler.current.QuiteGame();
    }
}
