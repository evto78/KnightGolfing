using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    private float baseMoveSpeed;
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
        transform.rotation = new Quaternion(transform.rotation.x, transform.GetChild(1).rotation.y, transform.rotation.z, transform.rotation.w);
    }

}
