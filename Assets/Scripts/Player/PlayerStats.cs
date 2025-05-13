using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public int movement_speed = 0;
    public int attack_speed = 0;
    public int attack_damage = 0;
    public int defense = 0;

    public void updateStats(List<int> levels)
    {
        movement_speed = levels[0];
        attack_speed = levels[1];
        attack_damage = levels[2];
        defense = levels[3];
    }
}