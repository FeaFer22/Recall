using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CuttingCounterController : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;


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
                    interactionController.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeScriptableObj cuttingRecipeScriptableObj = GetCuttingRecipeScriptableObjWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeScriptableObj.cuttingProgressMax
                    });
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
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(interactionController);
            }
        }
    }

    public override void InteractAlternate(PlayerInteractionController interactionController)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            cuttingProgress++;
            CuttingRecipeScriptableObj cuttingRecipeScriptableObj = GetCuttingRecipeScriptableObjWithInput(GetKitchenObject().GetKitchenObjectSO());
            
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeScriptableObj.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeScriptableObj.cuttingProgressMax)
            {
                KitchenObjectScriptableObject outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
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
