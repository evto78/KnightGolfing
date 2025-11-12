using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVISWING : MonoBehaviour
{
    PlayerUI _ui;
    PlayerInput input;
    PlayerMovement movement;
    private float chargeBar;
    public float chargeBarMAX;
    [SerializeField] private float chargeSPEED;

    //Math
    float x;
    float y;
    float xStart;
    float Tau;
    float xFinish;
    float progress;

    // Start is called before the first frame update
    void Start()
    {
        xStart = 0;
        Tau = 2 * Mathf.PI;
        xFinish = Tau;
        _ui = GetComponent<PlayerUI>();
        input = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageInput();
    }

    void ManageInput()
    {
        if (Input.GetMouseButton(0))
        {
            SwingCharge();
        }
    }

    void SwingCharge()
    {
        


        gameObject.SendMessage("Charging", y);
        Debug.Log(chargeBar);
    }
}
