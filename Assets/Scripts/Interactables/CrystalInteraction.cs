using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum crystalColor
{
    RED,
    YELLOW,
    GREEN,
    ORANGE,
    PURPLE,
    PINK
}
public class CrystalInteraction : EInteractable
{
    private InventoryManager _inventoryManager;
    public crystalColor color;
    //public string color;
    // Start is called before the first frame update
    void Start()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();
        if (_inventoryManager != null)
        {
            if (_inventoryManager.HasCrystal(color))
            {
                Debug.Log("Already have this crystal");
                Destroy(gameObject);
            }
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
            _inventoryManager.gainedCrystal(color); // Add to Inventory
            SubmitScore.increaseScore(5000);
        }
        SoundManager.Instance.Play(SoundType.DING);
        Destroy(gameObject);
    }
}