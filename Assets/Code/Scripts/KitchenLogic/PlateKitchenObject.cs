using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectScriptableObject kithcenObjectSO;
    }

    [SerializeField] private List<KitchenObjectScriptableObject> validKitchenObjectSOList;
    private List<KitchenObjectScriptableObject> kitchenObjectScriptableObjectList;

    protected override void Awake()
    {
        base.Awake();
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

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs 
            { 
                kithcenObjectSO = kitchenObjectScriptableObject
            });

            return true;
        }

    }

    public List<KitchenObjectScriptableObject> GetKitchenObjectSOList()
    {
        return kitchenObjectScriptableObjectList;
    }
}
