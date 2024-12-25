using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private LayerMask groundLayer;

    [Header("Statistics")]
    [SerializeField] private float moveSpeed        = 10f;
    [SerializeField] private float speedMultiplier  = 1f;
    [SerializeField] private float jumpPower        = 2f;

    //hiddenVars
    private const float gravity = -9.82f;
    private bool isGrounded, isBonking;
    private bool isFrozen;
    private Vector3 velocity;

    public bool IsFrozen { get => isFrozen; set => isFrozen = value; }


    #region Generic
    void Update()
    {
        if (!IsFrozen)
        {
            handleMovement();
            handleWalking();
            handleJumping();
            handleGravity();
        }

    }
    #endregion

    #region Movement
    private void handleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move * speedMultiplier * Time.deltaTime);
    }

    private void handleWalking()
    {
        float _tempMult = 1f;

        if (Input.GetButtonDown("Walk"))    { _tempMult = 0.5f; Debug.Log("Walking!"); }
        if (Input.GetButtonUp("Walk"))      { _tempMult = 1f; }

        if (isGrounded)
        {
            _tempMult = moveSpeed * speedMultiplier;
        }
        else
        {
            _tempMult = 1f;
        }
    }

    private void handleJumping()
    {
        if (groundCheck() && Input.GetButton("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
        }
    }

    #endregion

    #region Gravity
    private void handleGravity()
    {
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // ensures that the player actually sticks to the ground
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
    }

    private bool groundCheck()
    {
        Transform t = characterController.transform;
        Vector3 dwn = t.TransformDirection(Vector3.down);

        Debug.DrawRay(t.position, dwn);

        if (Physics.Raycast(t.position, dwn, 2f))
        {
            return true;
        }
        return false;   
    }
    #endregion
}
