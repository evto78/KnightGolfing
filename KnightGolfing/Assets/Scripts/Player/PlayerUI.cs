using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        pItem = GetComponent<PlayerItem>();
        pMvt = GetComponent<PlayerMovement>();
        pInput = GetComponent<PlayerInput>();
    }
    void Start()
    {
        chargeTopPos = chargeTop.position;
        chargeBottomPos = chargeBottom.position;
    }
    public void ChargingUI(float charge)
    {
        chargeFill.fillAmount = charge;

        chargeFillLine.transform.position = Vector3.Lerp(chargeBottomPos, chargeTopPos, charge); 
        
    }
    void Charging(float charge)
    {
        //chargeMeter.transform.localPosition = new Vector3(0, Mathf.Sin(charge * Time.deltaTime), 0);
    }
    public void UpdateUI()
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
}
