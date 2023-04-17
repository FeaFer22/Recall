using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CuttingCounterController : BaseCounter
{
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }


    [SerializeField] private CuttingRecipeScriptableObj[] cutRecipeSriptableObjArray;

    private int cuttingProgress;

    public override void Interact(PlayerInteractionController interactionController)
    {
        if (!HasKitchenObject())
        {
            if (interactionController.HasKitchenObject())
            {
                if(HasRecipeWithInput(interactionController.GetKitchenObject().GetKitchenObject()))
                {
                    interactionController.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeScriptableObj cuttingRecipeScriptableObj = GetCuttingRecipeScriptableObjWithInput(GetKitchenObject().GetKitchenObject());

                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
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

            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(interactionController);
            }
        }
    }

    public override void InteractAlternate(PlayerInteractionController interactionController)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObject()))
        {
            cuttingProgress++;
            CuttingRecipeScriptableObj cuttingRecipeScriptableObj = GetCuttingRecipeScriptableObjWithInput(GetKitchenObject().GetKitchenObject());
            
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeScriptableObj.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeScriptableObj.cuttingProgressMax)
            {
                KitchenObjectScriptableObject outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObject());
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
