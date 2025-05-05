using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    //// public variables
    public Image[] crystalIcons;
    public TextMeshProUGUI move;
    public TextMeshProUGUI a_sp;
    public TextMeshProUGUI a_dam;
    public TextMeshProUGUI dam_red;

    //// private variables
    private static int curCrystals;
    private static List<int> stat_levels = new List<int> {1,1,1,1};
    //          0 = movement speed
    //          1 = attack speed
    //          2 = attack damage
    //          3 = damage reduction


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            updateStats(i, stat_levels[i]);
        }
        for (int i = 0; i < curCrystals + 1; i++)
        {
            gainedCrystal();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (curCrystals < 0) {curCrystals = 0;}
    }

    public void gainedCrystal()
    {
        crystalIcons[curCrystals].enabled = true;
        curCrystals++;
    }

    public void lostCrystal()
    {
        crystalIcons[curCrystals].enabled = false;
        curCrystals--;
    }

    public void updateStats(int stat_index, int level)
    {
        // stat_index has 4 options :
        //          0 = movement speed
        //          1 = attack speed
        //          2 = attack damage
        //          3 = damage reduction

        switch (stat_index)
        {
            case 0: // movement speed
                move.text = level.ToString();
                stat_levels[stat_index] = level;
                break;
            case 1: // attack speed
                a_sp.text = level.ToString();
                stat_levels[stat_index] = level;
                break;
            case 2: // attack damage
                a_dam.text = level.ToString();
                stat_levels[stat_index] = level;
                break;
            case 3: // damage reduction
                dam_red.text = level.ToString();
                stat_levels[stat_index] = level;
                break;
            default:
                Debug.Log("Did not enter a correct stat type");
                break;
        }

    }
}
