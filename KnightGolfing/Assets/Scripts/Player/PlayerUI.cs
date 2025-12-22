using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PlayerUI : MonoBehaviour
{
    //manager scripts
    PlayerItem pItem;
    PlayerMovement pMvt;
    PlayerInput pInput;

    [Header("Equipment")]
    public Image curBall;
    public Image prevBall;
    public Image nextBall;

    public Image curClub;
    public Image prevClub;
    public Image nextClub;

    [Header("UI")]
    public Image chargeFill;
    public Transform chargeFillLine;
    public Transform chargeTop; Vector3 chargeTopPos;
    public Transform chargeBottom; Vector3 chargeBottomPos;
    public TextMeshProUGUI ballVelocityText;
    [System.Serializable]
    public class ShotType
    {
        public GameObject textObj;
        public TextMeshProUGUI tmp;
        public Color initialColor;
        public float timeToFade;
        [Tooltip("A value between 0 and 1 that is the amount the current aim charge needs to be above for this to display.")]
        public float chargeThreshold;
    }
    public List<ShotType> shotTypes;
    ShotType curShotType;
    Color curShotTypeColor; float shotTypeFadeTimer;
    public enum State { walking, aiming, spectating}
    public State curState;
    public GameObject walkUI;
    public GameObject aimUI;
    public GameObject specUI;
    public GameObject gearUI;
    public LineRenderer lr;
    public GameObject ballPointer;

    private void Awake()
    {
        pItem = GetComponent<PlayerItem>();
        pMvt = GetComponent<PlayerMovement>();
        pInput = GetComponent<PlayerInput>();
    }
    void Start()
    {
        lr.enabled = false;
        chargeTopPos = chargeTop.position;
        chargeBottomPos = chargeBottom.position;

        ChangeState(State.walking);
    }
    public void ChangeState(State newState)
    {
        curState = newState;
        switch (curState)
        {
            case State.walking:
                walkUI.SetActive(true); aimUI.SetActive(false); specUI.SetActive(false); gearUI.SetActive(true);
                break;
            case State.aiming:
                walkUI.SetActive(false); aimUI.SetActive(true); specUI.SetActive(false); gearUI.SetActive(true);
                break;
            case State.spectating:
                walkUI.SetActive(false); aimUI.SetActive(true); specUI.SetActive(true); gearUI.SetActive(false);
                UpdateCurShotType(pInput.chargePower);
                break;
        }
    }
    public void ChargingUI(float charge)
    {

        chargeFill.fillAmount = charge;

        chargeFillLine.transform.position = Vector3.Lerp(chargeBottomPos, chargeTopPos, charge);
    }
    public void UpdateUI()
    {
        switch (curState)
        {
            case State.walking:
                UpdateGearUI();
                UpdateWalkUI();
                break;
            case State.aiming:
                UpdateGearUI();
                UpdateAimUI();
                break;
            case State.spectating:
                UpdateSpecUI();
                break;
        }
    }
    void UpdateGearUI()
    {
        switch (pItem.heldClubs.Count)
        {
            case 0:
                curClub.enabled = false; nextClub.enabled = false; prevClub.enabled = false;
                break;
            case 1:
                curClub.enabled = true; nextClub.enabled = false; prevClub.enabled = false;
                curClub.sprite = pItem.heldClubs[0].sprite;
                break;
            case 2:
                curClub.enabled = true; nextClub.enabled = true; prevClub.enabled = false;
                curClub.sprite = pItem.heldClubs[0].sprite; nextClub.sprite = pItem.heldClubs[1].sprite;
                break;
            case 3:
                curClub.enabled = true; nextClub.enabled = true; prevClub.enabled = true;
                curClub.sprite = pItem.heldClubs[0].sprite; nextClub.sprite = pItem.heldClubs[1].sprite; prevClub.sprite = pItem.heldClubs[2].sprite;
                break;
        }
        switch (pInput.selectedBallSlot)
        {
            case 0:
                switch (pItem.heldBalls.Count)
                {
                    case 0:
                        curBall.enabled = false; nextBall.enabled = false; prevBall.enabled = false;
                        break;
                    case 1:
                        curBall.enabled = true; nextBall.enabled = false; prevBall.enabled = false;
                        curBall.sprite = pItem.heldBalls[0].sprite;
                        break;
                    case 2:
                        curBall.enabled = true; nextBall.enabled = true; prevBall.enabled = false;
                        curBall.sprite = pItem.heldBalls[0].sprite; nextBall.sprite = pItem.heldBalls[1].sprite;
                        break;
                    case 3:
                        curBall.enabled = true; nextBall.enabled = true; prevBall.enabled = true;
                        curBall.sprite = pItem.heldBalls[0].sprite; nextBall.sprite = pItem.heldBalls[1].sprite; prevBall.sprite = pItem.heldBalls[2].sprite;
                        break;
                }
                break;
            case 1:
                switch (pItem.heldBalls.Count)
                {
                    case 0:
                        curBall.enabled = false; nextBall.enabled = false; prevBall.enabled = false;
                        break;
                    case 1:
                        curBall.enabled = true; nextBall.enabled = false; prevBall.enabled = false;
                        curBall.sprite = pItem.heldBalls[1].sprite;
                        break;
                    case 2:
                        curBall.enabled = true; nextBall.enabled = true; prevBall.enabled = false;
                        curBall.sprite = pItem.heldBalls[1].sprite; nextBall.sprite = pItem.heldBalls[2].sprite;
                        break;
                    case 3:
                        curBall.enabled = true; nextBall.enabled = true; prevBall.enabled = true;
                        curBall.sprite = pItem.heldBalls[1].sprite; nextBall.sprite = pItem.heldBalls[2].sprite; prevBall.sprite = pItem.heldBalls[0].sprite;
                        break;
                }
                break;
            case 2:
                switch (pItem.heldBalls.Count)
                {
                    case 0:
                        curBall.enabled = false; nextBall.enabled = false; prevBall.enabled = false;
                        break;
                    case 1:
                        curBall.enabled = true; nextBall.enabled = false; prevBall.enabled = false;
                        curBall.sprite = pItem.heldBalls[2].sprite;
                        break;
                    case 2:
                        curBall.enabled = true; nextBall.enabled = true; prevBall.enabled = false;
                        curBall.sprite = pItem.heldBalls[2].sprite; nextBall.sprite = pItem.heldBalls[0].sprite;
                        break;
                    case 3:
                        curBall.enabled = true; nextBall.enabled = true; prevBall.enabled = true;
                        curBall.sprite = pItem.heldBalls[2].sprite; nextBall.sprite = pItem.heldBalls[0].sprite; prevBall.sprite = pItem.heldBalls[1].sprite;
                        break;
                }
                break;
        }
        switch (pInput.selectedClubSlot)
        {
            case 0:
                switch (pItem.heldClubs.Count)
                {
                    case 0:
                        curClub.enabled = false; nextClub.enabled = false; prevClub.enabled = false;
                        break;
                    case 1:
                        curClub.enabled = true; nextClub.enabled = false; prevClub.enabled = false;
                        curClub.sprite = pItem.heldClubs[0].sprite;
                        break;
                    case 2:
                        curClub.enabled = true; nextClub.enabled = true; prevClub.enabled = false;
                        curClub.sprite = pItem.heldClubs[0].sprite; nextClub.sprite = pItem.heldClubs[1].sprite;
                        break;
                    case 3:
                        curClub.enabled = true; nextClub.enabled = true; prevClub.enabled = true;
                        curClub.sprite = pItem.heldClubs[0].sprite; nextClub.sprite = pItem.heldClubs[1].sprite; prevClub.sprite = pItem.heldClubs[2].sprite;
                        break;
                }
                break;
            case 1:
                switch (pItem.heldClubs.Count)
                {
                    case 0:
                        curClub.enabled = false; nextClub.enabled = false; prevClub.enabled = false;
                        break;
                    case 1:
                        curClub.enabled = true; nextClub.enabled = false; prevClub.enabled = false;
                        curClub.sprite = pItem.heldClubs[1].sprite;
                        break;
                    case 2:
                        curClub.enabled = true; nextClub.enabled = true; prevClub.enabled = false;
                        curClub.sprite = pItem.heldClubs[1].sprite; nextClub.sprite = pItem.heldClubs[2].sprite;
                        break;
                    case 3:
                        curClub.enabled = true; nextClub.enabled = true; prevClub.enabled = true;
                        curClub.sprite = pItem.heldClubs[1].sprite; nextClub.sprite = pItem.heldClubs[2].sprite; prevClub.sprite = pItem.heldClubs[0].sprite;
                        break;
                }
                break;
            case 2:
                switch (pItem.heldClubs.Count)
                {
                    case 0:
                        curClub.enabled = false; nextClub.enabled = false; prevClub.enabled = false;
                        break;
                    case 1:
                        curClub.enabled = true; nextClub.enabled = false; prevClub.enabled = false;
                        curClub.sprite = pItem.heldClubs[2].sprite;
                        break;
                    case 2:
                        curClub.enabled = true; nextClub.enabled = true; prevClub.enabled = false;
                        curClub.sprite = pItem.heldClubs[2].sprite; nextClub.sprite = pItem.heldClubs[0].sprite;
                        break;
                    case 3:
                        curClub.enabled = true; nextClub.enabled = true; prevClub.enabled = true;
                        curClub.sprite = pItem.heldClubs[2].sprite; nextClub.sprite = pItem.heldClubs[0].sprite; prevClub.sprite = pItem.heldClubs[1].sprite;
                        break;
                }
                break;
        }
    }
    void UpdateWalkUI()
    {
        if (pInput.launchedBall != null)
        {
            ballPointer.SetActive(true);
            ballPointer.GetComponent<LookAtTarget>().target = pInput.launchedBall.transform;
        }
        else
        {
            ballPointer.SetActive(false);
        }
    }
    void UpdateAimUI()
    {
        ballPointer.SetActive(false);
    }
    void UpdateCurShotType(float chargePower) 
    {
        curShotType = shotTypes[0];
        foreach (ShotType st in shotTypes) { if (chargePower > st.chargeThreshold) { curShotType = st; } }
        curShotType.tmp.color = curShotType.initialColor;
        shotTypeFadeTimer = curShotType.timeToFade;
        curShotTypeColor = curShotType.initialColor;
    }
    void UpdateSpecUI()
    {
        ballPointer.SetActive(false);
        int roundedVel = Mathf.FloorToInt(pInput.launchedBall.rb.velocity.magnitude);
        switch (roundedVel)
        {
            case < 1: ballVelocityText.text = "Velocity : <1"; break;
            default: ballVelocityText.text = "Velocity : " + roundedVel; break;
        } if (pInput.launchedBall.rb.velocity.magnitude == 0) { ballVelocityText.text = "Velocity : STOPPED"; }
        foreach (ShotType st in shotTypes) { st.textObj.SetActive(false); }
        curShotType.textObj.SetActive(true);
        if (shotTypeFadeTimer > 0) { shotTypeFadeTimer -= Time.deltaTime; }
        curShotTypeColor = Color.Lerp(curShotType.initialColor, new Color(curShotType.initialColor.r, curShotType.initialColor.g, curShotType.initialColor.b, 0f), (shotTypeFadeTimer / curShotType.timeToFade));
        curShotType.tmp.color = curShotTypeColor;
    }
    public void DrawPredictionLine(float force, float mass, float drag, float chargeAmt, Vector3 forceDir)
    {
        lr.enabled = true;
        if (chargeAmt > 0.95f) { chargeAmt = 1.25f; }

        float stepTime = 0.1f;
        float flightDuration = 3f;

        int pointCount = (int)(flightDuration / stepTime);
        Vector3[] lPoints = new Vector3[pointCount];

        Vector3 predicVel = ((chargeAmt * force * 3f * forceDir) / mass);
        Vector3 stepPredicVel = predicVel;
        Vector3 prevPoint = pInput.ballSpawn.position;

        lPoints[0] = prevPoint;
        for (int i = 0; i < pointCount-1; i++) 
        {
            stepPredicVel += Physics.gravity * stepTime;
            stepPredicVel /= 1 + ((drag * stepTime) / mass);

            lPoints[i+1] = prevPoint + stepPredicVel * stepTime;

            prevPoint = lPoints[i+1];
        }

        lr.positionCount = lPoints.Length;
        lr.SetPositions(lPoints);
    }
}
