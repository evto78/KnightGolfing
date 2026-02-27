using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private float damageMax;
    private float radiusMax;
    private float concentrationMax;
    private float durationMax;
    public string _name;
    public enum StatModifier
    {
        Fire,
        Ice,
        Poison,
        Regeneration
    }
    public StatModifier statMod;

    public float duration;
    public float concentration;
    public float damage;
    public float radius;

// Start is called before the first frame update
void Start()
    {
        Array temp = StatModifier.GetValues(typeof(StatModifier));
        statMod = (StatModifier)temp.GetValue((int)UnityEngine.Random.Range(0, 4));
        Debug.Log(statMod);

        switch (statMod)
        {
            case StatModifier.Fire:
                damageMax = 25;
                radiusMax = 20;
                concentrationMax = 3.5f;
                durationMax = 0.9f;
                break;

            case StatModifier.Ice:
                damageMax = 15;
                radiusMax = 10;
                concentrationMax = 10;
                durationMax = 20;
                break;

            case StatModifier.Poison:
                damageMax = 2.5f;
                radiusMax = 20;
                concentrationMax = 10;
                durationMax = 50;
                break;

            case StatModifier.Regeneration:
                damageMax = 3.5f;
                radiusMax = 3;
                concentrationMax = 10;
                durationMax = 45;
                break;
        }

        damage = UnityEngine.Random.Range(0.2f,damageMax);
        radius = UnityEngine.Random.Range(0.2f, radiusMax);
        concentration = UnityEngine.Random.Range(0.2f, concentrationMax);
        duration = UnityEngine.Random.Range(0.2f, durationMax);

        Debug.Log("dmg " + damage);
        Debug.Log("radius " + radius);
        Debug.Log("concentrtion " + concentration);
        Debug.Log("duration " + duration);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
