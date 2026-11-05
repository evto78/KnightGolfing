using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class KeybindData
{
    [Header("Movement")]
    public KeyCode walkForward;
    public KeyCode walkLeft;
    public KeyCode walkRight;
    public KeyCode walkBackward;
    public KeyCode sprint;
    public KeyCode jump;
    public KeyCode slide;

    [Header("Golfing")]
    public KeyCode swing;
    public KeyCode interact;
    public KeyCode changeClub;
    public KeyCode changeBall;
    public void ResetKeybinds()
    {
        walkForward = KeyCode.W;
        walkLeft = KeyCode.A;
        walkRight = KeyCode.D;
        walkBackward = KeyCode.S;
        sprint = KeyCode.LeftShift;
        jump = KeyCode.Space;
        slide = KeyCode.LeftControl;

        swing = KeyCode.Mouse0;
        interact = KeyCode.F;
        changeClub = KeyCode.E;
        changeBall = KeyCode.Q;
    }

    public void FirstPersonKeybinds()
    {
        return;
    }
}
