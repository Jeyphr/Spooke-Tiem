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
    public TerminalManager terminalManager;

    //vars
    public enum uiState
    {
        Gameplay,
        Paused,
        Terminal,
        Shop
    }
    public uiState  page;
    public Canvas   gamePlayScreen, pausedScreen, terminalScreen, shopScreen;
    public bool     isDebugMode = false;
    private bool    notOnGameplay = false;
    

    #region Generic
    private void Awake()
    {
        checkSingleton(); //THIS ONE GOES FIRST DUMBASS



        switchUIState(uiState.Terminal);
    }

    private void Start()
    {
        //Debug.Log("uimanageer: " + inputDookickey.name);
    }

    private void Update()
    {
        if (isPressingKey(KeyCode.Escape))
        {
            page = uiState.Paused;
            switchUIState(page);
        }
        if (isPressingKey(KeyCode.Slash))
        {
            page = uiState.Terminal;
            switchUIState(page);
        }
        if (isPressingKey(KeyCode.Tab))
        {
            page = uiState.Shop;
            switchUIState(page);
        }

        if (isPressingDebugKey(KeyCode.J, isDebugMode))
        {
            page = uiState.Paused;
            switchUIState(page);
        }
        if (isPressingDebugKey(KeyCode.K, isDebugMode))
        {
            page = uiState.Terminal;
            switchUIState(page);
        }
        if (isPressingDebugKey(KeyCode.L, isDebugMode))
        {
            page = uiState.Shop;
            switchUIState(page);
        }
    }
    #endregion

    #region UI Switcher
    public void switchUIState(uiState swappedUI)
    {
        /*
        if (swappedUI == page)
        {
            terminalManager.createNotif("UI Error");
            return;
        }
        */

        switch (page)
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

            case uiState.Shop:
                if (notOnGameplay)
                {
                    returnToGameplay();
                }
                else
                {
                    hideAllCanvases();
                    showCanvas(shopScreen);

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
        hideCanvas(shopScreen);
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
