using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private FirstPersonPlayerInput playerInput;
    private Vector3 initialPosition;
    private float yOffset;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //ViewBobbing();
        Look();
    }

    void ViewBobbing()
    {
        Vector3 moveDirection = playerInput.GetVector();
        yOffset = Mathf.Sin(timer * 5) * 5f;

        if (playerInput.GetMoveState() == FirstPersonPlayerInput.PlayerMoveState.Moving)
        {
            timer += 0.5f * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, initialPosition.y + yOffset, transform.position.z);
        }
        else
        {
            Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, initialPosition.y, transform.position.z), 0.55f);
            timer = 0;
        }

    }

    void Look()
    {
        Vector2 look = playerInput.GetLookVector();
        Quaternion.AngleAxis(look.x, Vector3.right);
    }
}
