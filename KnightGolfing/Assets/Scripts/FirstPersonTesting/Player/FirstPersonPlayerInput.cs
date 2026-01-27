using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayerInput : MonoBehaviour
{
    public Vector2 movementVector;
    public KeybindData keybinds;

    public enum playerMoveState 
    {
        Moving,
        Sprinting,
        Crouching,
        Jumping
    }


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
        if (Input.GetKey(keybinds.walkForward))
        {
            movementVector.y = 1;
        }
        if (Input.GetKey(keybinds.walkBackward))
        {
            movementVector.y = -1;
        }
        if (Input.GetKey(keybinds.walkLeft))
        {
            movementVector.x = -1;
        }
        if (Input.GetKey(keybinds.walkRight))
        {
            movementVector.x = 1;
        }




    }

}
