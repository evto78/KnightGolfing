using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    private float baseMoveSpeed;


    //[SerializeField] FirstPersonPlayerInput playerInput;
    void Start()
    {
        baseMoveSpeed = 15.0f;
    }

    // Update is called once per frame
    void Update()
    { 

        transform.position += transform.forward * Input.GetAxis("Vertical") * baseMoveSpeed * Time.deltaTime;
        transform.position += transform.right * Input.GetAxis("Horizontal") * baseMoveSpeed * Time.deltaTime;
    }

}
