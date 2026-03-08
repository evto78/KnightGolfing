using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    private float baseMoveSpeed;

    public GameObject head;
    //[SerializeField] FirstPersonPlayerInput playerInput;
    void Start()
    {
        baseMoveSpeed = 15.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        transform.position += transform.forward * Input.GetAxis("Vertical") * baseMoveSpeed * Time.deltaTime;
        transform.position += transform.right * Input.GetAxis("Horizontal") * baseMoveSpeed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(head.transform.position, head.transform.TransformDirection(Vector3.forward), 
            out hit, 5.0f))
        {
            Debug.DrawRay(head.transform.position, head.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            if (hit.collider.CompareTag("Interactable")) { hit.collider.gameObject.SendMessage("Interact");}
        }
        else
        {
            Debug.DrawRay(head.transform.position, head.transform.TransformDirection(Vector3.forward) * 5, Color.blue);
        }
    }

}
