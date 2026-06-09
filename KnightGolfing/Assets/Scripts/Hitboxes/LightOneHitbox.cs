using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOneHitbox : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 launchDir;
    float timer;
    void Start()
    {
        launchDir = new Vector3(0, 0, 0);
    }

    // Update is called once per frame

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 0.5f;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("ball"))
        {
            Vector3 launch = new Vector3(transform.forward.x, transform.forward.y + 0.25f, transform.forward.z + 0.5f);
            ballScript ball = collision.gameObject.GetComponent<ballScript>();
            ball.Launch(50f, launch);
        }
        
    }
}
