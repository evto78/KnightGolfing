using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
[System.Serializable]
public class ItemObject : ScriptableObject
{
    public int id;
    public Sprite sprite;
    public string itemName;
    public List<string> statBuffs;
    public List<string> statDebuffs;
    public string activateAbility;
    public bool hasActive;
    public float activeCooldown;
    public string description;
    public string flavorText;

    [System.Serializable]
    public enum Rarity { peasant, knight, king, wizard }
    public Rarity rarity;

    [System.Serializable]
    public enum Type { item, club, ball, armor}
    public Type type;

    public ItemProperties itemInfo;
    public ClubProperties clubInfo;
    public BallProperties ballInfo;
    public ArmorProperties armorInfo;

    [System.Serializable]
    public class ItemProperties
    {
        public int stacks;
    }
    [System.Serializable]
    public class ClubProperties
    {
        public GameObject meshPrefab;
        public float force;
        public float control;
        public float swingSpeed;
    }
    [System.Serializable]
    public class BallProperties
    {
        public GameObject prefab;
        public float mass;
        public int pierce;
        public float airDrag;
        public float bounce;
    }
    [System.Serializable]
    public class ArmorProperties
    {
        public float speedMod;
        public float jumpMod;
    }
}
