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
    ballScript launchedBall;
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
    }
    void Swing()
    {
        //hitbox.SetActive(true);
        chargePower = pUI.chargeFill.fillAmount;
        launchedBall.Launch(chargePower * pItem.heldClubs[selectedClubSlot].clubInfo.force, ballSpawn.forward);

        ReturnToMovement();


        pUI.ChargingUI(0);
    }
    void ManageSwing()
    {
        pUI.ChargingUI(chargeAmt);
        launchedBall.transform.position = ballSpawn.position;
        launchedBall.transform.rotation = ballSpawn.rotation;
        launchedBall.PrepareForLaunch();
    }
    void ReturnToMovement()
    {
        aimSwingBufferTimer = 0.1f;
        freezeMovement = false;
        curState = State.idle;
        pMvt.actingCamOffset = pMvt.camOffset;
        pMvt.ResumeProcedural();
        anim.enabled = false;
    }
}
