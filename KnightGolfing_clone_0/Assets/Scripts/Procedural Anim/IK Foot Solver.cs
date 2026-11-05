using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    public BodyHeightAdjust manager;
    public IKFootSolver pairedLeg;
    public Transform hip;
    public float maxStepHeight;
    public float stepHeight;
    public float stepLength;
    public LayerMask terrainLayer;
    public bool stay;
    public bool stepping;
    public float stepProgress;
    public float stepSpeed;
    Vector3 stayPos;
    public Vector3 nextPos;
    public float nextPosOffset;

    public enum state { walk, run, idle}
    public state curState;

    void Start()
    {
        SnapToGround();
        stay = true;
        stepping = false;
    }
    void Update()
    {
        StateFluidSteps();
    }
    void StateFluidSteps()
    {
        if (!stepping)
        {
            if (stay) { transform.position = stayPos; } else { SnapToGround(); }
            Ray ray = new Ray(); RaycastHit info;
            switch (curState)
            {
                case state.idle:
                    ray = new Ray(hip.position, Vector3.down);
                    if (Physics.Raycast(ray, out info, maxStepHeight * 2f, terrainLayer.value)) { nextPos = info.point; Debug.DrawRay(info.point, Vector3.up); }
                    if (Vector3.Distance(transform.position, nextPos) > (Mathf.Lerp(manager.walkStepLengthMod, manager.runStepLengthMod, manager.progressToRun) * stepLength) / 3f)
                    {
                        if (!pairedLeg.stepping) { stay = false; stepping = true; stepProgress = 0; }
                    }
                    break;
                case state.walk:
                    ray = new Ray(hip.position + hip.up * (Mathf.Lerp(manager.walkStepLengthMod, manager.runStepLengthMod, manager.progressToRun) * stepLength), Vector3.down);
                    if (Physics.Raycast(ray, out info, maxStepHeight * 2f, terrainLayer.value)) { nextPos = info.point; Debug.DrawRay(info.point, Vector3.up); }
                    if (Vector3.Distance(transform.position, nextPos) > (Mathf.Lerp(manager.walkStepLengthMod, manager.runStepLengthMod, manager.progressToRun) * stepLength) * 1.5f)
                    {
                        if (!pairedLeg.stepping) { stay = false; stepping = true; stepProgress = 0; }
                    }
                    break;
                case state.run:
                    ray = new Ray(hip.position + hip.up * (Mathf.Lerp(manager.walkStepLengthMod, manager.runStepLengthMod, manager.progressToRun) * stepLength), Vector3.down);
                    if (Physics.Raycast(ray, out info, maxStepHeight * 2f, terrainLayer.value)) { nextPos = info.point; Debug.DrawRay(info.point, Vector3.up); }
                    if (Vector3.Distance(transform.position, nextPos) > (Mathf.Lerp(manager.walkStepLengthMod, manager.runStepLengthMod, manager.progressToRun) * stepLength) * 1.5f)
                    {
                        if (!pairedLeg.stepping) { stay = false; stepping = true; stepProgress = 0; }
                    }
                    break;
            }
        }
        else
        {
            stepProgress += Time.deltaTime * Mathf.Clamp(manager.currentSpeed * stepSpeed, stepSpeed, stepSpeed*50f);
            if (curState == state.idle) { stepProgress += Time.deltaTime * Mathf.Clamp(manager.currentSpeed * stepSpeed, stepSpeed*2, stepSpeed * 50f); }
            transform.position = Vector3.LerpUnclamped(stayPos, nextPos, Mathf.Lerp(manager.walkProgressCurve.Evaluate(stepProgress), manager.runProgressCurve.Evaluate(stepProgress), manager.progressToRun));
            transform.position += Vector3.up * (Mathf.Lerp(manager.walkCurve.Evaluate(stepProgress), manager.runCurve.Evaluate(stepProgress), manager.progressToRun) * stepHeight * Mathf.Lerp(manager.walkStepHeightMod, manager.runStepHeightMod, manager.progressToRun));
            if (stepProgress >= 1) { SnapToGround(); stay = true; stepping = false; }
            if (curState == state.idle)
            {
                Ray ray = new Ray(); RaycastHit info;
                ray = new Ray(hip.position, Vector3.down);
                if (Physics.Raycast(ray, out info, maxStepHeight * 2f, terrainLayer.value)) { nextPos = info.point; Debug.DrawRay(info.point, Vector3.up); }
                if (Vector3.Distance(transform.position, nextPos) > (Mathf.Lerp(manager.walkStepLengthMod, manager.runStepLengthMod, manager.progressToRun) * stepLength) / 3f)
                {
                    if (!pairedLeg.stepping) { stay = false; stepping = true; }
                }
            }
        }
    }
    void SnapToGround()
    {
        Ray ray = new Ray(transform.position + Vector3.up * maxStepHeight, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, maxStepHeight * 2f, terrainLayer.value))
        {
            transform.position = new Vector3(transform.position.x, info.point.y, transform.position.z);
            stayPos = transform.position;
        }
    }
}
