using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPost : MonoBehaviour
{
    public float goalSlime;
    // Start is called before the first frame update
    void Start()
    {
        goalSlime = 1.025f;
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 8)
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

            rb.velocity = rb.velocity / (goalSlime);

            if (other.gameObject.GetComponent<ballScript>().curState == ballScript.State.idle)
            {
                Destroy(gameObject);
            }
        }

    }
}
