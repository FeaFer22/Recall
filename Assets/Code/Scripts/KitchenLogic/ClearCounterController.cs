using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounterController : BaseCounter
{
    [SerializeField] private KitchenObjectScriptableObject kitchenObjectSO;

    public override void Interact(PlayerInteractionController interactionController)
    {
        if(!HasKitchenObject())
        {
            if (interactionController.HasKitchenObject())
            {
                interactionController.GetKitchenObject().SetKitchenObjectParent(this);
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

}
