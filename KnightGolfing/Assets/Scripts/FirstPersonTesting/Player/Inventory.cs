using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> m_items;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddItem(Item _item)
    {
        if (m_items.Count > 3)
        {
            Debug.Log("Inventory Full");
            return;
        }

        m_items.Add(_item);

    }

    void RemoveItem(Item _item)
    {

    }
}
