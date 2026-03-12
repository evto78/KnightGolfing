using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    private float baseMoveSpeed;
    [SerializeField] GameObject other;
    public enum InteractState { Interacting, Not_Interacting};
    public InteractState interactState;
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
        if (Input.GetKey(KeyCode.E) && interactState == InteractState.Interacting)
        {
            other.gameObject.SendMessage("Interact");
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(head.transform.position, head.transform.TransformDirection(Vector3.forward), 
            out hit, 5.0f))
        {
            Debug.DrawRay(head.transform.position, head.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            if (hit.collider.CompareTag("Interactable")) 
            { 
                interactState = InteractState.Interacting;
                other = hit.collider.gameObject;
            }
        }
        else
        {
            interactState = InteractState.Not_Interacting;
            Debug.DrawRay(head.transform.position, head.transform.TransformDirection(Vector3.forward) * 5, Color.blue);
        }
    }

}
