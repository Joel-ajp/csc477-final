using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reflective Material", menuName = "Materials/ReflectiveMaterial")]
public class ReflectiveMaterial : ScriptableObject
{
    public enum Reflect_property
    {
        REFLECT,
        REFRACT,
        NONE
    }

    public Reflect_property materialProperty;
    public float refractiveIndex = 1.0f;
}
