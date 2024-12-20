using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    #region Generic
    private void Awake()
    {
        checkSingleton();
    }
    #endregion

    #region Inputs and stuff
    public bool isPressingDebugKey(KeyCode key, bool isDebugMode)
    {
        if (Input.GetKeyDown(key) && isDebugMode)
            return true;
        return false;
    }

    public bool isPressingKey(KeyCode key)
    {
        if (Input.GetKeyDown(key))
            return true;
        return false;
    }
    #endregion

    #region Singleton Stuff
    private void checkSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion
}
