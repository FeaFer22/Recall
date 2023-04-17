using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CuttingCounterController : BaseCounter
{

    [SerializeField] private CuttingRecipeScriptableObj[] cutRecipeSriptableObjArray;

    public override void Interact(PlayerInteractionController interactionController)
    {
        if (!HasKitchenObject())
        {
            if (interactionController.HasKitchenObject())
            {
                if(HasRecipeWithInput(interactionController.GetKitchenObject().GetKitchenObject()))
                {
                    interactionController.GetKitchenObject().SetKitchenObjectParent(this);
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
            KitchenObjectScriptableObject outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObject());
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
        }
    }


    private bool HasRecipeWithInput(KitchenObjectScriptableObject inputKitchenObjSO)
    {
        foreach (CuttingRecipeScriptableObj cuttingRecipeScriptableObj in cutRecipeSriptableObjArray)
        {
            if(cuttingRecipeScriptableObj.input == inputKitchenObjSO)
            {
                return true;
            }
        }
        return false;
    }

    private KitchenObjectScriptableObject GetOutputForInput(KitchenObjectScriptableObject inputKitchenObjSO)
    {
        foreach (CuttingRecipeScriptableObj recipeScriptableObj in cutRecipeSriptableObjArray)
        {
            if(recipeScriptableObj.input == inputKitchenObjSO)
            {
                return recipeScriptableObj.output;
            }
        }
        return null;
    }
}
