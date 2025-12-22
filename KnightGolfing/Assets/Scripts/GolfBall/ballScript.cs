using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class ballScript : MonoBehaviour
{
    [Header("Technical")]
    PlayerItem pi;
    public Rigidbody rb;
    public PhysicMaterial basePhyMat;
    PhysicMaterial phyMat;
    Collider myCollider;
    public enum State { idle, flying, rolling}
    public State curState = State.idle;
    [Header("Stats")]
    public float mass;
    public int pierce;
    public float airDrag;
    public float friction;
    public float bounce;
    [Header("Internal")]
    public bool onGround;
    
    public void SetUp(PlayerItem playerItem, PlayerInput playerInput)
    {
        //Get Refs
        pi = playerItem;
        rb = GetComponent<Rigidbody>();

        //Set Base Stats
        mass = 0.1f;
        pierce = 0;
        airDrag = 0.01f;
        bounce = 0.9f;
        friction = 0.05f;

        //Apply Stat Changes
        ItemObject.BallProperties myInfo = playerItem.heldBalls[playerInput.selectedBallSlot].ballInfo;
        mass *= myInfo.mass;
        pierce += myInfo.pierce;
        airDrag *= myInfo.airDrag;
        bounce *= myInfo.bounce;
        friction *= myInfo.friction;

        //Update Physics
        PhysicsSetUp();
    }
    void PhysicsSetUp()
    {
        myCollider = GetComponent<Collider>();

        //Build New PhysicsMaterial
        phyMat = new PhysicMaterial();
        phyMat.dynamicFriction = 0;
        phyMat.staticFriction = 0;
        phyMat.bounciness = bounce;
        phyMat.frictionCombine = PhysicMaterialCombine.Minimum;
        phyMat.bounceCombine = basePhyMat.bounceCombine;
        phyMat.name = "Ball_Instance";

        //Setup RigidBody For A Clean Slate.
        rb.mass = mass;
        rb.drag = 0f;
        rb.angularDrag = 0.05f;
        rb.useGravity = false;
        rb.isKinematic = false;

        //Apply New PhysicsMaterial
        myCollider.material = phyMat;
    }
    public void PrepareForLaunch()
    {
        if (rb.isKinematic) { return; }
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }
    public void Launch(float force, Vector3 forceDir)
    {
        //FIRE!!
        rb.isKinematic = false; onGround = false;
        StartCoroutine(FlightCheck());
        rb.AddForce(3f * force * forceDir, ForceMode.Impulse);
    }
    public bool IsIdle() { return curState == State.idle; }
    IEnumerator FlightCheck()
    {
        curState = State.flying; float graceTimer = 0.1f;
        while (!onGround || graceTimer > 0) { yield return new WaitForEndOfFrame(); if (graceTimer > 0) { graceTimer -= Time.deltaTime; } }
        curState = State.rolling;
        yield return null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        int colLayer = collision.collider.gameObject.layer;
        switch (colLayer)
        {
            case 0: onGround = true; break; //Default
            case 3: onGround = true; break; //Obstacle
            case 6: onGround = true; break; //Ground
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        int colLayer = collision.collider.gameObject.layer;
        switch (colLayer)
        {
            case 0: onGround = false; break; //Default
            case 3: onGround = false; break; //Obstacle
            case 6: onGround = false; break; //Ground
        }
    }
    private void Update()
    {
        
        if (curState == State.idle) { return; }
        ManagePhysics();
    }
    void ManagePhysics()
    {
        //Gravity
        rb.velocity += Physics.gravity * Time.deltaTime;

        //Drag
        rb.velocity /= 1 + ((airDrag * Time.deltaTime) / mass);

        //Friction
        float frictionMod;
        if (onGround)
        {
            switch (rb.velocity.magnitude)
            {
                case < 0.3f: rb.velocity = Vector3.zero; curState = State.idle; return;
                case < 1f: frictionMod = 1f + (rb.velocity.magnitude / 0.5f); break;
                default: frictionMod = 1f; break;
            }
            rb.velocity /= 1 + ((friction * frictionMod * Time.deltaTime) / mass);
        }
        
    }
}
