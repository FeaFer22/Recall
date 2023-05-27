using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectScriptableObject> validKitchenObjectSOList;
    private List<KitchenObjectScriptableObject> kitchenObjectScriptableObjectList;

    private void Awake()
    {
        kitchenObjectScriptableObjectList = new List<KitchenObjectScriptableObject>();
    }
    public bool TryAddIngredient(KitchenObjectScriptableObject kitchenObjectScriptableObject)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectScriptableObject))
        {
            return false;
        }
        if (kitchenObjectScriptableObjectList.Contains(kitchenObjectScriptableObject))
        {
            return false;
        }
        else
        {
            kitchenObjectScriptableObjectList.Add(kitchenObjectScriptableObject);
            return true;
        }
    }
}
