using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistPLayer : MonoBehaviour
{
    // Start is called before the first frame update
   void Awake()
    {
            GameObject root = transform.root.gameObject;
            DontDestroyOnLoad(root);
    }
}
