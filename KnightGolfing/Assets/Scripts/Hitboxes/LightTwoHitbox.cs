using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTwoHitbox : MonoBehaviour
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
            ballScript ball = collision.gameObject.GetComponent<ballScript>();
            ball.Launch(50f, Vector3.up);
        }

    }
}
