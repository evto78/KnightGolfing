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


    // Start is called before the first frame update
    void Start()
    {
        _ui = GetComponent<PlayerUI>();
        input = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        _Input();
    }

    void _Input()
    {
        if (Input.GetMouseButton(0))
        {
            SwingCharge();
        }
    }

    void SwingCharge()
    {
        if (chargeBar < 0f || chargeBar > chargeBarMAX)
        {
            chargeSPEED *= -1;
        }
        chargeBar += chargeSPEED * Time.deltaTime;
        gameObject.SendMessage("Charging", chargeBar);
    }
}
