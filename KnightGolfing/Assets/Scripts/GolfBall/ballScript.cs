using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.UI;
using UnityEngine;

public class ballScript : MonoBehaviour
{
    [Header("Technical")]
    PlayerItem pi;
    Rigidbody rb;
    public PhysicMaterial basePhyMat;
    PhysicMaterial phyMat;
    Collider myCollider;
    public enum State { idle, flying, rolling}
    public State curState = State.idle;
    [Header("Stats")]
    public float mass;
    public int pierce;
    public float airDrag;
    public float bounce;
    [Header("Internal")]
    public bool onGround;
    bool launched = false;
    
    public void SetUp(PlayerItem playerItem, PlayerInput playerInput)
    {
        //Get Refs
        pi = playerItem;
        rb = GetComponent<Rigidbody>();

        //Update Physics
        PhysicsSetUp();

        //Get Stat Changes
        ItemObject.BallProperties myInfo = playerItem.heldBalls[playerInput.selectedBallSlot].ballInfo;
        mass = myInfo.mass;
        pierce = myInfo.pierce;
        airDrag = myInfo.airDrag;
        bounce = myInfo.bounce;

        //Apply Stat Changes
        rb.mass *= mass;
        rb.drag *= airDrag;
        phyMat.bounciness *= bounce;
    }
    void PhysicsSetUp()
    {
        myCollider = GetComponent<Collider>();

        //Build New PhysicsMaterial
        phyMat = new PhysicMaterial();
        phyMat.dynamicFriction = basePhyMat.dynamicFriction;
        phyMat.staticFriction = basePhyMat.staticFriction;
        phyMat.bounciness = basePhyMat.bounciness;
        phyMat.frictionCombine = basePhyMat.frictionCombine;
        phyMat.bounceCombine = basePhyMat.bounceCombine;
        phyMat.name = "Ball_Instance";

        //Setup RigidBody For A Clean Slate.
        rb.mass = 0.1f;
        rb.drag = 0.1f;
        rb.angularDrag = 0.05f;
        rb.useGravity = true;
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
        rb.isKinematic = false;
        StartCoroutine(FlightCheck());
        rb.AddForce(forceDir * force, ForceMode.Impulse);
    }
    public bool IsIdle() { return curState == State.idle; }
    IEnumerator FlightCheck()
    {
        curState = State.flying; launched = true; float graceTimer = 0.1f;
        while (!onGround || graceTimer > 0) { yield return new WaitForEndOfFrame(); if (graceTimer > 0) { graceTimer -= Time.deltaTime; } }
        launched = false; curState = State.rolling;
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
}
