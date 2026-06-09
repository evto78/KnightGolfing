using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Vector3 launchVector;

    public float timer = 0.5f;
    void Update()
    {

    }

    public void Timeout(GameObject obj)
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 0.5f;
            obj.SetActive(false);
        }
    }


}
