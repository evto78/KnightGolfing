using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    //Manager Scripts
    PlayerMovement pMvt;
    PlayerUI pUI;
    PlayerInput pInput;

    [Header("Categories")]
    public List<ItemObject> itemData;

    List<ItemObject> peasantItems;
    List<ItemObject> knightItems;
    List<ItemObject> kingItems;
    List<ItemObject> wizardItems;
    public List<List<ItemObject>> rarityList;

    public List<ItemObject> ballItems;
    public List<ItemObject> clubItems;
    public List<ItemObject> armorItems;
    public List<ItemObject> itemItems;

    [Header("Inventory")]
    public List<ItemObject> heldItems;
    public List<ItemObject> heldBalls; //max 3 held at once
    public List<ItemObject> heldClubs; //max 3 held at once
    public ItemObject heldArmor; //max 1 equipped

    public List<ClubScript> backpactClubSlots; public GameObject emptyClubMesh;

    [Header("ItemVariables")]
    public float heartyMeadCooldown = 15f;
    public int bowlerNudges = 0;
    public int kingGolfCrownInfluence = 0;

    private void Awake()
    {
        pMvt = GetComponent<PlayerMovement>();
        pUI = GetComponent<PlayerUI>();
        pInput = GetComponent<PlayerInput>();
    }
    void Start()
    {
        itemData = new List<ItemObject>(); itemData.AddRange(Resources.LoadAll<ItemObject>("Items")); SortItemData();
        LoadCategories();

        //Starting equipment
        heldItems = new List<ItemObject>();
        heldBalls = new List<ItemObject>(); heldBalls.Add(itemData[0]);
        heldClubs = new List<ItemObject>(); heldClubs.Add(itemData[1]);
        heldArmor = itemData[2];
    }
    void LoadCategories()
    {
        peasantItems = new List<ItemObject>();
        knightItems = new List<ItemObject>();
        kingItems = new List<ItemObject>();
        wizardItems = new List<ItemObject>();
        rarityList = new List<List<ItemObject>>();

        ballItems = new List<ItemObject>();
        clubItems = new List<ItemObject>();
        armorItems = new List<ItemObject>();
        itemItems = new List<ItemObject>();

        foreach (ItemObject item in itemData)
        {
            switch (item.rarity)
            {
                case ItemObject.Rarity.peasant: peasantItems.Add(item); break;
                case ItemObject.Rarity.knight: knightItems.Add(item); break;
                case ItemObject.Rarity.king: kingItems.Add(item); break;
                case ItemObject.Rarity.wizard: wizardItems.Add(item); break;
            }
            switch (item.type)
            {
                case ItemObject.Type.item: itemItems.Add(item); break;
                case ItemObject.Type.ball: ballItems.Add(item); break;
                case ItemObject.Type.club: clubItems.Add(item); break;
                case ItemObject.Type.armor: armorItems.Add(item); break;
            }
        }

        rarityList.Add(peasantItems);
        rarityList.Add(knightItems);
        rarityList.Add(kingItems);
        rarityList.Add(wizardItems);
    }
    void SortItemData()
    {
        List<int> comparisonList = new List<int>();
        List<ItemObject> sortedItemData = new List<ItemObject>();
        for (int i = 0; i < itemData.Count; i++) { comparisonList.Add(i); sortedItemData.Add(null); }
        for (int i = 0; i < itemData.Count; i++)
        {
            sortedItemData[comparisonList.IndexOf(itemData[i].id)] = itemData[i];
        }
        itemData = sortedItemData;
    }
    void Update()
    {
        ItemStatUpdate();
        pUI.UpdateUI();
        ClubUpdate();
    }
    public void ItemStatUpdate()
    {
        foreach (ItemObject item in heldItems)
        {
            switch (item.id)
            {
                case 9: heartyMeadCooldown = 15 / item.itemInfo.stacks; break;
                case 10: bowlerNudges = item.itemInfo.stacks; break;
                case 11: kingGolfCrownInfluence = item.itemInfo.stacks; break;
            }
        }
    }
    public void ClubUpdate()
    {
        for (int i = 0; i < backpactClubSlots.Count; i++)
        {
            if (heldClubs.Count-1 < i) { backpactClubSlots[i].SetMesh(emptyClubMesh); }
            else
            {
                backpactClubSlots[i].SetMesh(heldClubs[i].clubInfo.meshPrefab);
            }
        }
    }
}
