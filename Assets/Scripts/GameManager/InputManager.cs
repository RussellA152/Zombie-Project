
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static bool IsInputEnabled;
    public static float InputTurnOnTimer;

    // Start is called before the first frame update
    void Start()
    {
        //initially false, but will be set to true a few seconds after loading main game scene
        IsInputEnabled = false;
        InputTurnOnTimer = 10f;
    }

}
