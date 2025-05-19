using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopStateManager : MonoBehaviour
{
    public static ShopStateManager Instance;

    public bool OWcrystalPurchased = false;
    public bool UWcrystalPurchased = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

