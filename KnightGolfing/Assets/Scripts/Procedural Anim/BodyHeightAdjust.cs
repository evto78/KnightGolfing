using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHeightAdjust : MonoBehaviour
{
    PlayerMovement pMvt;
    public float runOffset; public float crouchOffset; float actingOffset;
    float initalY;
    public List<IKFootSolver> legs; //0 is left, 1 is right
    public Transform root; public Transform rig;
    public enum state { walk, run, idle }
    public state curState;
    public float walkStepHeightMod; public float runStepHeightMod;
    public float walkStepLengthMod; public float runStepLengthMod;
    public float walkStepSpeedMod; public float runStepSpeedMod; float baseStepSpeed;
    public AnimationCurve walkCurve;
    public AnimationCurve runCurve;
    public AnimationCurve walkProgressCurve;
    public AnimationCurve runProgressCurve;
    public float currentSpeed;
    public Vector2 walkRunSpeedThreshold; public float progressToRun;

    private void Awake() { foreach (IKFootSolver solver in legs) { solver.manager = this; } }

    private void Start()
    {
        pMvt = GetComponentInParent<PlayerMovement>();
        actingOffset = crouchOffset;
        initalY = legs[0].transform.localPosition.y;
        baseStepSpeed = legs[0].stepSpeed;
    }
    void Update()
    {
        currentSpeed = pMvt.rb.velocity.magnitude;
        progressToRun = Mathf.Clamp((currentSpeed - walkRunSpeedThreshold.x) / walkRunSpeedThreshold.y, 0f, 1f);

        //auto state change
        if (progressToRun == 1) { ChangeState(state.run); } else { ChangeState(state.walk); }
        if (pMvt.rb.velocity.magnitude < 0.6f) { ChangeState(state.idle); }
        actingOffset = Mathf.Lerp(crouchOffset, runOffset, progressToRun);

        foreach(IKFootSolver solver in legs) { solver.stepSpeed = baseStepSpeed * Mathf.Lerp(walkStepSpeedMod, runStepSpeedMod, progressToRun); }
        ForceStepProgressBetweenPairs();
        HeightAdjustment();
        Lean();
    }
    void Lean()
    {
        float leanAmount = 0f;
        if (legs[0].transform.localPosition.x < -0.18f) { leanAmount += legs[0].transform.localPosition.x; }
        if (legs[1].transform.localPosition.x > 0.18f) { leanAmount += legs[1].transform.localPosition.x; }
        leanAmount *= 10f;
        root.transform.localEulerAngles = new Vector3(root.transform.localEulerAngles.x, root.transform.localEulerAngles.y, leanAmount);
        //rig.transform.localEulerAngles = new Vector3(root.transform.localEulerAngles.x, root.transform.localEulerAngles.y, -leanAmount);
    }
    public void HeightAdjustment()
    {
        float avgY = 0f;
        foreach (IKFootSolver solver in legs)
        {
            avgY += solver.transform.localPosition.y;
        }
        avgY /= legs.Count;
        avgY /= initalY;
        //transform.localPosition = Vector3.up * (avgY + actingOffset);
    }
    public void ChangeState(state newState)
    {
        curState = newState;
        foreach (IKFootSolver solver in legs)
        {
            switch (curState)
            {
                case state.walk: solver.curState = IKFootSolver.state.walk; break;
                case state.run: solver.curState = IKFootSolver.state.run; break;
                case state.idle: solver.curState = IKFootSolver.state.idle; break;
            }
        }
    }
    void ForceStepProgressBetweenPairs()
    {
        float walkPairDifference = 1f; float runPairDifference = 0.2f;

        IKFootSolver l = legs[0];
        IKFootSolver r = legs[1];

        float tarPairDifference = Mathf.Lerp(walkPairDifference, runPairDifference, progressToRun); 

        ApplyChangeToStepProgress(l, r, tarPairDifference, 1f);
    }
    void ApplyChangeToStepProgress(IKFootSolver a, IKFootSolver b, float tarDiff, float force)
    {
        float diff; float diffToTar;
        if (a.stepProgress > b.stepProgress)
        {
            diff = a.stepProgress - b.stepProgress;
            diffToTar = diff - tarDiff;
            if (Mathf.Abs(diffToTar) < 0.1f) { return; }
            a.stepProgress -= Time.deltaTime * (currentSpeed * a.stepSpeed) * diffToTar * force;
        }
        else
        {
            diff = b.stepProgress - a.stepProgress;
            diffToTar = diff - tarDiff;
            if (Mathf.Abs(diffToTar) < 0.1f) { return; }
            b.stepProgress -= Time.deltaTime * (currentSpeed * b.stepSpeed) * diffToTar * force;
        }
    }
}
