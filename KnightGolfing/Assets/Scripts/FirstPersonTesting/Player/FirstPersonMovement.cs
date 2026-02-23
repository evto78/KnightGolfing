using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    private float baseMoveSpeed;
    private float lookSpeed = 3;
    Vector2 rotation = Vector2.zero;


    [SerializeField] FirstPersonPlayerInput playerInput;
    void Start()
    {
        baseMoveSpeed = 15.0f;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (playerInput.GetMoveState() == FirstPersonPlayerInput.PlayerMoveState.Moving)
        {
            transform.position += playerInput.GetVector() * baseMoveSpeed * Time.deltaTime;
        }
        rotation.x += -Input.GetAxis("Mouse Y");
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x = Mathf.Clamp(rotation.x, -30, 30);

        transform.eulerAngles = new Vector2()

    }

}
