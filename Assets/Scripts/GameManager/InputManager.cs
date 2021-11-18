
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static bool IsInputEnabled;
    public static float InputTurnOnTimer;
    private static InputManager current;

    // Start is called before the first frame update


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
    void Start()
    {
        //initially false, but will be set to true a few seconds after loading main game scene
        IsInputEnabled = false;
        InputTurnOnTimer = 5f;
    }

}
