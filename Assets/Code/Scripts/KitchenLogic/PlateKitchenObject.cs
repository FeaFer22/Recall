using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
            AddIngredientServerRpc(GameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObjectScriptableObject));

            return true;
        }

    }

    [ServerRpc(RequireOwnership = false)]
    private void AddIngredientServerRpc(int kitchenObjectSOIndex)
    {
        AddIngredientClientRpc(kitchenObjectSOIndex);
    }
    [ClientRpc]
    private void AddIngredientClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectScriptableObject kitchenObjectSO = GameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        kitchenObjectScriptableObjectList.Add(kitchenObjectSO);

        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
        {
            kithcenObjectSO = kitchenObjectSO
        });
    }

    public List<KitchenObjectScriptableObject> GetKitchenObjectSOList()
    {
        return kitchenObjectScriptableObjectList;
    }
}
