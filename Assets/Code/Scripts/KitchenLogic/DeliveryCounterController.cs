using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounterController : BaseCounter
{
    public override void Interact(PlayerInteractionController playerController)
    {
        if (playerController.HasKitchenObject())
        {
            if(playerController.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {

                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);

                playerController.GetKitchenObject().DestroySelf();
            }
        }
    }
}
