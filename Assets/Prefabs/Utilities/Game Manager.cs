using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private TerminalManager tm;

    [Header("Object References")]
    [SerializeField] GameObject[] pots;
    [SerializeField] GameObject wrongKey;
    [SerializeField] GameObject rightKey;

    [Header("Pots")]
    [SerializeField] int headAmmountPerPot  = 5;
    [SerializeField] int numberOfKeys       = 1;

    //refs
    

    //vars


    //const

    #region Generic
    private void Awake()
    {
        checkSingleton();
    }

    private void Start()
    {
        tm = TerminalManager.FindAnyObjectByType<TerminalManager>();

        fillPots();
    }
    #endregion

    #region Game Management
    private void fillPots()
    {
        #region catches
        if (wrongKey == null || rightKey == null)
        {
            tm.createNotif("Keys are unassigned!");
        }
        #endregion

        foreach (var p in pots)
        {
            #region catches
            if (p == null || !p.activeInHierarchy)
            {
                tm.createNotif(p.name + " is either null or not active.");
                break;
            }
            #endregion
            Debug.Log(p.name);
        }
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
