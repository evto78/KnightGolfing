using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
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
}
