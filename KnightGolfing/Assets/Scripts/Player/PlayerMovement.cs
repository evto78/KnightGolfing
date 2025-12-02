using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Manager Scripts
    PlayerInput pInput; KeybindData keybinds;

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

    [Header("Animation")]
    public BodyHeightAdjust heightAdjust;
    public Transform mesh;
    public bool pauseProcedural;
    public List<Transform> meshLimbTransforms = new List<Transform>();
    List<Vector3> meshInitialPos = new List<Vector3>();
    List<Vector3> meshInitialRot = new List<Vector3>();
    public Transform backpack; public Transform torso; Vector3 backpackInitalOffset;

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

    private void Awake()
    {
        pInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        initialHeight = myCollider.height;
        keybinds = pInput.keybinds;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        foreach (Camera cam in camHolder.GetComponentsInChildren<Camera>()) { cam.fieldOfView = fov; }
        initialCamPos = camHolder.localPosition;
        camDistance = Mathf.Abs(camHolder.GetChild(0).localPosition.z);
        actingCamOffset = camOffset;
        camHolder.localPosition = initialCamPos + actingCamOffset;

        for (int i = 0; i < meshLimbTransforms.Count; i++)
        {
            meshInitialPos.Add(meshLimbTransforms[i].localPosition);
            meshInitialRot.Add(meshLimbTransforms[i].localEulerAngles);
        }
        backpackInitalOffset = backpack.localPosition;
    }
    void Update()
    {
        onGround = GroundCheck();
        PositionBackPack();
        if (pInput.freezeMovement) { CameraMove(); Friction(); Gravity(); return; }
        StatUpdate();
        GetInput();
        Move();
        Turn();
    }
    public void PauseProcedural()
    {
        pauseProcedural = true;
        heightAdjust.enabled = !pauseProcedural;
        heightAdjust.transform.GetComponent<Animator>().enabled = !pauseProcedural;
        for (int i = 0; i < meshLimbTransforms.Count; i++)
        {
            meshLimbTransforms[i].localPosition = meshInitialPos[i];
            meshLimbTransforms[i].localEulerAngles = meshInitialRot[i];
        }
    }
    public void ResumeProcedural()
    {
        pauseProcedural = false;
        heightAdjust.enabled = !pauseProcedural;
        heightAdjust.transform.GetComponent<Animator>().enabled = !pauseProcedural;
    }
    void StatUpdate()
    {
        walkSpeed = baseWalkSpeed;
        jumpHeight = baseJumpHeight;
    }
    void GetInput()
    {
        timeSinceLastJump = Mathf.Clamp(timeSinceLastJump+Time.deltaTime, 0f, 100f);

        Transform cam = Camera.main.transform;
        Vector3 flatCamForward = new Vector3(cam.forward.x, 0f, cam.forward.z).normalized;
        Vector3 flatCamRight = new Vector3(cam.right.x, 0f, cam.right.z).normalized;
        inputDir = Vector3.zero;
        if (Input.GetKey(keybinds.walkForward)) { inputDir += flatCamForward; }
        if (Input.GetKey(keybinds.walkLeft)) { inputDir -= flatCamRight; }
        if (Input.GetKey(keybinds.walkRight)) { inputDir += flatCamRight; }
        if (Input.GetKey(keybinds.walkBackward)) { inputDir -= flatCamForward; }
        if (onGround) { sprinting = Input.GetKey(keybinds.sprint); }
        if (Input.GetKeyDown(keybinds.jump)) { Jump(); }
        if (Input.GetKey(keybinds.slide)) { Slide(); } else { sliding = false; myCollider.height = initialHeight; }
        inputDir = inputDir.normalized;

        CameraMove();

        //UpdateState
        if (onGround && rb.velocity.magnitude < 0.1f) { pInput.curState = PlayerInput.State.idle; }
        else if (onGround && timeSinceLastJump < 0.25f) { pInput.curState = PlayerInput.State.jumping; }
        else if (sliding) { pInput.curState = PlayerInput.State.sliding; }
        else if (inputDir != Vector3.zero && sprinting && rb.velocity.magnitude > 3) { pInput.curState = PlayerInput.State.sprinting; }
        else if (inputDir != Vector3.zero) { pInput.curState = PlayerInput.State.moveing; }
    }
    void Move()
    {
        float sprintBonus;
        if (sprinting) { sprintBonus = sprintingModifier; } else { sprintBonus = 1f; }
        if (sliding) { inputDir /= 1.5f; sprintBonus = 1f; }
        if (onGround) { rb.AddForce(inputDir * walkSpeed * Time.deltaTime * sprintBonus); }
        else { rb.AddForce(inputDir * airControlSpeed * Time.deltaTime * sprintBonus); }
        Friction();
        Gravity();
    }
    void Turn()
    {
        if(rb.velocity != Vector3.zero)
        {
            Quaternion curRotation = mesh.rotation;
            Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if(flatVel != Vector3.zero)
            {
                mesh.rotation = Quaternion.LookRotation(flatVel);
                Quaternion tarRotation = mesh.rotation;
                mesh.rotation = Quaternion.Lerp(curRotation, tarRotation, Time.deltaTime * 10f);
            }
        }
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
    void Jump()
    {
        if (jumpsLeft > 0 && (timeSinceLastJump > 0.25f || rb.velocity.y < 0))
        {
            timeSinceLastJump = 0f;
            jumpsLeft -= 1;
            if(rb.velocity.y < 0) { rb.velocity += Vector3.up * -rb.velocity.y; }
            rb.AddForce(transform.up * jumpHeight, ForceMode.Force);
        }
    }
    void Slide()
    {
        sliding = true;
        myCollider.height = initialHeight / 2f;
    }
    void Friction()
    {
        float actingFriction = friction;
        if (sliding) { actingFriction = ((friction - 1f) / 4f) + 1f; }
        if (onGround)
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (inputDir == Vector3.zero && !sliding) { flatVel = flatVel / (friction * 4 * (1 + Time.deltaTime)); rb.useGravity = false; } else { rb.useGravity = true; }
            flatVel = flatVel / (friction * (1 + Time.deltaTime));
            rb.velocity = new Vector3(flatVel.x, rb.velocity.y / (1 + Time.deltaTime), flatVel.z);
        }
        else { rb.useGravity = true; }
    }
    void Gravity()
    {
        if(!onGround || sliding) { rb.AddForce(Vector3.up * -additionalGravity * Time.deltaTime); }
    }
    bool GroundCheck()
    {
        if (Physics.BoxCast(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(myCollider.radius/1.1f,myCollider.height/2.2f,myCollider.radius/1.1f), -Vector3.up, out RaycastHit hit, transform.rotation, 0.55f))
        {
            if (hit.transform.gameObject.layer == 0 || hit.transform.gameObject.layer == 3 || hit.transform.gameObject.layer == 6)
            {
                if(timeSinceLastJump > 0.25f) { jumpsLeft = numOfJumps; }
                return true;
            }
        }

        return false;
    }
    void PositionBackPack()
    {
        backpack.parent = torso;
        backpack.localPosition = Vector3.zero; backpack.localEulerAngles = Vector3.zero;
        backpack.localPosition += backpackInitalOffset;
        backpack.parent = transform;
    }
}
