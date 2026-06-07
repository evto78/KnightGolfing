using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    Vector3 launchVector;

    float timer = 0.5f;
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 0.5f;
            gameObject.SetActive(false);
        }
    }

    void SetLaunch(Vector3 _launch)
    {
        launchVector = _launch;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ball"))
        {
            ballScript ball = collision.gameObject.GetComponent<ballScript>();
            ball.Launch(5f, launchVector);
        }
    }
}
