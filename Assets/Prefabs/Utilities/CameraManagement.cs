using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[DisallowMultipleComponent]
public class CameraManagement : MonoBehaviour
{
    /// <summary> 12/14/24
    /// This script changes the postion of the camera dependant on the page of the game.
    /// It also contains funcitons that allow the camera to swap between different transforms
    /// look at different transforms, etc
    /// </summary>

    //vars
    public static CameraManagement Instance { get; private set; }
    public TerminalManager terminalManager;


    [Header("Object References")]
    [SerializeField] private Camera[]   arrCams;
    [SerializeField] private Camera     cam_player, cam_start;
    [SerializeField] private Transform  lookatPos;

    [Header("Player Shenannigans")]
    [SerializeField] private PlayerMovement pMove;
    [SerializeField] private PlayerFreecam  pLook;

    public bool isDebugMode = false;
    private int cameraIndex = 0;

    //private vars
    private Camera cam_active;
    private TerminalManager tm;
    


    #region Base Methods
    private void Awake()
    {
        checkSingleton();
    }
    private void Start()
    {
        tm = TerminalManager.FindAnyObjectByType<TerminalManager>();
        arrCams = findAllCamsInScene();

        swapCameras(cam_start);
    }
    private void Update()
    {


        if (isPressingDebugKey(KeyCode.J, isDebugMode))
        {
            cameraIndex++;

            Debug.Log("Camera Index: " + cameraIndex);
            if (cameraIndex < arrCams.Length)
            {
                
                swapCameras(arrCams[cameraIndex]);
            }
            else 
            {
                cameraIndex = 0;
                swapCameras(arrCams[cameraIndex]);
            }

        }
    }
    #endregion

    #region Camera Manipulation
    public void swapCameras(Camera swapCam)
    {
        if (swapCam == null)
        {
            tm.createNotif(swapCam.name + " is Null.");
            return;
        }

        if(cam_active == swapCam)
        {
            tm.createNotif(swapCam.name + " is already the active camera.");
            return;
        }

        lockPlayer(swapCam);
        foreach (var cam in arrCams)
        {
            cam.enabled = false;
        }
        cam_active = swapCam;
        cam_active.enabled = true;
    }

    public void snapLook(Transform lookAtPos)                   //hard swaps between cameras. no lerp
    {
        if (lookatPos == null)
        {
            tm.createNotif(lookatPos.name + " is null.");
            return;
        }

        if (cam_active == null)
        {
            tm.createNotif(cam_active + " is null.");
            return;
        }
        cam_active.transform.LookAt(lookAtPos);
    }

    public void lerpLook(Transform lookAtPos, Transform endPos, float lerpDuration = 5f)            //lerps between two positions
    {
        #region checks
        if (lookatPos == null)
        {
            Debug.Log("Attempted to look at null transform");
            return;
        }

        if (cam_active == null)
        {
            Debug.Log("Attempted to make a null camera look");
            return;
        }

        if (endPos == null)
        {
            Debug.Log("endpos is null");
            return;
        }
        #endregion

        //setvars
        Transform   startPos = cam_active.transform;
        float       timeElapsed = lerpDuration;
        bool        isSwapping = false;
    }

    public void resetLook(Camera cam)                           //parent must have transform component for this to not cause errors
    {
        if (cam == null)
        {
            tm.createNotif(cam.name + " is null.");
            return;
        }

        cam.transform.rotation = GetComponentInParent<Transform>().rotation;
    }
    #endregion

    #region Find Cameras
    private Camera[] findAllCamsInScene()
    {
        return FindObjectsOfType<Camera>();
    }

    public void setCurrentCamera(Camera cam)
    {
        lockPlayer(cam);
        if (cam == null || cam.enabled == false)
        {
            tm.createNotif(cam.name + " is either disabled or null.");
            return;
        }
        cam_active = cam;
    }
    #endregion

    #region lerp
    IEnumerator LerpCamera(Transform lerpCam, Vector3 endPosition, float lerpDuration)
    {
        float timeElapsed = 0;
        Vector3 startPosition = lerpCam.position;

        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;
            lerpCam.position = Vector3.Lerp(startPosition, endPosition, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        lerpCam.position = endPosition;
    }
    #endregion

    #region Tests

    #endregion

    #region Locks
    private void lockPlayer(Camera cam)
    {
        pMove.IsFrozen = true;
        pLook.IsLocked = true;

        if (cam == cam_player)
        { 
            pMove.IsFrozen = false; 
            pLook.IsLocked = false; 
        }
        else 
        { 
            pMove.IsFrozen = true; 
            pLook.IsLocked = true;
        }
    }
    #endregion

    #region Debug
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
            Debug.Log("There exists another copy of Camera Management");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion
}
