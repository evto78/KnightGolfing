using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("References")]
    PlayerMovement pMvt;
    PlayerUI pUI;
    PlayerItem pItem;
    public ballScript launchedBall;
    public Transform ballSpawn;

    [Header("Controls")]
    public KeybindData keybinds;
    public bool positive;
    [Header("Golfing")]
    public GameObject hitbox;
    public Animator anim;
    public enum State { idle, moving, sprinting, sliding, jumping, aiming, swinging, spectating}
    public State curState;
    public bool freezeMovement;
    float aimSwingBufferTimer;
    public int selectedClubSlot;
    public int selectedBallSlot;
    float chargeAmt;
    public float chargePower;
    public float turnPower = 0.5f;
    private void Awake()
    {
        hitbox.SetActive(false);
        curState = State.idle;
        pMvt = GetComponent<PlayerMovement>();
        pItem = GetComponent<PlayerItem>();
        pUI = GetComponent<PlayerUI>();
        positive = true;
    }
    void Start()
    {
        keybinds.ResetKeybinds(); // <-- Force reseting every time since nothing is saved rn
        anim.enabled = false;
    }
    private void Update()
    {
        switch (curState)
        {
            case State.aiming:
                //quick thing to switch between adding delta time and subtracting it without a wave
                switch(positive)
                {
                    case true:
                        chargeAmt += Time.deltaTime;

                        if (chargeAmt >= 1) 
                        { 
                            positive = false;
                        }
                        break;

                    case false:
                        chargeAmt -= Time.deltaTime;
                        if (chargeAmt <= 0)
                        {
                            positive = true;
                        }
                        break;
                }
                ManageSwing();
                break;
            case State.spectating:
                ManageSpectating();
                break;
        }
        GetInputs();
    }
    void GetInputs()
    {
        if (aimSwingBufferTimer > 0) { aimSwingBufferTimer -= Time.deltaTime; }

        if (Input.GetKeyDown(keybinds.swing)) { AttemptSwing(); }
        if (Input.GetKeyDown(keybinds.changeBall)) { selectedBallSlot++; } if (selectedBallSlot >= pItem.heldBalls.Count) { selectedBallSlot = 0; }
        if (Input.GetKeyDown(keybinds.changeClub)) { selectedClubSlot++; } if (selectedClubSlot >= pItem.heldClubs.Count) { selectedClubSlot = 0; }

        //DEVCOMMANDS
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        { 
            pItem.heldBalls.Clear(); pItem.heldBalls.Add(pItem.itemData[0]); pItem.heldBalls.Add(pItem.itemData[4]); pItem.heldBalls.Add(pItem.itemData[3]);
            pItem.heldClubs.Clear(); pItem.heldClubs.Add(pItem.itemData[1]); pItem.heldClubs.Add(pItem.itemData[6]); pItem.heldClubs.Add(pItem.itemData[12]);
            pItem.heldArmor = pItem.itemData[2];
            pItem.ClubUpdate(); 
        }

    }
    void AttemptSwing()
    {
        if (!pMvt.onGround || aimSwingBufferTimer > 0) { return; }
        switch (curState)
        {
            case State.idle: BeginSwinging(); break;
            case State.moving: BeginSwinging(); break;
            case State.sprinting: BeginSwinging(); break;
            case State.sliding: BeginSwinging(); break;
            case State.aiming: Swing(); break;
            case State.spectating: ReturnToMovement(); break;
        }
    }
    void BeginSwinging()
    {
        aimSwingBufferTimer = 0.1f;
        freezeMovement = true;
        curState = State.aiming;
        pMvt.actingCamOffset = pMvt.camOffset + new Vector3(pMvt.mainCam.transform.right.x, 0, pMvt.mainCam.transform.right.z);
        pMvt.PauseProcedural();
        anim.enabled = true;
        pMvt.rb.velocity /= 4f;
        chargeAmt = 0f;

        launchedBall = Instantiate(pItem.heldBalls[selectedBallSlot].ballInfo.prefab).GetComponent<ballScript>();
        launchedBall.transform.position = ballSpawn.position;
        launchedBall.transform.rotation = ballSpawn.rotation;
        launchedBall.SetUp(pItem, this);
        launchedBall.PrepareForLaunch();

        pUI.ChangeState(PlayerUI.State.aiming);
    }
    void Swing()
    {
        //hitbox.SetActive(true);
        chargePower = pUI.chargeFill.fillAmount;
        if(chargePower > 0.95f) { chargePower = 1.25f; pUI.chargeFill.fillAmount = 1; } //POWER SHOT!
        Vector3 launchDir = (pMvt.mainCam.transform.forward + Vector3.up * 0.2f).normalized;
        launchedBall.Launch(chargePower * pItem.heldClubs[selectedClubSlot].clubInfo.force, launchDir);
        curState = State.spectating;
        pMvt.spectatingCamera.GetComponent<LookAtTarget>().target = launchedBall.transform;
        pUI.ChangeState(PlayerUI.State.spectating);
    }
    void ManageSwing()
    {
        pMvt.actingCamOffset = pMvt.camOffset + new Vector3(pMvt.mainCam.transform.right.x, 0, pMvt.mainCam.transform.right.z);

        pUI.ChargingUI(chargeAmt);
        launchedBall.transform.position = ballSpawn.position;
        launchedBall.transform.rotation = ballSpawn.rotation;
        launchedBall.PrepareForLaunch();

        pUI.DrawPredictionLine(pItem.heldClubs[selectedClubSlot].clubInfo.force, launchedBall.mass, launchedBall.airDrag, chargeAmt, (pMvt.mainCam.transform.forward + Vector3.up * 0.2f).normalized);
    }
    void ManageSpectating()
    {
        chargeAmt = 0;
        pUI.ChargingUI(chargeAmt);
        pMvt.mainCam.enabled = false;
        pMvt.uiCam.enabled = false;
        Camera sCam = pMvt.spectatingCamera;
        pMvt.spectatingBall = true;
        sCam.transform.parent = null;
        sCam.enabled = true;
        Vector3 spectatingOffset = sCam.transform.forward * -6f;
        sCam.transform.position = Vector3.Lerp(sCam.transform.position, launchedBall.transform.position + spectatingOffset, Time.deltaTime * 4f);
        sCam.fieldOfView = Mathf.Lerp(pMvt.minMaxSpectatingFOV.x, pMvt.minMaxSpectatingFOV.y, Mathf.Clamp(launchedBall.rb.velocity.magnitude / 100f, 0.1f, 1f));

        switch (launchedBall.curState)
        {
            case ballScript.State.idle: break;
            case ballScript.State.flying: break;
            case ballScript.State.rolling: break;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Rigidbody rb = launchedBall.GetComponent<Rigidbody>();
            rb.velocity = Quaternion.Euler(0, -turnPower, 0) * rb.velocity;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Rigidbody rb = launchedBall.GetComponent<Rigidbody>();
            rb.velocity = Quaternion.Euler(0, turnPower, 0) * rb.velocity;
        }
    }
    void ReturnToMovement()
    {
        chargeAmt = 0;
        pUI.ChargingUI(0);
        aimSwingBufferTimer = 0.1f;
        freezeMovement = false;
        pMvt.mainCam.enabled = true;
        pMvt.uiCam.enabled = true;
        pMvt.spectatingBall = false;
        Camera sCam = pMvt.spectatingCamera;
        sCam.enabled = false;
        sCam.transform.parent = pMvt.camHolder.GetChild(0);
        sCam.transform.localPosition = Vector3.forward * 0.5f;
        sCam.transform.localEulerAngles = Vector3.zero;
        curState = State.idle;
        pMvt.actingCamOffset = pMvt.camOffset;
        pMvt.ResumeProcedural();
        anim.enabled = false;

        pUI.ChangeState(PlayerUI.State.walking);
    }
}
