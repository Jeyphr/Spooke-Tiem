using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDoohickey : MonoBehaviour
{
    private static InputDoohickey _instance;
    public static InputDoohickey Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (InputDoohickey)GameObject.FindAnyObjectByType(typeof(InputDoohickey));

                if (_instance == null)
                {
                    GameObject go = new GameObject("InputDookickey:Singleton");
                    _instance = go.AddComponent<InputDoohickey>();
                }

            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    #region Generic
    private void Awake()
    {
        //checkSingleton();
        Instance = this;
    }

    private void OnDisable()
    {
        Instance = null;
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
