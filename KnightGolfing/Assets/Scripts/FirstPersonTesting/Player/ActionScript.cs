using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionScript : MonoBehaviour
{
    public GameObject ball;
    public List<GameObject> hitboxes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Light_1();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Light_2();
        }
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            Heavy_1();
        }
        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift))
        {
            Heavy_2();
        }


    }

    void Light_1()
    {
        GameObject _hitbox = hitboxes[0];
        _hitbox.SetActive(true);

    }

    void Light_2()
    {

    }

    void Heavy_1()
    {

    }

    void Heavy_2()
    {

    }
}

