using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //Manager Scripts
    PlayerMovement pMvt;
    PlayerItem pItem;
    PlayerUI pUI;

    [Header("Controls")]
    public KeybindData keybinds;

    [Header("Golfing")]
    public Animator anim;
    public Vector3 swingingCamOffset;
    public enum State { idle, moveing, sprinting, sliding, jumping, aiming, swinging, spectating}
    public State curState;
    public bool freezeMovement;
    float aimSwingBufferTimer;
    public int selectedClubSlot;
    public int selectedBallSlot;

    private void Awake()
    {
        curState = State.idle;
        pMvt = GetComponent<PlayerMovement>();
        pItem = GetComponent<PlayerItem>();
        pUI = GetComponent<PlayerUI>();
    }
    void Start()
    {
        keybinds.ResetKeybinds();
        anim.enabled = false;
    }
    private void Update()
    {
        switch (curState)
        {
            case State.aiming:

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
    }
    void Swing()
    {
        ReturnToMovement();
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
