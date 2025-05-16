using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalInteraction : EInteractable
{
    private InventoryManager _inventoryManager;
    //public string color;
    // Start is called before the first frame update
    void Start()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();
        if (_inventoryManager == null)
        {
            //Debug.LogError("uhh why is it null?");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void DoInteract() // Changes DoInteract in EInteractable to this, can be used with same logic
    {
        if (_inventoryManager != null)
        {
            _inventoryManager.gainedCrystal(); // Add to Inventory
        }
        SoundManager.Instance.Play(SoundType.DING);
        Destroy(gameObject);
    }
}
