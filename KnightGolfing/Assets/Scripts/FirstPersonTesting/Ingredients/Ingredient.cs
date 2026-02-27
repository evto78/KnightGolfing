using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ingredients/PotionIngredient")]
public class Ingredient : ScriptableObject
{
    [SerializeField] private string m_name;
    [SerializeField] private Sprite m_sprite;

    private enum StatModifier
    {
        Fire,
        Ice,
        Poison,
        Regeneration
    }
    [SerializeField] private StatModifier m_modifier;
    [SerializeField] private float m_strength;
}
