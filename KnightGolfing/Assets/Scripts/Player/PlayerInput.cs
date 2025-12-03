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
    public GameObject ballPrefab;
    public ballScript launchedBall;
    public Transform ballSpawn;

    [Header("Controls")]
    public KeybindData keybinds;

    [Header("Golfing")]
    public GameObject hitbox;
    public Animator anim;
    public Vector3 swingingCamOffset;
    public enum State { idle, moveing, sprinting, sliding, jumping, aiming, swinging, spectating}
    public State curState;
    public bool freezeMovement;
    float aimSwingBufferTimer;
    public int selectedClubSlot;
    public int selectedBallSlot;
    float chargeAmt;
    public float chargePower;
    private void Awake()
    {
        hitbox.SetActive(false);
        curState = State.idle;
        pMvt = GetComponent<PlayerMovement>();
        pItem = GetComponent<PlayerItem>();
        pUI = GetComponent<PlayerUI>();

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
                chargeAmt += Time.deltaTime; if (chargeAmt > 1) { chargeAmt--; }
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
            case State.moveing: BeginSwinging(); break;
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
        pMvt.actingCamOffset = swingingCamOffset;
        pMvt.PauseProcedural();
        anim.enabled = true;
        pMvt.rb.velocity /= 4f;
        chargeAmt = 0f;

        launchedBall = Instantiate(ballPrefab).GetComponent<ballScript>();
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
        //Vector3 launchDir = new Vector3(pMvt.mainCam.transform.forward.x, 0.2f, pMvt.mainCam.transform.forward.z).normalized;
        Vector3 launchDir = (pMvt.mainCam.transform.forward + Vector3.up * 0.2f).normalized;
        launchedBall.Launch(chargePower * pItem.heldClubs[selectedClubSlot].clubInfo.force, launchDir);
        curState = State.spectating;
        pMvt.spectatingCamera.GetComponent<LookAtTarget>().target = launchedBall.transform;
        pUI.ChangeState(PlayerUI.State.spectating);
    }
    void ManageSwing()
    {
        pUI.ChargingUI(chargeAmt);
        launchedBall.transform.position = ballSpawn.position;
        launchedBall.transform.rotation = ballSpawn.rotation;
        launchedBall.PrepareForLaunch();

        //pUI.DrawPredictionLine(pItem.heldClubs[selectedClubSlot].clubInfo.force, launchedBall.rb.mass, launchedBall.rb.drag, chargeAmt, new Vector3(pMvt.mainCam.transform.forward.x, 0.2f, pMvt.mainCam.transform.forward.z).normalized);
        pUI.DrawPredictionLine(pItem.heldClubs[selectedClubSlot].clubInfo.force, launchedBall.rb.mass, launchedBall.rb.drag, chargeAmt, (pMvt.mainCam.transform.forward + Vector3.up * 0.2f).normalized);
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
        sCam.fieldOfView = Mathf.Lerp(pMvt.minMaxSpectatingFOV.x, pMvt.minMaxSpectatingFOV.y, launchedBall.rb.velocity.magnitude / 100f);

        switch (launchedBall.curState)
        {
            case ballScript.State.idle: break;
            case ballScript.State.flying: break;
            case ballScript.State.rolling: break;
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
