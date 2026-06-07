using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    private float baseMoveSpeed;
    [SerializeField] GameObject other;
    [SerializeField] Rigidbody rb;
    public enum InteractState { Interacting, Not_Interacting};
    public InteractState interactState;
    public GameObject head;
    Vector3 m_movementVector;
    //[SerializeField] FirstPersonPlayerInput playerInput;
    void Start()
    {
        baseMoveSpeed = 15.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        m_movementVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += transform.forward * m_movementVector.z * baseMoveSpeed * Time.deltaTime;
        transform.position += transform.right * m_movementVector.x * baseMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E) && interactState == InteractState.Interacting)
        {
            other.gameObject.SendMessage("Interact");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            
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
