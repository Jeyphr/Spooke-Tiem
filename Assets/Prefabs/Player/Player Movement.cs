using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera playerCam, nmeCam;
    [SerializeField] private LayerMask groundLayer;

    [Header("Statistics")]
    [SerializeField] private float moveSpeed       ;
    [SerializeField] private float speedMultiplier ;
    [SerializeField] private float jumpPower       ;

    //hiddenVars
    private CameraManagement cm;

    private const float gravity = -9.82f;
    private const float interactRange = 4f;
    private bool isFrozen, isOnCams;
    private Vector3 velocity;

    public bool IsFrozen { get => isFrozen; set => isFrozen = value; }


    #region Generic
    void Update()
    {
        handleCameraSwitching();
        handleGravity();
        if (!IsFrozen)
        {
            handleMovement();
            handleJumping();
            handleInteract();
        }

    }

    private void Start()
    {
        cm = FindAnyObjectByType<CameraManagement>();
    }
    #endregion

    #region Movement
    private void handleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * (handleWalking()) * Time.deltaTime);
    }

    private float handleWalking()
    {
        float _tempMult = 1f;

        if (Input.GetButtonDown("Walk"))    { _tempMult = 0.5f; Debug.Log("Walking!"); }
        if (Input.GetButtonUp("Walk"))      { _tempMult = 1f; }

        if (groundCheck())
        {
            _tempMult = moveSpeed * speedMultiplier;
        }
        else
        {
            _tempMult = moveSpeed;
        }
        return _tempMult;
    }

    private void handleJumping()
    {
        if (groundCheck() && Input.GetButton("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
        }
    }

    private void handleCameraSwitching()
    {
        if (Input.GetButtonDown("Cams"))
        {
            if (!isOnCams)
            {
                cm.swapCameras(nmeCam);
                isOnCams = true;
            }
            else
            {
                cm.swapCameras(playerCam);
                isOnCams = false;
            }
        }
    }

    private void handleInteract()
    {
        Transform t = playerCam.transform;
        Vector3 fwd = t.TransformDirection(Vector3.forward);

        Debug.DrawRay(t.position, fwd);

        if (Input.GetButtonDown("Interact"))
        {
            if (Physics.Raycast(t.position, fwd, interactRange))
            {
                Debug.Log("Got it.");
            }
        }
    }

    #endregion

    #region Gravity
    private void handleGravity()
    {
        if(groundCheck() && velocity.y < 0)
        {
            velocity.y = -2f; // ensures that the player actually sticks to the ground
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    private bool groundCheck()
    {
        Transform t = controller.transform;
        Vector3 dwn = t.TransformDirection(Vector3.down);

        Debug.DrawRay(t.position, dwn);

        if (Physics.Raycast(t.position, dwn, 1.1f))
        {
            return true;
        }
        return false;   
    }
    #endregion
}
