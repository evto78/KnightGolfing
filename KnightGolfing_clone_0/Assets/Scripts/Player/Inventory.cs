using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] PlayerItem m_playerItem;
    GameObject m_heldItem;
    [SerializeField] Transform m_hand;
    int m_inventorySlot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            m_heldItem = HoldItem(m_playerItem.heldClubs[0].GameObject());
            m_inventorySlot = 1;
        }
    }

    GameObject HoldItem(GameObject _item)
    {
        Transform _itemTransform = _item.GetComponent<Transform>();
        _itemTransform.localPosition = Vector3.zero;
        _itemTransform.parent = m_hand;
        return _item;
    }
}
