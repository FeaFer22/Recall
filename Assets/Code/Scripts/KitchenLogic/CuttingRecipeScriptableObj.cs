using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeScriptableObj : ScriptableObject
{
    public KitchenObjectScriptableObject input;
    public KitchenObjectScriptableObject output;
}
