using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<PotionObject> potions;

    // Start is called before the first frame update
    void Start()
    {
        FirePot.potionType = PotionObject.PotionType.Throwing;
        FirePot.statMod = PotionObject.StatModifier.Fire;
        FirePot.concentration = 55;
        FirePot.duration = 0.5f;

        potions.Add(FirePot);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
