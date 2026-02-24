using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Potion", menuName = "Potion/Create New Potion")]
public class PotionObject : ScriptableObject
{
    public enum StatModifier
    {
        Fire,
        Ice,
        Poison,
        Regeneration
    }
    public StatModifier statMod;

    public enum PotionType
    {
        Drinking,
        Throwing,
        Infusion
    }
    public PotionType potionType;

    public float duration;
    public float concentration;

}