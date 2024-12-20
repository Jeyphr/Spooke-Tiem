using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using System;

public class TerminalManager : MonoBehaviour
{
    /// <summary>
    /// Manages the whatever
    /// </summary>
    public static TerminalManager   Instance { get; private set; }
    private static UIManagement     uiManagement;
    private static InputManager     inputManager;


    //private vars
    [Header("Notification Prefabs")]
    [SerializeField] private GameObject notification_prefab;
    [SerializeField] private GameObject terminal_prefab;

    [Header("Object References")]
    [SerializeField] private InputField terminal_input;
    [SerializeField] private Sprite defaultImage;

    [Header("Statistics")]
    [SerializeField] private float cooldown;

    //vars
    private int noteCount = 0;

    #region Generic
    private void Awake()
    {
        checkSingleton();
        createNotif("It Worked!");
        createNotif("Your Mom!");
        createNotif("So Original!");
    }
    #endregion

    #region Pinger
    public void createNotif(string t = "Ping")
    {
        try
        {
            noteCount++;    

            GameObject      notif           = Instantiate(notification_prefab, terminal_prefab.transform);
            TextMeshProUGUI notifText       = notif.GetComponentInChildren<TextMeshProUGUI>();

            notif.name = "Note " + noteCount;
            notifText.text = t;
        }
        catch(NullReferenceException q)
        {
            Debug.LogError("NotifERR: " + q);
        }
    }
    #endregion

    #region Cooldowns and Stuff
    IEnumerator decay(float decayTimeInFrames, Image bar)
    {
        float temp = decayTimeInFrames;
        while (temp > 0)
        {
            yield return null;
        }
    }
    #endregion

    #region Inputs and Stuff

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
