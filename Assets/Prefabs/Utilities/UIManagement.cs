using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UIManagement : MonoBehaviour
{
    /// <summary> 12/15/24
    /// This script controls how the game shows and hides
    /// the various pause screens and hud elements
    /// </summary>
    public static UIManagement Instance { get; private set; }
    private TerminalManager tm;

    //vars
    public enum uiState
    {
        Gameplay,
        Paused,
        Terminal,
        Inventory
    }
    public uiState  page;
    public Canvas   gamePlayScreen, pausedScreen, terminalScreen, inventoryScreen;
    public bool     isDebugMode = false;
    private bool    notOnGameplay = false;
    

    #region Generic
    private void Awake()
    {
        checkSingleton(); //THIS ONE GOES FIRST DUMBASS


        page = uiState.Terminal;
        switchUIState();
    }

    private void Start()
    {
        tm = TerminalManager.FindAnyObjectByType<TerminalManager>();
    }

    private void Update()
    {
        if (isPressingKey(KeyCode.Escape))
        {
            switchUIState(uiState.Paused);
        }
        if (isPressingKey(KeyCode.Slash))
        {
            switchUIState(uiState.Terminal);
        }
        if (isPressingKey(KeyCode.Tab))
        {
            switchUIState(uiState.Inventory);
        }

        if (isPressingDebugKey(KeyCode.J, isDebugMode))
        {
            switchUIState(uiState.Paused);
        }
        if (isPressingDebugKey(KeyCode.K, isDebugMode))
        {
            switchUIState(uiState.Terminal);
        }
        if (isPressingDebugKey(KeyCode.L, isDebugMode))
        {
            switchUIState(uiState.Inventory);
        }
    }
    #endregion

    #region UI Switcher
    public void switchUIState(uiState state = uiState.Gameplay)
    {
        /*
        if (swappedUI == page)
        {
            terminalManager.createNotif("UI Error");
            return;
        }
        */

        switch (state)
        {
            case uiState.Gameplay:
                returnToGameplay();
                break;

            case uiState.Paused:
                if (notOnGameplay)
                {
                    returnToGameplay();
                }
                else
                {
                    hideAllCanvases();
                    showCanvas(pausedScreen);

                    Time.timeScale = 0;
                    notOnGameplay = true;
                }
                break;

            case uiState.Terminal:
                if (notOnGameplay)
                {
                    returnToGameplay();
                }
                else
                {
                    hideAllCanvases();
                    showCanvas(terminalScreen);

                    Time.timeScale = 0;
                    notOnGameplay = true;
                }
                break;

            case uiState.Inventory:
                if (notOnGameplay)
                {
                    returnToGameplay();
                }
                else
                {
                    hideAllCanvases();
                    showCanvas(inventoryScreen);

                    notOnGameplay = true;
                }
                break;
        }
    }

    private void returnToGameplay()
    {
        hideAllCanvases();
        showCanvas(gamePlayScreen);

        Time.timeScale = 1; 
        notOnGameplay = false;
        page = uiState.Gameplay;
    }
    #endregion

    #region Show and Hide Elements
    private void showCanvas(Canvas canvas)
    {
        canvas.enabled = true;
    }

    private void hideCanvas(Canvas canvas)
    {
        canvas.enabled = false;
    }

    private void hideAllCanvases()
    {
        hideCanvas(terminalScreen);
        hideCanvas(inventoryScreen);
        hideCanvas(pausedScreen);
        hideCanvas(gamePlayScreen);
    }

    #endregion

    #region Input
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
