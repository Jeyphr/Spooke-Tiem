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


    [Header("Object References")]
    [SerializeField] private Camera[]     arrCams;
    [SerializeField] private Camera       cam_player;
    [SerializeField] private Transform    lookatPos;
    public bool isDebugMode = false;

    //private vars
    private Camera cam_active;
    


    #region Base Methods
    private void Awake()
    {
        checkSingleton();

        setCurrentCamera(cam_player);

        //setvars
        arrCams = findAllCamsInScene();
        cam_active = Camera.main;
    }
    private void Update()
    {

        if (isPressingDebugKey(KeyCode.J))
        {
            swapCameras(cam_player);
            snapLook(lookatPos);
        }

        if (isPressingDebugKey(KeyCode.K))
        {
            resetLook(cam_active);
        }
    }
    #endregion

    #region Camera Manipulation
    public void swapCameras(Camera swapCam)
    {
        if (swapCam == null)
        {
            Debug.Log(swapCam.name + " is Null");
            return;
        }

        if(cam_active == swapCam)
        {
            Debug.Log(swapCam.name + " is Swap Cam");
            return;
        }

        foreach (var cam in arrCams)
        {
            Debug.Log("Disabling:\t" + cam.name);
            cam.enabled = false;
        }
        cam_active = swapCam;
        cam_active.enabled = true;
    }

    public void snapLook(Transform lookAtPos)                   //hard swaps between cameras. no lerp
    {
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
            Debug.Log(cam.name + " is null");
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
        if (cam == null || cam.enabled == false)
        {
            Debug.Log("Cam is either null or disabled.");
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

    #region Debug
    private bool isPressingDebugKey(KeyCode key)
    {
        if (Input.GetKeyDown(key) && isDebugMode)
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
