using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounterController : BaseCounter, IKitchenObjectParent
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
                if(interactionController.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    }
                }
                else
                {
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(interactionController.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            KitchenObject.DestroyKitchenObject(interactionController.GetKitchenObject());
                        }
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(interactionController);
            }
        }
    }

}
