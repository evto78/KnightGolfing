using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEWPlayerMovement : MonoBehaviour
{
    [Header("Camera")]
    public Transform camHolder;
    public float sensitivity;
    float yaw = 0.0f;
    float pitch = 0.0f;
    public float fov;
    float camDistance;
    Vector3 initialCamPos;
    public Vector3 camOffset; public Vector3 actingCamOffset;
    public Camera mainCam;
    public Camera uiCam;
    public bool spectatingBall;
    public Camera spectatingCamera;
    public Vector2 minMaxSpectatingFOV;

    [Header("Movement")]
    Vector3 inputDir;
    public Rigidbody rb;
    public float baseWalkSpeed; float walkSpeed;
    public float sprintingModifier;
    public float airControlSpeed;
    public float baseJumpHeight; float jumpHeight;
    public float friction;
    public bool onGround;
    public bool sliding;
    public bool sprinting;
    public CapsuleCollider myCollider; float initialHeight;
    public int numOfJumps;
    int jumpsLeft; float timeSinceLastJump = 1f;
    public float additionalGravity;
    KeybindData keybinds;

    // Start is called before the first frame update
    void Start()
    {
        keybinds = new KeybindData();
        keybinds.ResetKeybinds();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach (Camera cam in camHolder.GetComponentsInChildren<Camera>()) { cam.fieldOfView = fov; }
        initialCamPos = camHolder.localPosition;
        camDistance = Mathf.Abs(camHolder.GetChild(0).localPosition.z);
        actingCamOffset = camOffset;
        camHolder.localPosition = initialCamPos + actingCamOffset;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
        GetInput();
    }

    void CameraMove()
    {
        camHolder.localPosition = initialCamPos + actingCamOffset;

        Ray ray = new Ray();
        ray.origin = camHolder.position;
        ray.direction = (camHolder.GetChild(0).position - camHolder.position).normalized;
        float newDist = camDistance;
        if (Physics.Raycast(ray, out RaycastHit hit, camDistance)) { newDist = hit.distance; }
        camHolder.GetChild(0).localPosition = -Vector3.forward * newDist;
        Debug.DrawRay(camHolder.position, (camHolder.GetChild(0).position - camHolder.position).normalized * newDist);

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            //get mouse input
            yaw += sensitivity * Input.GetAxis("Mouse X");
            pitch -= sensitivity * Input.GetAxis("Mouse Y");

            //limit cam angle
            pitch = Mathf.Clamp(pitch, -60.0f, 85.0f);

            //set cam angle
            camHolder.eulerAngles = new Vector3(pitch, yaw, camHolder.eulerAngles.z);
        }
    }

    void GetInput()
    {
        inputDir = Vector3.zero;
        Transform cam = Camera.main.transform;
        Vector3 flatCamForward = new Vector3(cam.forward.x, 0f, cam.forward.z).normalized;
        Vector3 flatCamRight = new Vector3(cam.right.x, 0f, cam.right.z).normalized;

        if (Input.GetKey(keybinds.walkForward))
        {
            inputDir += flatCamForward;
        }
        if (Input.GetKey(keybinds.walkBackward))
        {
            inputDir -= flatCamForward;
        }
        if (Input.GetKey(keybinds.walkLeft))
        {
            inputDir -= flatCamRight;
        }
        if (Input.GetKey(keybinds.walkRight))
        {
            inputDir += flatCamRight;
        }
    }

}
