using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreecam : MonoBehaviour
{
    //VARS
    [Header("Look Sensitivity")]
    [SerializeField] public float hSensitivity = 300f;
    [SerializeField] public float vSensitivity = 250f;

    [Header("Object References")]
    [SerializeField] public Transform playerBody;

    //PRIVATE VARS
    float xRots = 0f;
    private bool isLocked;

    public bool IsLocked { get => isLocked; set => isLocked = value; }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsLocked)
        {
            float mousex = Input.GetAxis("Mouse X") * Time.deltaTime * hSensitivity;
            float mousey = Input.GetAxis("Mouse Y") * Time.deltaTime * vSensitivity;

            playerBody.Rotate(Vector3.up * mousex);

            xRots -= mousey;
            xRots = Mathf.Clamp(xRots, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRots, 0f, 0f);
        }
    }
}
