using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static FirstPersonPlayerInput;

public class FirstPersonPlayerInput : MonoBehaviour
{
    private Vector3 movementVector;
    private Vector2 lookVector;
    public KeybindData keybinds;

    public enum PlayerMoveState 
    {
        Moving,
        Sprinting,
        Crouching,
        Jumping,
        Idle
    }
    [SerializeField] private PlayerMoveState currentState;


    // Start is called before the first frame update
    void Start()
    {
        keybinds = new KeybindData();
        keybinds.ResetKeybinds();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }


    void HandleInput()
    {
        movementVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (movementVector.x + movementVector.y + movementVector.z != 0)
        {
            currentState = PlayerMoveState.Moving;
        }
        else
        {
            currentState = PlayerMoveState.Idle;
        }

        
    }

    public Vector3 GetVector()
    {
        return movementVector;
    }
    
    public PlayerMoveState GetMoveState()
    {
        return currentState;
    }


}
