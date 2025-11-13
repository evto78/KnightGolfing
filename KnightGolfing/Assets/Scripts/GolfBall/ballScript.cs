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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("hitbox"))
        {
            float power = collision.gameObject.GetComponent<PlayerInput>().chargePower;
            
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, collision.transform.rotation.z, transform.rotation.w);
            rb.velocity = new Vector3(0, power, power*100);
        }
    }
}
