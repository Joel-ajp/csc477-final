using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public Image[] crystalIcons;
    private int maxCrystals = 6;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gainedCrystal(int crystalNum)
    {
        crystalIcons[crystalNum].enabled = crystalNum <= maxCrystals;
    }

    public void updateStats(string stat, int level)
    {
        
    }
}
