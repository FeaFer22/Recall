using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class CuttingCounterController : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeScriptableObj[] cutRecipeSriptableObjArray;

    private int cuttingProgress;

    public override void Interact(PlayerInteractionController interactionController)
    {
        if (!HasKitchenObject())
        {
            if (interactionController.HasKitchenObject())
            {
                if(HasRecipeWithInput(interactionController.GetKitchenObject().GetKitchenObjectSO()))
                {
                    KitchenObject kitchenObject = interactionController.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);

                    InteractionLogicPlaceObjOnCounterServerRpc();
                }
            }
            else
            {

            }
        }
        else
        {
            if (interactionController.HasKitchenObject())
            {
                if (interactionController.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(interactionController);
            }
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void InteractionLogicPlaceObjOnCounterServerRpc()
    {
        InteractionLogicPlaceObjOnCounterClientRpc();
    }
    [ClientRpc]
    private void InteractionLogicPlaceObjOnCounterClientRpc()
    {
        cuttingProgress = 0;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = 0f
        });
    }

    public override void InteractAlternate(PlayerInteractionController interactionController)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            CutObjectServerRpc();
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc()
    {
        CutObjectClientRpc();
    }

    [ClientRpc]
    private void CutObjectClientRpc()
    {
        cuttingProgress++;

        OnCut?.Invoke(this, EventArgs.Empty);

        CuttingRecipeScriptableObj cuttingRecipeScriptableObj = GetCuttingRecipeScriptableObjWithInput(GetKitchenObject().GetKitchenObjectSO());

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = (float)cuttingProgress / cuttingRecipeScriptableObj.cuttingProgressMax
        });

        if (cuttingProgress >= cuttingRecipeScriptableObj.cuttingProgressMax)
        {
            KitchenObjectScriptableObject outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            
            KitchenObject.DestroyKitchenObject(GetKitchenObject());

            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
        }
    }

    private bool HasRecipeWithInput(KitchenObjectScriptableObject inputKitchenObjSO)
    {
        CuttingRecipeScriptableObj cuttingRecipeScriptableObj = GetCuttingRecipeScriptableObjWithInput(inputKitchenObjSO);
        return cuttingRecipeScriptableObj != null;
    }

    private KitchenObjectScriptableObject GetOutputForInput(KitchenObjectScriptableObject inputKitchenObjSO)
    {
        CuttingRecipeScriptableObj cuttingRecipeScriptableObj = GetCuttingRecipeScriptableObjWithInput(inputKitchenObjSO);
        if (cuttingRecipeScriptableObj != null)
        {
            return cuttingRecipeScriptableObj.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeScriptableObj GetCuttingRecipeScriptableObjWithInput(KitchenObjectScriptableObject inputKitchenObjSO)
    {
        foreach (CuttingRecipeScriptableObj recipeScriptableObj in cutRecipeSriptableObjArray)
        {
            if (recipeScriptableObj.input == inputKitchenObjSO)
            {
                return recipeScriptableObj;
            }
        }
        return null;
    }
}

