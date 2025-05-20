using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Properties;

public class InventoryManager : MonoBehaviour
{
    //// public variables
    [Header("Crystal Inventory Icons")]
    public GameObject red;
    public GameObject orange;
    public GameObject yellow;
    public GameObject green;
    public GameObject purple;
    public GameObject pink;

    [Header("UI Stuff")]
    public TextMeshProUGUI move;
    public TextMeshProUGUI a_sp;
    public TextMeshProUGUI a_dam;
    public TextMeshProUGUI dam_red;
    private GameObject _player;
    private PlayerLives lives;
    public Dictionary<crystalColor, GameObject> crystalIcons = new Dictionary<crystalColor, GameObject>();
    private static List<crystalColor> heldCrystals = new List<crystalColor> { };

    //// private variables
    public int curCrystals = 0;
    private static List<int> stat_levels = new List<int> { 1, 1, 1, 1 };
    //          0 = movement speed
    //          1 = attack speed
    //          2 = attack damage
    //          3 = damage reduction


    // Start is called before the first frame update
    void Start()
    {
        // set the dictionary values for reference
        crystalIcons[crystalColor.RED] = red;
        crystalIcons[crystalColor.ORANGE] = orange;
        crystalIcons[crystalColor.YELLOW] = yellow;
        crystalIcons[crystalColor.GREEN] = green;
        crystalIcons[crystalColor.PURPLE] = purple;
        crystalIcons[crystalColor.PINK] = pink;

        for (int i = 0; i < 4; i++)
        {
            setStats(i, stat_levels[i]);
        }
        foreach (var c in heldCrystals)
        {
            gainedCrystal(c);
        }

        _player = GameObject.FindGameObjectWithTag("Player");
        _player.GetComponent<PlayerStats>().updateStats(stat_levels);
        lives = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLives>();
    }

    // Update is called once per frame
    void Update()
    {
        if (curCrystals >= 6)
        {
            _player.GetComponent<PlayerLives>().kill();
        }
    }

    public void gainedCrystal(crystalColor color)
    {
        crystalIcons[color].SetActive(true);
        heldCrystals.Add(color);
        curCrystals++;
        
        // Print the newly acquired crystal
        Debug.Log("Player picked up a " + color.ToString() + " crystal!");
        
        // Print all crystals the player currently has
        PrintCrystalInventory();
    }
    
    // New method to print all crystals in inventory
    private void PrintCrystalInventory()
    {
        string crystalList = "Current Crystal Inventory (" + curCrystals + " total): ";
        
        if (heldCrystals.Count == 0)
        {
            crystalList += "Empty";
        }
        else
        {
            for (int i = 0; i < heldCrystals.Count; i++)
            {
                crystalList += heldCrystals[i].ToString();
                
                // Add comma if not the last item
                if (i < heldCrystals.Count - 1)
                {
                    crystalList += ", ";
                }
            }
        }
        
        Debug.Log(crystalList);
    }

    // New method to check if a crystal color is already in inventory
    public bool HasCrystal(crystalColor color)
    {
        return heldCrystals.Contains(color);
    }

    /*public void lostCrystal(crystalColor color)
    {
        crystalIcons[color].enabled = false;
        curCrystals--;
    }*/

    public void updateStat(int stat_index)
    {
        // stat_index has 4 options :
        //          0 = movement speed
        //          1 = attack speed
        //          2 = attack damage
        //          3 = damage reduction

        switch (stat_index)
        {
            case 0: // movement speed
                stat_levels[stat_index] = stat_levels[stat_index] + 1;
                move.text = stat_levels[stat_index].ToString();
                break;
            case 1: // attack speed
                stat_levels[stat_index] = stat_levels[stat_index] + 1;
                a_sp.text = stat_levels[stat_index].ToString();
                break;
            case 2: // attack damage
                stat_levels[stat_index] = stat_levels[stat_index] + 1;
                a_dam.text = stat_levels[stat_index].ToString();
                break;
            case 3: // damage reduction
                lives.gainHearts();
                stat_levels[stat_index] = stat_levels[stat_index] + 1;
                dam_red.text = stat_levels[stat_index].ToString();
                break;
            default:
                Debug.Log("Did not enter a correct stat type");
                break;
        }

        _player.GetComponent<PlayerStats>().updateStats(stat_levels);
    }

    public void setStats(int stat_index, int level)
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
    
    // Optional: Add a public method to print crystals on demand from other scripts
    public void PrintCurrentInventory()
    {
        PrintCrystalInventory();
    }
}