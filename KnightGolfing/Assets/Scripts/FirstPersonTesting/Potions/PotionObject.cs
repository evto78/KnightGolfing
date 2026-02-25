using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Potion/Create New Potion")]
public class PotionObject : ScriptableObject
{
    //Constructors/Destructors
    public PotionObject()
    {
        Array temp = StatModifier.GetValues(typeof(StatModifier));
        statMod = (StatModifier)temp.GetValue((int)UnityEngine.Random.Range(0, 4));
        Debug.Log(statMod);

        duration = 3.0f;
        concentration = 3.0f;
        damage = 3.0f;
        radius = 3.0f;
    }


    //Members
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
}