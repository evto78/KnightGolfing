using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class ballScript : MonoBehaviour
{
    public GameObject player;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("hitbox"))
        {
            float power = other.gameObject.GetComponentInParent<PlayerInput>().chargePower;
            Debug.Log("hit");
            transform.rotation = new Quaternion(transform.rotation.x, other.transform.rotation.y, transform.rotation.z, transform.rotation.w);
            rb.velocity += Vector3.forward * power * 25;
        }
    }
}
